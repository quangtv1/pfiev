from dataclasses import dataclass


@dataclass
class Note:
    note_id: int = 0
    candidat_id: int = 0
    matiere_id: int = 0
    note_value: str = "0"
