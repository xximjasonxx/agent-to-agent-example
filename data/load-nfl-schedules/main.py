#!/usr/bin/env python3
"""
Main script to fetch NFL schedules from ESPN API and load into PostgreSQL.
"""

from dotenv import load_dotenv

from src.espn_client import ESPNScheduleClient
from src.database import NFLScheduleDatabase

# Load environment variables from .env file
load_dotenv()


def main():
    """Main function to fetch schedules and load into database."""
    
    # Initialize clients
    espn_client = ESPNScheduleClient()
    db = NFLScheduleDatabase()
    
    # Ensure table exists
    print("Setting up database...")
    db.create_table_if_not_exists()
    
    # Fetch games from ESPN API
    print("\nFetching NFL schedules from ESPN API...")
    games = espn_client.fetch_games_for_seasons(
        years=[2024, 2025],
        weeks=range(1, 18)
    )
    
    print(f"\nTotal games fetched: {len(games)}")
    
    # Insert into database
    print("\nInserting games into database...")
    inserted, updated = db.upsert_games(games)
    
    print(f"✓ Inserted: {inserted} new games")
    print(f"✓ Updated: {updated} existing games")
    print(f"✓ Total games in database: {db.get_game_count()}")
    
    # Validation: print sample data
    print("\n" + "="*80)
    print("VALIDATION: Sample games (first 5)")
    print("="*80 + "\n")
    
    for game in games[:5]:
        print(f"ID: {game.generate_id()}")
        print(f"Season: {game.season}")
        print(f"Week: {game.week_no}")
        print(f"Date: {game.date}")
        print(f"Title: {game.title}")
        print(f"Venue: {game.venue}")
        print("-" * 80)


if __name__ == "__main__":
    main()
