from dataclasses import dataclass
from typing import Optional


@dataclass
class Candidat:
    candidat_id: int = 0
    nom: str = ""
    nom_intermediaire: str = ""
    prenom: str = ""
    date_de_naissance: Optional[str] = None  # stored as TEXT "YYYY-MM-DD"
    sexe: str = ""
    candidat_statut: str = "I"               # "I" = interne, "E" = externe
    langue: str = "fr"
    etab_id: int = 0
    candidat_moyenne: Optional[float] = None
    candidat_classement: Optional[int] = None
    anonymat: str = ""
