#!/usr/bin/env python3
"""Validate relative Markdown links and image paths in the UML documentation tree."""
from pathlib import Path
from urllib.parse import unquote, urlsplit
import re
import sys

ROOT = Path(__file__).resolve().parents[1]
UML = ROOT / "docs" / "UML"
errors = []
checked = 0
link_pattern = re.compile(r"!?\[[^\]]*\]\(([^)]+)\)")

for page in sorted(UML.rglob("*.md")):
    text = page.read_text(encoding="utf-8")
    for match in link_pattern.finditer(text):
        raw = match.group(1).strip()
        if not raw:
            continue
        if raw.startswith("<") and ">" in raw:
            raw = raw[1:].split(">", 1)[0]
        destination = raw.split(maxsplit=1)[0] if raw else ""
        parsed = urlsplit(destination)
        if not parsed.path or parsed.scheme or destination.startswith("//"):
            continue
        target = (page.parent / unquote(parsed.path)).resolve()
        checked += 1
        if not target.exists():
            errors.append(f"{page.relative_to(ROOT)}: broken relative link/image: {destination}")
        elif target.is_dir() and not any((target / name).is_file() for name in ("README.md", "index.md", "index.html")):
            errors.append(f"{page.relative_to(ROOT)}: directory link has no index: {destination}")

if errors:
    print("UML link validation failed:")
    for error in errors:
        print(f" - {error}")
    sys.exit(1)

print(f"UML link validation passed: {checked} relative Markdown links and image paths checked.")
