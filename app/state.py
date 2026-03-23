class AppState:
    language: str = "fr"  # default until user selects at startup
    is_first_open: bool = False
    session_in_progress: bool = False
    max_choices: int = 5
