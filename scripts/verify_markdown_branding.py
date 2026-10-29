#!/usr/bin/env python3
"""Enforce the Workshop moniker header and static avatar-only footer on every Markdown file."""

from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
MONIKER_URLS = (
    "https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg",
    "https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg",
)
AVATAR = "https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png"
FOOTER_MARKER = "<!-- THE SINGULARITY WORKSHOP FOOTER -->"
EXCLUDED_PARTS = {".git", "bin", "obj", "_dependencies", "TestResults", "artifacts"}

markdown_files = sorted(
    path for path in ROOT.rglob("*.md")
    if not any(part in EXCLUDED_PARTS for part in path.relative_to(ROOT).parts)
)
errors = []
for path in markdown_files:
    relative = path.relative_to(ROOT).as_posix()
    text = path.read_text(encoding="utf-8")
    lines = text.splitlines()
    first_lines = "\n".join(lines[:8])
    if not any(moniker in first_lines for moniker in MONIKER_URLS):
        errors.append(f"{relative}: Workshop moniker SVG is missing from the document header.")
    if text.count(FOOTER_MARKER) != 1:
        errors.append(f"{relative}: expected exactly one Workshop footer marker.")
    footer = text.split(FOOTER_MARKER, 1)[-1].strip()
    expected_footer = (
        f'<p align="center"><img src="{AVATAR}" '
        'alt="The Singularity Workshop avatar" width="200" /></p>'
    )
    if footer != expected_footer:
        errors.append(
            f"{relative}: footer must contain only the canonical static avatar paragraph."
        )

if errors:
    print("Markdown branding validation failed:")
    for error in errors:
        print(f" - {error}")
    sys.exit(1)

print(f"Markdown branding validated: {len(markdown_files)} files have the moniker header and static avatar-only footer.")
