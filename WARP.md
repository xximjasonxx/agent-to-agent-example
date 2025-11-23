# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Overview

This repository contains two main pieces:

- A .NET 9 MCP (Model Context Protocol) weather server under `mcp/Farrellsoft.MCP.Weather`.
- A React + Vite chat UI prototype under `frontend`.

They are currently independent: the backend exposes an MCP HTTP endpoint for weather lookup, and the frontend is a purely client-side chat prototype with stubbed responses.

## Common commands

### Backend: MCP Weather server (`mcp/Farrellsoft.MCP.Weather`)

The backend is a single ASP.NET Core project targeting .NET 9 (`Farrellsoft.MCP.Weather.csproj`).

- Restore and build:

  ```bash
  cd /Users/jfarrell/Projects/dotnet/net90/agent-to-agent-example
  dotnet restore mcp/Farrellsoft.MCP.Weather/Farrellsoft.MCP.Weather.csproj
  dotnet build mcp/Farrellsoft.MCP.Weather/Farrellsoft.MCP.Weather.csproj
  ```

- Run locally (Mac/zsh) — requires a WeatherAPI key in the `ApiKey` environment variable:

  ```bash
  cd /Users/jfarrell/Projects/dotnet/net90/agent-to-agent-example
  export ApiKey="YOUR_WEATHERAPI_KEY"
  dotnet run --project mcp/Farrellsoft.MCP.Weather/Farrellsoft.MCP.Weather.csproj
  ```

  This starts the ASP.NET Core app and exposes the MCP HTTP endpoint at `/mcp`.

- Tests:

  There are currently no .NET test projects in this repository. If tests are added later, prefer `dotnet test` with explicit project paths rather than relying on solution files (none exist at the moment).

### Frontend: React + Vite chat UI (`frontend`)

The frontend is a standard Vite + React app with scripts defined in `frontend/package.json`.

- Install dependencies (first time only):

  ```bash
  cd /Users/jfarrell/Projects/dotnet/net90/agent-to-agent-example/frontend
  npm install
  ```

- Run dev server:

  ```bash
  cd /Users/jfarrell/Projects/dotnet/net90/agent-to-agent-example/frontend
  npm run dev
  ```

- Build production bundle:

  ```bash
  cd /Users/jfarrell/Projects/dotnet/net90/agent-to-agent-example/frontend
  npm run build
  ```

- Lint frontend code:

  ```bash
  cd /Users/jfarrell/Projects/dotnet/net90/agent-to-agent-example/frontend
  npm run lint
  ```

- Preview built assets:

  ```bash
  cd /Users/jfarrell/Projects/dotnet/net90/agent-to-agent-example/frontend
  npm run preview
  ```

## Backend architecture: `mcp/Farrellsoft.MCP.Weather`

### High-level flow

The weather MCP server is structured into clear layers:

1. **Hosting and DI setup** (`Program.cs`)
2. **Configuration** (`Configuration/WeatherOptions.cs`)
3. **HTTP client for external weather API** (`Clients/IWeatherApiClient.cs`, `Clients/WeatherApiClient.cs`)
4. **MCP server interface and implementation** (`Mcp/IWeatherMcpServer.cs`, `Mcp/WeatherMcpServer.cs`)
5. **MCP tool surface / entrypoints** (`Mcp/WeatherTools.cs`)
6. **DTOs for tool contracts** (`Mcp/Contracts/*.cs`)

The typical request path for the main tool is:

`MCP HTTP request → ModelContextProtocol routing → WeatherTools.GetWeatherByPostalCodeAsync → IWeatherMcpServer → WeatherMcpServer → IWeatherApiClient → WeatherApiClient → external weather API → WeatherMcpServer JSON mapping → GetWeatherForPostalCodeResponse`

### Hosting and MCP wiring (`Program.cs`)

Key responsibilities:

- Creates a `WebApplication` and configures services.
- Binds `WeatherOptions` from configuration section `"Weather"`.
- Registers a typed `HttpClient` for `IWeatherApiClient` that sets its `BaseAddress` from `WeatherOptions.BaseUrl`, defaulting to `https://api.weatherapi.com/` when unset.
- Registers `WeatherApiClient` and `WeatherMcpServer` in DI:
  - `IWeatherApiClient` → `WeatherApiClient`
  - `IWeatherMcpServer` → `WeatherMcpServer`
- Sets up the MCP server using the `ModelContextProtocol.AspNetCore` package:
  - `AddMcpServer().WithHttpTransport().WithToolsFromAssembly()` — this discovers any classes annotated with `[McpServerToolType]` (such as `WeatherTools`).
- Maps the MCP endpoint:
  - `app.MapMcp("/mcp");`

When extending the MCP surface, prefer adding new tool methods to a `[McpServerToolType]`-annotated static class and keeping core logic in injectable services behind interfaces.

### Configuration (`Configuration/WeatherOptions.cs`)

`WeatherOptions` holds configuration for the external API:

- `ApiKey` (deprecated): retained for backward compatibility; the live implementation uses the `ApiKey` environment variable instead.
- `BaseUrl`: optional base URL for the weather API; if omitted, `WeatherApiClient` defaults to `https://api.weatherapi.com/`.

