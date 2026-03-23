import json
from pathlib import Path


def _resource_path(relative: str) -> Path:
    """Return correct path whether running from source or PyInstaller bundle."""
    import sys
    base = getattr(sys, "_MEIPASS", Path(__file__).parent.parent)
    return Path(base) / relative


class L:
    _strings: dict = {}

    @classmethod
    def set_language(cls, lang: str):
        path = _resource_path(f"resources/strings_{lang}.json")
        with open(path, encoding="utf-8") as f:
            cls._strings = json.load(f)

    @classmethod
    def get(cls, key: str) -> str:
        return cls._strings.get(key, key)

    @classmethod
    def fmt(cls, key: str, *args) -> str:
        return cls.get(key).format(*args)
