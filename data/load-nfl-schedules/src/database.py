"""Database operations for NFL schedules."""

import os
from typing import List
import psycopg2
from psycopg2.extras import execute_values

from .models import Game


class NFLScheduleDatabase:
    """Handles PostgreSQL database operations for NFL schedules."""
    
    def __init__(self):
        """Initialize database connection using environment variables."""
        self.host = os.environ.get("PG_SERVER_HOST")
        self.user = os.environ.get("PG_USER")
        self.password = os.environ.get("PG_PASSWORD")
        self.database = "nfl-schedules"
        
        if not all([self.host, self.user, self.password]):
            raise ValueError(
                "Missing required environment variables: "
                "PG_SERVER_HOST, PG_USER, PG_PASSWORD"
            )
    
    def get_connection(self):
        """Create and return a database connection."""
        return psycopg2.connect(
            host=self.host,
            user=self.user,
            password=self.password,
            database=self.database
        )
    
    def create_table_if_not_exists(self):
        """Create the nfl_schedules table if it doesn't exist."""
        create_table_query = """
        CREATE TABLE IF NOT EXISTS nfl_schedules (
            id UUID PRIMARY KEY,
            season INTEGER NOT NULL,
            week_no INTEGER NOT NULL,
            date TIMESTAMP NOT NULL,
            title VARCHAR(255) NOT NULL,
            venue VARCHAR(255) NOT NULL,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        );
        
        ALTER TABLE nfl_schedules ADD COLUMN IF NOT EXISTS created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP;
        ALTER TABLE nfl_schedules ADD COLUMN IF NOT EXISTS updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP;

        CREATE INDEX IF NOT EXISTS idx_season_week ON nfl_schedules(season, week_no);
        """
        
        with self.get_connection() as conn:
            with conn.cursor() as cursor:
                cursor.execute(create_table_query)
                conn.commit()
    
    def upsert_games(self, games: List[Game]) -> tuple[int, int]:
        """
        Insert or update games in the database.
        
        Uses INSERT ... ON CONFLICT DO UPDATE to prevent duplicates.
        The ID is deterministically generated from season, week_no, and title.
        
        Args:
            games: List of Game objects to insert/update
        
        Returns:
            Tuple of (inserted_count, updated_count)
        """
        if not games:
            return 0, 0
        
        upsert_query = """
        INSERT INTO nfl_schedules (id, season, week_no, date, title, venue, created_at, updated_at)
        VALUES %s
        ON CONFLICT (id) 
        DO UPDATE SET
            season = EXCLUDED.season,
            week_no = EXCLUDED.week_no,
            date = EXCLUDED.date,
            title = EXCLUDED.title,
            venue = EXCLUDED.venue,
            updated_at = CURRENT_TIMESTAMP
        """
        
        # Prepare data for bulk insert
        values = [
            (
                str(game.generate_id()),
                game.season,
                game.week_no,
                game.date,
                game.title,
                game.venue,
                'NOW()',
                'NOW()'
            )
            for game in games
        ]
        
        with self.get_connection() as conn:
            with conn.cursor() as cursor:
                # Get count before
                cursor.execute("SELECT COUNT(*) FROM nfl_schedules")
                count_before = cursor.fetchone()[0]
                
                # Execute bulk upsert
                execute_values(cursor, upsert_query, values)
                conn.commit()
                
                # Get count after
                cursor.execute("SELECT COUNT(*) FROM nfl_schedules")
                count_after = cursor.fetchone()[0]
                
                inserted = count_after - count_before
                updated = len(games) - inserted
                
                return inserted, updated
    
    def get_game_count(self) -> int:
        """Get the total number of games in the database."""
        with self.get_connection() as conn:
            with conn.cursor() as cursor:
                cursor.execute("SELECT COUNT(*) FROM nfl_schedules")
                return cursor.fetchone()[0]
