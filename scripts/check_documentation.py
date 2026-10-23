#!/usr/bin/env python3
"""Validate the minimum FSM_REST documentation contract without third-party packages."""
from pathlib import Path
import re
import sys

ROOT = Path(__file__).resolve().parents[1]
errors = []

# Every hand-maintained production source file needs a neighboring explanation.
for source in sorted((ROOT / "src").rglob("*.cs")):
    if any(part in {"bin", "obj"} for part in source.parts):
        continue
    companion = source.with_suffix(".md")
    if not companion.is_file():
        errors.append(f"Missing source companion: {companion.relative_to(ROOT)}")

uml_dir = ROOT / "docs" / "UML"
if not (uml_dir / "README.md").is_file():
    errors.append("Missing docs/UML/README.md index")

# Every target page (not the index) must state use cases and identify its source.
if uml_dir.is_dir():
    for page in sorted(uml_dir.glob("*.md")):
        if page.name == "README.md":
            continue
        content = page.read_text(encoding="utf-8")
        relative = page.relative_to(ROOT)
        if not re.search(r"^##?\s+Use cases\s*$", content, re.IGNORECASE | re.MULTILINE):
            errors.append(f"UML page has no Use cases section: {relative}")
        if not re.search(r"\*\*Source:\*\*|^- Source:", content, re.IGNORECASE | re.MULTILINE):
            errors.append(f"UML page has no explicit source provenance link: {relative}")
        if not re.search(r"\*\*Provenance:\*\*|\*\*Status:\*\*", content, re.IGNORECASE):
            errors.append(f"UML page has no provenance/status declaration: {relative}")

if errors:
    print("Documentation contract failed:")
    for error in errors:
        print(f" - {error}")
    sys.exit(1)

source_count = sum(1 for p in (ROOT / "src").rglob("*.cs") if not any(part in {"bin", "obj"} for part in p.parts))
uml_count = sum(1 for p in uml_dir.glob("*.md") if p.name != "README.md")
print(f"Documentation contract passed: {source_count} source companions checked; {uml_count} UML target pages checked.")
