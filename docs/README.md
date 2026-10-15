# Medja documentation

Medja is a historical, employer-supported C# UI framework. Start with the
[project overview](../README.md) and [current build/verification scope](build.md).
Linked source paths are relative to each document.

## Current entry points

- [Build and verification](build.md)
- [Headless behavioral testing](testing.md)
- [Adding a control](howto-add-control.md)
- [Examples and manual visual harness](../src/examples/README.md)
- [Prototype status index](../src/prototypes/README.md)
- [Native-load troubleshooting](known-errors.md)
- [Third-party notices and provenance](../THIRD-PARTY-NOTICES.md)

## Architecture in source

| Entry point | Responsibility |
| --- | --- |
| [Control](../src/Medja/Controls/Control.cs) | State, layout and input integration |
| [ControlFactory](../src/Medja/Theming/ControlFactory.cs) | Control construction and renderer attachment |
| [Property](../src/Medja.Properties/Properties/Property.cs) | Typed values and change notification |
| [Binding](../src/Medja.Properties/Binding/Binding.cs) | Binding lifecycle |
| [OpenTK renderer](../src/Medja.OpenTk/Rendering/OpenTkRenderer.cs) | Native rendering integration |

## Historical engineering material

These documents preserve design exploration and project conventions. They are
not a current product roadmap or a guarantee that every sample API still exists.

| Document | Status |
| --- | --- |
| [Core design](CoreDesign.md) | Historical requirements and architecture questions |
| [Property experiments](Properties.md) | Historical measurements and rejected designs; not reproducible comparative benchmarks |
| [Binding notes](Binding.md) | Experimental design/API sketch |
| [Control notes](Controls.md) | Historical authoring notes |
| [Coding guidelines](CodingGuideline.md) | Historical team conventions, with unsupported performance rules removed |
| [Renderer guidelines](guidelines-renderer.md) | Historical checklist |
| [OpenTK initialization](OpenTK_Sequence.md) | Historical observation for OpenTK 3.1 |
| [OpenGL notes](opengl/README.md) | Historical graphics experiments |
| [0.9.1.0 release notes](releases/0.9.1.0) | Archived release record, not current platform guidance |
| [Assets and flyer context](assets.md) | Authored historical imagery and marketing material |
