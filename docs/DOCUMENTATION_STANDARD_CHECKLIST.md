<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_REST Documentation Standard Checklist

**Purpose:** Apply the shared [FSM_COS documentation standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md) to this repository. This checklist is a practical review aid, not a replacement for the canonical standard.

## 1. Branding and Markdown hygiene

- [ ] Every Markdown document has the animated Workshop moniker in its first eight lines.
- [ ] Every Markdown document ends with exactly one canonical Workshop footer marker.
- [ ] The footer marker is followed only by the centered canonical static image paragraph:
  `<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>`
- [ ] No profile avatar, alternate footer artwork, duplicate moniker, or extra content follows the canonical footer.
- [ ] The branding verifier runs in CI and checks every repository Markdown file, not only the files changed in a pull request.

## 2. README and reader journey

- [ ] The README identifies the package, its owner, and the problem it solves.
- [ ] A meaningful visual or example appears early and has accessible alternative text.
- [ ] Where relevant, the README follows the shared reader journey: **00 Identity**, **01 What**, **02 Why**, **03 How**, **04 See it in a minute**, and **05 Choose your next step**.
- [ ] Build, NuGet, coverage, and other badges resolve to real targets; broken badges are fixed or explicitly reported.
- [ ] The first runnable example includes prerequisites, exact commands, expected output, and what the result does and does not prove.
- [ ] Public documentation, theory, recipes, Gallery, API/type references, UML, support, and next steps are easy to discover.
- [ ] The Gallery gives a short overview, links to detailed recipes, and explains how each exhibit can be reproduced.

## 3. Theory and teaching quality

- [ ] The theory index defines a learning path through modular, independently linkable chapters.
- [ ] Chapters begin with a recognizable problem, define terms, explain the reasoning and trade-offs, map concepts to implementation, and include worked examples.
- [ ] Chapters distinguish related concepts that readers may confuse.
- [ ] Exercises, discussion prompts, or questions invite readers to test their understanding.
- [ ] The chapter set teaches FSM_REST's domain and links to FSM_COS or other repositories for theory those repositories own.
- [ ] Instructors can reorder, replace, annotate, or extend a chapter without rewriting a monolithic document.

## 4. Evidence, provenance, and limitations

- [ ] Each technical claim is labeled or worded according to its evidence: implemented, source-checked, runtime-observed, or conceptual.
- [ ] Source, tests, samples, and workflows support claims about behavior.
- [ ] A diagram is identified as conceptual, source-derived, or captured from a named execution.
- [ ] Visuals include a caption or nearby explanation and an accessible text equivalent; animation is not presented as runtime proof.
- [ ] Limitations, unresolved assumptions, failure paths, ownership, and non-goals are explicit.
- [ ] Transport completion, HTTP status, domain success, and reconciliation remain distinct concepts.
- [ ] No benchmark, guarantee, dependency, feature, or test result is invented or overstated.

## 5. API, type, and UML documentation

- [ ] Public types have useful companion explanations linked to source and tests.
- [ ] Type pages explain purpose, responsibility, non-goals, public contract, important relationships, examples, status, and limitations.
- [ ] The UML index inventories applicable view types and marks gaps as required, not applicable with reason, manual, source-derived, runtime-captured, or planned.
- [ ] Target-centered diagrams place verified consumers above, the focal type at the center, dependencies below or around it, and package/project references to the right where useful.
- [ ] Sequence, state, activity, architecture, data, and use-case views answer distinct questions rather than forcing every fact into a class diagram.
- [ ] Relationships are source-supported or explicitly identified as authored concepts; partial scan scope is disclosed.
- [ ] Diagrams have regeneration and stale-output checks when generated. Manual diagrams are never described as automatically generated.

## 6. Examples, links, and maintenance

- [ ] Relative links and image paths resolve from the document that contains them.
- [ ] Commands, project paths, API names, package versions, and expected output match the target branch.
- [ ] Examples are runnable or clearly labeled as conceptual.
- [ ] One authoritative home exists for each detailed contract; other documents link to it rather than copying stale versions.
- [ ] Public guides teach users; internal audit ledgers and release evidence remain clearly identified and are not accidentally shipped as package documentation.
- [ ] CI validates documentation contracts and branding alongside code tests.
- [ ] NuGet publication, deployment, merges, and branch cleanup remain separately authorized operations; documentation work does not imply permission to perform them.

## Review outcome

- [ ] Checklist completed for the scope of this change.
- [ ] Remaining gaps are recorded with a concrete next action.
- [ ] The exact commit and CI run used for verification are recorded when reporting readiness.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