Configuration values can be supplied via standard ASP.NET Core configuration sources (environment variables, JSON, etc.), but there is no `appsettings.json` checked into this repo.

### External weather client (`Clients`)

`IWeatherApiClient` defines a single operation:

- `GetCurrentWeatherByPostalCodeAsync(string postalCode, CancellationToken)` → returns a `JsonElement` payload from the provider.

`WeatherApiClient` implements the interface and is responsible for:

- Validating that `postalCode` is non-empty.
- Reading the API key from the `ApiKey` environment variable (required at runtime).
- Building the request URL using `QueryHelpers.AddQueryString` to call the provider at:

  `v1/current.json?key=<key>&q=<postalCode>&aqi=no`

- Issuing the HTTP GET with the typed `HttpClient` and enforcing success via `EnsureSuccessStatusCode()`.
- Deserializing the response body into `JsonElement` using `ReadFromJsonAsync<JsonElement>()` and returning it.

When modifying provider integration (e.g., changing providers or adding fields), keep this logic localized in `WeatherApiClient` and adjust the DTO mapping in `WeatherMcpServer` accordingly.

### MCP server layer (`Mcp`)

`IWeatherMcpServer` is an abstraction over MCP-exposed operations; it currently defines:

- `GetWeatherForPostalCodeResponse GetWeatherByPostalCodeAsync(GetWeatherByPostalCodeRequest request, CancellationToken)`

`WeatherMcpServer` implements this by:

- Validating the request and its `PostalCode`.
- Calling `IWeatherApiClient.GetCurrentWeatherByPostalCodeAsync`.
- Projecting the provider JSON into a simplified DTO `GetWeatherForPostalCodeResponse` by reading:
  - `location.name` → `LocationName`
  - `location.region` → `LocationRegion`
  - `current.temp_f` → `Temp`
  - `current.wind_mph` → `Wind`
  - `current.condition.text` → `ConditionDescription`

This separation means you can evolve DTOs and mapping logic independently of the MCP tool signatures.

### MCP tool surface (`Mcp/WeatherTools.cs`)

`WeatherTools` is a static class marked with `[McpServerToolType]`, which makes its methods discoverable by `WithToolsFromAssembly()` in `Program.cs`.

The primary tool is:

- `GetWeatherByPostalCodeAsync(IWeatherMcpServer server, string postalCode, CancellationToken)` annotated with:
  - `[McpServerTool(Name = "GetWeatherByPostalCode")]`
  - `Description(...)` for tool metadata

It constructs a `GetWeatherByPostalCodeRequest` from the string `postalCode` and delegates to `server.GetWeatherByPostalCodeAsync`.

When adding new MCP tools:

- Prefer defining them on a `[McpServerToolType]`-annotated static class like `WeatherTools`.
- Accept interfaces (e.g., `IWeatherMcpServer` or other services) as parameters and perform only light orchestration in the tool method; keep business logic in injected services.

### DTOs / Contracts (`Mcp/Contracts`)

- `GetWeatherByPostalCodeRequest`: request payload containing a single `PostalCode` string.
- `GetWeatherForPostalCodeResponse`: response DTO with `LocationName`, `LocationRegion`, `PostalCode`, `Temp`, `Wind`, and `ConditionDescription`.

These types define the boundary both for the MCP tool and for any HTTP clients consuming the MCP server. When extending the data returned by the provider, update these contracts together with the mapping in `WeatherMcpServer`.

## Frontend architecture: `frontend`

The frontend is a minimal, self-contained chat UI designed to be wired up to a backend later.

### Structure

- Entry point: `src/main.jsx` renders `<App />` into the `root` DOM element under `StrictMode`.
- Main application component: `src/App.jsx`.
- UI components under `src/components`:
  - `ChatHeader.jsx`: static header with title, subtitle, and status badges.
  - `MessageList.jsx`: scrollable list of chat bubbles; auto-scrolls to the newest message via a `ref` + `useEffect`.
  - `MessageInput.jsx`: controlled textarea + submit button with Enter-to-send and Shift+Enter for newline behavior.

### Conversation model

- `App.jsx` holds all conversation state in React local state (`messages`, `isThinking`, `responseIndex`).
- It seeds an initial conversation with a few messages explaining that everything is running locally in memory.
- When a user sends a message:
  - The message is appended to `messages`.
  - `isThinking` is set to `true`.
  - A `setTimeout` schedules a stubbed assistant response using one of the `STUB_RESPONSES`, then clears `isThinking` and increments `responseIndex`.

Currently, **no network calls** are made from the frontend. To integrate with the backend or another agent service, replace the `setTimeout` block in `App.jsx` with a real API call that posts the user message and appends streamed or batched responses into `messages`.

### Linting and build

- ESLint is configured via `eslint.config.js`; the `npm run lint` script runs ESLint across the frontend.
- Vite config is minimal (`vite.config.js`), enabling the React plugin.

When introducing new tooling or build steps for the frontend, prefer wiring them through `package.json` scripts so they can be invoked easily via Warp.
