#!/usr/bin/env python3
"""Verify source contract anchors and generate the deterministic core-surface SVG.

This is intentionally a small, authored diagram with source-anchor checks, not a general C# parser.
The checks verify named symbols and members exist; they do not derive or prove every diagram edge.
"""
from __future__ import annotations

import argparse
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
OUTPUT = ROOT / "docs/assets/rest-contract-surface.svg"
CONTRACTS = {
    "src/FSM_REST/RestApiDescriptor.cs": [
        "public sealed record RestApiDescriptor(",
        "IReadOnlyList<RestOperationDescriptor> Operations",
        "public int OperationCount => Operations.Count;",
    ],
    "src/FSM_REST/RestOperationDescriptor.cs": [
        "public sealed record RestOperationDescriptor(",
        "public string PaletteLabel =>",
        "public IReadOnlyList<RestResponseDescriptor> ResponseDescriptors =>",
    ],
    "src/FSM_REST/RestRequestFactory.cs": [
        "public static class RestRequestFactory",
        "public static RestRequest Create(",
        "return new RestRequest(",
    ],
    "src/FSM_REST/IRestTransport.cs": [
        "public interface IRestTransport",
        "Task<RestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default);",
    ],
    "src/FSM_REST/RestResponse.cs": [
        "public sealed record RestResponse(",
        "public bool IsSuccessStatusCode => StatusCode is >= 200 and < 300;",
        "public T? Deserialize<T>",
    ],
}
SVG = '''<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1000 560" role="img" aria-labelledby="title desc">
  <title id="title">FSM_REST core contract surface — source-anchor-checked</title>
  <desc id="desc">Authored relationships between API and operation descriptors, request construction, transport, and observed response; named source anchors are checked, but edges are not inferred from code. This diagram is not runtime telemetry.</desc>
  <defs>
    <style>
      text{font-family:system-ui,sans-serif;fill:#18212b}
      .node{fill:#f7fafc;stroke:#315b75;stroke-width:2}
      .boundary{fill:#eef6fa;stroke:#315b75;stroke-width:2}
      .title{font-size:18px;font-weight:700}
      .detail{font-size:13px}
      .edge{fill:none;stroke:#536b7a;stroke-width:2;marker-end:url(#arrow)}
      .label{font-size:12px;fill:#405665}
      .note{font-size:13px;fill:#4a5963}
    </style>
    <marker id="arrow" markerWidth="8" markerHeight="8" refX="6" refY="3" orient="auto"><path d="M0,0 L6,3 L0,6 Z" fill="#536b7a"/></marker>
  </defs>
  <rect width="1000" height="560" rx="18" fill="#fff"/>
  <text x="32" y="38" font-size="23" font-weight="700">FSM_REST core contract surface</text>
  <text x="32" y="61" class="note">Authored inventory · named contract anchors checked against repository source</text>
  <rect class="node" x="45" y="105" width="270" height="105" rx="12"/>
  <text class="title" x="65" y="135">RestApiDescriptor</text>
  <text class="detail" x="65" y="160">Title · Version · Operations</text>
  <text class="detail" x="65" y="181">OperationCount</text>
  <rect class="node" x="370" y="105" width="270" height="105" rx="12"/>
  <text class="title" x="390" y="135">RestOperationDescriptor</text>
  <text class="detail" x="390" y="160">Method · Path · parameters</text>
  <text class="detail" x="390" y="181">PaletteLabel · response descriptors</text>
  <path class="edge" d="M315 157 H365"/>
  <text class="label" x="319" y="145">contains</text>
  <rect class="boundary" x="370" y="270" width="270" height="105" rx="12"/>
  <text class="title" x="390" y="300">RestRequestFactory</text>
  <text class="detail" x="390" y="325">Create operation request</text>
  <text class="detail" x="390" y="346">Binds runtime values; does not send</text>
  <path class="edge" d="M505 210 V265"/>
  <text class="label" x="515" y="241">builds request from</text>
  <rect class="boundary" x="700" y="270" width="255" height="105" rx="12"/>
  <text class="title" x="720" y="300">IRestTransport</text>
  <text class="detail" x="720" y="325">SendAsync(request, token)</text>
  <text class="detail" x="720" y="346">Transport is replaceable</text>
  <path class="edge" d="M640 323 H695"/>
  <text class="label" x="646" y="311">request</text>
  <rect class="node" x="700" y="425" width="255" height="85" rx="12"/>
  <text class="title" x="720" y="455">RestResponse</text>
  <text class="detail" x="720" y="478">Observed status, headers, body</text>
  <text class="detail" x="720" y="496">JSON deserialize ≠ schema validation</text>
  <path class="edge" d="M827 375 V420"/>
  <text class="label" x="840" y="405">returns</text>
  <text x="45" y="445" class="note">Boundary discipline</text>
  <text x="45" y="468" class="detail">Descriptor = declared capability</text>
  <text x="45" y="490" class="detail">Factory = request construction</text>
  <text x="45" y="512" class="detail">Transport = observed communication</text>
  <text x="45" y="534" class="detail">Response = data to interpret and validate</text>
  <text x="500" y="548" text-anchor="middle" font-size="11" fill="#687782">Template checked by scripts/generate_rest_contract_diagram.py · not a runtime capture</text>
</svg>
'''


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--check", action="store_true", help="fail if the checked-in SVG is stale")
    args = parser.parse_args()

    for relative_path, required in CONTRACTS.items():
        path = ROOT / relative_path
        if not path.is_file():
            print(f"Missing contract source: {relative_path}", file=sys.stderr)
            return 1
        source = path.read_text(encoding="utf-8")
        missing = [anchor for anchor in required if anchor not in source]
        if missing:
            print(f"{relative_path}: documented contract anchor changed or missing: {missing}", file=sys.stderr)
            return 1

    if args.check:
        if not OUTPUT.is_file() or OUTPUT.read_text(encoding="utf-8") != SVG:
            print(f"{OUTPUT.relative_to(ROOT)} is stale; run python3 scripts/generate_rest_contract_diagram.py", file=sys.stderr)
            return 1
        print("PASS: source contract anchors and checked-in SVG are synchronized")
        return 0

    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    OUTPUT.write_text(SVG, encoding="utf-8", newline="\n")
    print(f"Generated {OUTPUT.relative_to(ROOT)}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
