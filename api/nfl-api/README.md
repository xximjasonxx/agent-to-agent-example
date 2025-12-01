# NFL Schedule API

Azure Functions HTTP endpoint for querying NFL game schedules.

## Endpoint

```
GET /api/schedule/{season_number}/week/{week_number}
```

### Parameters

- `season_number` (int): NFL season year (must be between 2020 and 2030)
- `week_number` (int): Week number (must be between 1 and 17 inclusive)

### Authentication

Anonymous - no authentication required.

### Response

**Success (200 OK):**
```json
[
  {
    "title": "Kansas City Chiefs vs Baltimore Ravens",
    "venue": "GEHA Field at Arrowhead Stadium",
    "season": 2024,
    "weekNo": 1,
    "date": "2024-09-05"
  }
]
```

**Bad Request (400):**
```json
{
  "error": "Invalid season_number. Must be between 2020 and 2030."
}
```

or

```json
{
  "error": "Invalid week_number. Must be between 1 and 17 inclusive."
}
```

**Internal Server Error (500):**
```json
{
  "error": "An error occurred while retrieving the schedule."
}
```

## Configuration

The API requires the following environment variables to connect to PostgreSQL:

- `PG_HOST`: PostgreSQL server hostname
- `PG_USER`: Database username
- `PG_PASSWORD`: Database password
- `PG_DATABASE`: Database name

For local development, these are configured in `local.settings.json`.

## Running Locally

1. Ensure all environment variables are set in `local.settings.json`
2. Start the function app:
   ```bash
   func start
   ```
   or
   ```bash
   dotnet run
   ```

3. Test the endpoint:
   ```bash
   curl http://localhost:7071/api/schedule/2024/week/1
   ```

## Database Schema

The API queries the `nfl_schedules` table with the following schema:
- `id` (uuid) - Primary key
- `title` (varchar(120)) - Game title (e.g., "Team A vs Team B")
- `venue` (varchar(100)) - Game venue/location
- `season` (smallint) - Season year (2020-3000)
- `week_no` (smallint) - Week number (1-25)
- `date` (date) - Game date
- `created_at` (timestamp) - Record creation timestamp
- `updated_at` (timestamp) - Record update timestamp
