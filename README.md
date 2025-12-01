# Agent-to-Agent Example

A demonstration repository showcasing a multi-component architecture with MCP (Model Context Protocol) servers, REST APIs, a React frontend, and data loading utilities for NFL schedules.

## Repository Structure

```
.
├── frontend/           # React + Vite chat UI prototype
├── backend/            # .NET 9 ASP.NET Core chat backend
├── mcp/
│   ├── Farrellsoft.MCP.Weather/  # MCP Weather server
│   └── Farrellsoft.MCP.NFL/      # MCP NFL server (placeholder)
├── api/
│   └── nfl-api/        # Azure Functions NFL schedule API
└── data/
    └── load-nfl-schedules/  # Python data loader for NFL schedules
```

## Components

### Frontend (React + Vite)

A minimal React chat UI prototype with client-side stubbed responses.

**Location:** `frontend/`

**Features:**
- Chat interface with message bubbles
- Pure React state management (no server calls yet)
- Components: ChatHeader, MessageList, MessageInput

**Commands:**
```bash
cd frontend
npm install          # Install dependencies
npm run dev          # Start development server
npm run build        # Build production bundle
npm run lint         # Run ESLint
npm run preview      # Preview built assets
```

### Backend (ASP.NET Core)

A .NET 9 ASP.NET Core backend exposing a single chat endpoint.

**Location:** `backend/`

**Endpoint:**
- `POST /send` - Accepts `{ "request": "<message>", "threadId": "<optional>" }` and returns a response

**Commands:**
```bash
cd backend
dotnet restore       # Restore dependencies
dotnet build         # Build the project
dotnet run           # Run the server
```

### MCP Weather Server

A .NET 9 MCP (Model Context Protocol) server for weather lookups using [WeatherAPI](https://www.weatherapi.com/).

**Location:** `mcp/Farrellsoft.MCP.Weather/`

**MCP Tools:**
- `GetWeatherByPostalCode` - Retrieves current weather for a given postal code

**Configuration:**
- Set the `ApiKey` environment variable with your WeatherAPI key

**Commands:**
```bash
cd mcp/Farrellsoft.MCP.Weather
dotnet restore
dotnet build
ApiKey="YOUR_API_KEY" dotnet run
```

The MCP endpoint is exposed at `/mcp`.

### MCP NFL Server

A .NET 9 MCP server placeholder for NFL-related operations.

**Location:** `mcp/Farrellsoft.MCP.NFL/`

**Status:** Placeholder - MCP tools not yet implemented

**Commands:**
```bash
cd mcp/Farrellsoft.MCP.NFL
dotnet restore
dotnet build
dotnet run
```

The MCP endpoint is exposed at `/mcp`.

### NFL Schedule API (Azure Functions)

An Azure Functions HTTP endpoint for querying NFL game schedules from a PostgreSQL database.

**Location:** `api/nfl-api/`

**Endpoint:**
```
GET /api/schedule/{season_number}/week/{week_number}
```

**Parameters:**
- `season_number` (int): NFL season year (2020-2030)
- `week_number` (int): Week number (1-17)

**Configuration:**
Required environment variables for PostgreSQL connection:
- `PG_HOST` - PostgreSQL server hostname
- `PG_USER` - Database username
- `PG_PASSWORD` - Database password
- `PG_DATABASE` - Database name

**Commands:**
```bash
cd api/nfl-api
dotnet restore
dotnet build
func start            # Start with Azure Functions CLI
# or
dotnet run
```

See [api/nfl-api/README.md](api/nfl-api/README.md) for detailed API documentation.

### NFL Schedule Loader (Python)

A Python application to fetch NFL schedules from ESPN's API and load them into PostgreSQL.

**Location:** `data/load-nfl-schedules/`

**Features:**
- Fetches NFL schedules from ESPN API
- Loads data into PostgreSQL with upsert logic
- Prevents duplicates using deterministic UUIDs

**Prerequisites:**
- Python 3.9+
- PostgreSQL database

**Commands:**
```bash
cd data/load-nfl-schedules
pip install -r requirements.txt
python main.py
```

**Configuration:**
Create a `.env` file with:
```
PG_SERVER_HOST=your-postgres-host
PG_USER=your-username
PG_PASSWORD=your-password
```

See [data/load-nfl-schedules/README.md](data/load-nfl-schedules/README.md) for detailed documentation.

## Prerequisites

- **.NET 9 SDK** - For backend, MCP servers
- **.NET 8 SDK** - For Azure Functions (nfl-api)
- **Node.js 18+** - For frontend
- **Python 3.9+** - For data loader
- **PostgreSQL** - For NFL schedule data storage
- **Azure Functions Core Tools** - For running nfl-api locally (optional)

## License

See the repository for license information.
