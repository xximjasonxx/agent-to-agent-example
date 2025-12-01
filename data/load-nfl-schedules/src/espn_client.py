"""Client for fetching NFL schedules from ESPN API."""

import requests
from typing import List, Dict, Any

from .models import Game


class ESPNScheduleClient:
    """Client for interacting with ESPN's NFL schedule API."""
    
    BASE_URL = "https://cdn.espn.com/core/nfl/schedule"
    
    def fetch_schedule(self, year: int, week: int) -> Dict[str, Any]:
        """
        Fetch schedule data from ESPN API for a given year and week.
        
        Args:
            year: The NFL season year
            week: The week number (1-17)
        
        Returns:
            JSON response as a dictionary
        """
        url = f"{self.BASE_URL}?xhr=1&year={year}&week={week}"
        response = requests.get(url)
        response.raise_for_status()
        return response.json()
    
    def extract_games_from_response(self, data: Dict[str, Any]) -> List[Game]:
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
                season = game_data.get("season", {}).get("year", 0)
                
                # Navigate to competitions[0].venue.fullName
                venue = ""
                competitions = game_data.get("competitions", [])
                if competitions:
                    venue = competitions[0].get("venue", {}).get("fullName", "")
                
                # Create Game object
                game = Game(
                    date=date,
                    week_no=week_no,
                    title=title,
                    venue=venue,
                    season=season
                )
                games.append(game)
        
        return games
    
    def fetch_games_for_seasons(self, years: List[int], weeks: range) -> List[Game]:
        """
        Fetch games for multiple seasons and weeks.
        
        Args:
            years: List of years to fetch
            weeks: Range of weeks to fetch (e.g., range(1, 18))
        
        Returns:
            List of all Game objects
        """
        all_games = []
        
        for year in years:
            print(f"Fetching schedules for year {year}...")
            
            for week in weeks:
                print(f"  Fetching week {week}...", end=" ")
                try:
                    data = self.fetch_schedule(year, week)
                    games = self.extract_games_from_response(data)
                    all_games.extend(games)
                    print(f"✓ ({len(games)} games)")
                except Exception as e:
                    print(f"✗ Error: {e}")
        
        return all_games
