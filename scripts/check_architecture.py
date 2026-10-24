#!/usr/bin/env python3
"""Enforce FSM_REST's core/adapter dependency direction."""
from pathlib import Path
import sys
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
errors = []

def project(path):
    return ET.parse(ROOT / path).getroot()

def refs(path):
    root = project(path)
    return [
        node.attrib.get("Include", "")
        for node in root.iter()
        if node.tag.rsplit("}", 1)[-1] in {"ProjectReference", "PackageReference"}
    ]

core = "src/FSM_REST/FSM_REST.csproj"
core_refs = refs(core)
for reference in core_refs:
    if any(token.lower() in reference.lower() for token in (
        "fsm_rest.cos", "fsm_rest.gui", "fsm_rest.openapi",
        "microbundledomain", "fsm_cos", "gui.core",
    )):
        errors.append(f"Core has a forbidden integration/provider dependency: {reference}")

openapi_refs = refs("src/FSM_REST.OpenApi/FSM_REST.OpenApi.csproj")
if not any("FSM_REST.csproj" in item for item in openapi_refs):
    errors.append("OpenAPI provider must reference the neutral core project.")
if any(any(token in item.lower() for token in ("fsm_rest.cos", "fsm_rest.gui", "microbundledomain", "fsm_cos")) for item in openapi_refs):
    errors.append("OpenAPI provider must not depend on COS or GUI integration packages.")

gui_refs = refs("src/FSM_REST.GUI/FSM_REST.GUI.csproj")
if not any("FSM_REST.csproj" in item for item in gui_refs):
    errors.append("GUI adapter must reference the neutral core project.")
if any("fsm_rest.openapi" in item.lower() for item in gui_refs):
    errors.append("GUI adapter must execute neutral descriptors and transports, not require OpenAPI.")

cos_refs = refs("src/FSM_REST.COS/FSM_REST.COS.csproj")
if not any("FSM_REST.csproj" in item for item in cos_refs):
    errors.append("COS adapter must reference the neutral core project.")

forbidden_core_symbols = (
    "OpenApiJsonImporter",
    "OpenApiRestApiConnection",
    "RestGuiOperationExecutor",
)
for source in sorted((ROOT / "src" / "FSM_REST").glob("*.cs")):
    content = source.read_text(encoding="utf-8")
    for symbol in forbidden_core_symbols:
        if symbol in content:
            errors.append(f"Core source {source.relative_to(ROOT)} references adapter/provider type {symbol}.")

if errors:
    print("Architecture boundary check failed:")
    for error in errors:
        print(f" - {error}")
    sys.exit(1)

print("Architecture boundary check passed: core remains neutral; OpenAPI, GUI, and COS integrations stay outside core.")
