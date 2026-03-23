from dataclasses import dataclass


@dataclass
class Filiere:
    filiere_id: int = 0
    filiere_nom: str = ""
    filiere_code: str = ""
    etab_id: int = 0
    nb_place: int = 0
