# NFL Schedule Loader

A Python application to fetch NFL schedules from ESPN's API and load them into a PostgreSQL database.

## Project Structure

```
load-nfl-schedules/
├── src/
│   ├── __init__.py          # Package initializer
│   ├── models.py            # Data models (Game class)
│   ├── espn_client.py       # ESPN API client
│   └── database.py          # PostgreSQL database operations
├── main.py                  # Main entry point
├── requirements.txt         # Python dependencies
└── README.md               # This file
```

## Features

- **Modular design**: Separate modules for models, API client, and database operations
- **Duplicate prevention**: Uses deterministic UUIDs based on season, week, and title
- **Upsert logic**: INSERT ON CONFLICT ensures re-runs don't duplicate data
- **Bulk operations**: Efficient batch inserts using psycopg2's execute_values
- **Error handling**: Graceful handling of API failures with detailed logging

## Prerequisites

- Python 3.9+
- PostgreSQL database server
- Database named `nfl-schedules` must exist

## Installation

1. Install dependencies:

```bash
pip install -r requirements.txt
```

2. Create a `.env` file with your database credentials:

```bash
PG_SERVER_HOST=your-postgres-host
PG_USER=your-username
PG_PASSWORD=your-password
```

**Note**: The `.env` file is automatically ignored by git to protect your credentials.

## Usage

Run the main script:

```bash
python main.py
```

The script will:
1. Create the `nfl_schedules` table if it doesn't exist
2. Fetch NFL schedules for 2024 and 2025 (weeks 1-17)
3. Insert/update games in the database
4. Display summary statistics and sample data

## Database Schema

```sql
CREATE TABLE nfl_schedules (
    id UUID PRIMARY KEY,
    season INTEGER NOT NULL,
    week_no INTEGER NOT NULL,
    date TIMESTAMP NOT NULL,
    title VARCHAR(255) NOT NULL,
    venue VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_season_week ON nfl_schedules(season, week_no);
```

## How Duplicates Are Prevented

Each game gets a deterministic UUID generated from:
- Season year
- Week number
- Game title

This ensures:
- The same game always gets the same ID
- Re-running the script updates existing records instead of creating duplicates
- The `ON CONFLICT (id) DO UPDATE` clause handles upserts automatically
