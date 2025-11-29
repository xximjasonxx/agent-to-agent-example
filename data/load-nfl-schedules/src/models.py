"""Data models for NFL schedules."""

import uuid
from typing import NamedTuple


class Game(NamedTuple):
    """Represents an NFL game with key details."""
    
    date: str
    week_no: int
    title: str
    venue: str
    season: int
    
    def generate_id(self) -> uuid.UUID:
        """
        Generate a deterministic UUID based on season, week_no, and title.
        
        This ensures the same game always gets the same ID, preventing duplicates.
        """
        # Create a deterministic namespace-based UUID
        namespace = uuid.NAMESPACE_DNS
        unique_string = f"{self.season}-{self.week_no}-{self.title}"
        return uuid.uuid5(namespace, unique_string)
