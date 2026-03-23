from dataclasses import dataclass


@dataclass
class Etablissement:
    etab_id: int = 0
    etab_nom: str = ""
    etab_code: str = ""
