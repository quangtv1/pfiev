from dataclasses import dataclass


@dataclass
class Choix:
    choix_id: int = 0
    candidat_id: int = 0
    filiere_id: int = 0
    choix_ordre: int = 1
    choix_admis: bool = False
    filiere_nb_place: int = 0
