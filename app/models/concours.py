from dataclasses import dataclass


@dataclass
class Concours:
    concours_id: int = 0
    annee: int = 0
    moyenne_min: float = 0.0
