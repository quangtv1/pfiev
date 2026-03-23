from dataclasses import dataclass


@dataclass
class Matiere:
    matiere_id: int = 0
    matiere_nom: str = ""
    matiere_coefficient: float = 1.0
    matiere_code: str = ""
