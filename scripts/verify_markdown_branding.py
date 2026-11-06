#!/usr/bin/env python3
"""Enforce canonical Workshop branding on the Markdown files in the supplied change scope.

With no paths, validate every Markdown file (useful for an explicit full-inventory audit).
"""

from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
MONIKER_URL = "https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg"
FOOTER_IMAGE_URL = "https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png"
FOOTER_MARKER = "<!-- THE SINGULARITY WORKSHOP FOOTER -->"
EXCLUDED_PARTS = {".git", "bin", "obj", "_dependencies", "TestResults", "artifacts"}


def markdown_files():
    return sorted(
        path for path in ROOT.rglob("*.md")
        if not any(part in EXCLUDED_PARTS for part in path.relative_to(ROOT).parts)
    )


if len(sys.argv) > 1:
    candidates = []
    for argument in sys.argv[1:]:
        path = (ROOT / argument).resolve()
        if ROOT not in path.parents or path.suffix.lower() != ".md":
            raise SystemExit(f"Invalid Markdown path outside repository: {argument}")
        if path.is_file():
            candidates.append(path)
        else:
            raise SystemExit(f"Changed Markdown file does not exist: {argument}")
    files = sorted(set(candidates))
else:
    files = markdown_files()

errors = []
for path in files:
    relative = path.relative_to(ROOT).as_posix()
    content = path.read_text(encoding="utf-8")
    lines = content.splitlines()
    first_lines = "\n".join(lines[:8])
    if MONIKER_URL not in first_lines:
        errors.append(f"{relative}: canonical Workshop moniker SVG is missing from the document header.")
    if content.count(FOOTER_MARKER) != 1:
        errors.append(f"{relative}: expected exactly one Workshop footer marker.")
    footer = content.split(FOOTER_MARKER, 1)[-1].strip()
    expected_footer = (
        f'<p align="center"><img src="{FOOTER_IMAGE_URL}" '
        'alt="The Singularity Workshop avatar" width="200" /></p>'
    )
    if footer != expected_footer:
        errors.append(f"{relative}: footer must contain only the canonical static Workshop image paragraph.")

if errors:
    print("Markdown branding validation failed:")
    for error in errors:
        print(f" - {error}")
    sys.exit(1)

scope = "repository" if len(sys.argv) == 1 else "change"
print(f"Markdown branding validated: {len(files)} {scope}-scope files.")
