#!/usr/bin/env python3
"""
Fetch NFL schedules from ESPN API and extract game information.
"""

import requests
from typing import List, Dict, Any


class Game:
    """Represents an NFL game with key details."""
    
    def __init__(self, date: str, week_no: int, title: str, venue: str):
        self.date = date
        self.week_no = week_no
        self.title = title
        self.venue = venue
    
    def __repr__(self):
        return f"Game(date={self.date}, week_no={self.week_no}, title={self.title}, venue={self.venue})"


def fetch_schedule(year: int, week: int) -> Dict[str, Any]:
    """
    Fetch schedule data from ESPN API for a given year and week.
    
    Args:
        year: The NFL season year
        week: The week number (1-17)
    
    Returns:
        JSON response as a dictionary
    """
    url = f"https://cdn.espn.com/core/nfl/schedule?xhr=1&year={year}&week={week}"
    response = requests.get(url)
    response.raise_for_status()
    return response.json()


def extract_games_from_response(data: Dict[str, Any]) -> List[Game]:
    """
    Extract Game objects from the API response.
    
    Args:
        data: JSON response from the API
    
    Returns:
        List of Game objects
    """
    games = []
    
    # Navigate to content.schedule
    schedule = data.get("content", {}).get("schedule", {})
    
    # Iterate over each date in the schedule
    for date_key, date_data in schedule.items():
        # Each date has a 'games' array
        for game_data in date_data.get("games", []):
            # Extract required fields
            date = game_data.get("date", "")
            week_no = game_data.get("week", {}).get("number", 0)
            title = game_data.get("name", "")
            
            # Navigate to competitions[0].venue.name
            venue = ""
            competitions = game_data.get("competitions", [])
            if competitions:
                venue = competitions[0].get("venue", {}).get("fullName", "")
            
            # Create Game object
            game = Game(date=date, week_no=week_no, title=title, venue=venue)
            games.append(game)
    
    return games


def main():
    """Main function to fetch schedules and print game information."""
    all_games: List[Game] = []
    
    # Fetch for both 2024 and 2025
    for year in [2024, 2025]:
        print(f"Fetching schedules for year {year}...")
        
        # Fetch weeks 1-17
        for week in range(1, 18):
            print(f"  Fetching week {week}...", end=" ")
            try:
                data = fetch_schedule(year, week)
                games = extract_games_from_response(data)
                all_games.extend(games)
                print(f"✓ ({len(games)} games)")
            except Exception as e:
                print(f"✗ Error: {e}")
    
    print(f"\nTotal games collected: {len(all_games)}")
    print("\n" + "="*80)
    print("VALIDATION: Printing Date, Title, and Venue for all games")
    print("="*80 + "\n")
    
    # Print validation output
    for game in all_games:
        print(f"Date: {game.date}")
        print(f"Title: {game.title}")
        print(f"Venue: {game.venue}")
        print("-" * 80)


if __name__ == "__main__":
    main()
