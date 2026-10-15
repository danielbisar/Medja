# Historical prototype index

These nine groups preserve focused experiments, not supported applications or an
automated test suite. Their source, attribution, project formats and binary
assets have intentionally not been rewritten. For maintained buildable examples,
start with the [example index](../examples/README.md).

## Classification

- **Supported working**: maintained current entry points; none of these historical
  prototypes is promoted to that category.
- **Historical useful**: useful reference technique, without a supported-runtime
  or production-readiness promise.
- **Stale nonbuilding**: known source/API build blockers, deliberately preserved.
- **Experimental**: exploratory graphics/performance behavior, even if it builds.

The category and build result are separate: an environment-blocked build is not
proof that all source is broken, and a successful build is not proof of correct
runtime behavior.

## Inspection and build results

Every group's project and entry point were inspected. Build attempts used
**.NET SDK 10.0.112 on Linux** with default Debug configuration:

```sh
dotnet build src/prototypes/<group>/<project>/<project>.csproj --nologo -v:q
```

For every row below, `<group>` and `<project>` both equal the listed directory
name. These are individual project builds, not a claim that a global prototype
solution succeeds. No prototype project files or implementation were repaired.

| Group | Purpose | Category | Build verification / observed limitation | Kind |
| --- | --- | --- | --- | --- |
| [`Combine2DAnd3D`](Combine2DAnd3D/) | Overlay a Medja panel on an OpenGL scene using a layer panel. | **Stale nonbuilding** | Attempted; failed with 3 compiler errors: missing current imports for `CenterOnScreen`, `VerticalStackPanel` and `LayerPanel`. | Manual graphics prototype. |
| [`ManualProjectTo2D`](ManualProjectTo2D/) | Project 3D positions into a 2D canvas with an orthographic camera and input handling. | **Stale nonbuilding** | Attempted; failed with 2 compiler errors for old `DarkBlueTheme` and missing `Canvas` type import. Other old theme names remain in inspected source; these first errors are not an exhaustive repair list. | Manual graphics prototype. |
| [`Menu`](Menu/) | Hierarchical commands/menu model and textual serialization; includes its own `PrintMenuFormat.md`. | **Historical useful** | Attempted; blocked by **MSB3644** (legacy .NET Framework 4.7.2 reference assemblies unavailable to this non-SDK project). Also uses assembly `HintPath` references to previously built Medja outputs. | Console/model prototype; no automated assertions. |
| [`MultiOpenGLContext`](MultiOpenGLContext/) | Switch OpenGL contexts and combine GL objects with a Skia surface. | **Experimental** | Attempted; blocked by **MSB3644** before source compilation. Legacy `packages.config` / `HintPath` dependencies on OpenTK 3.1.0 and SkiaSharp 1.68.0 also require their original restore layout. | Manual graphics/context prototype. |
| [`OpenGL_FBO`](OpenGL_FBO/) | Render through a framebuffer object and use textured geometry, including `wire.jpg`. | **Historical useful** | Attempted; blocked by **MSB3644** before source compilation. Legacy OpenTK 3.1.0 `packages.config` / `HintPath` setup; loads `wire.jpg` relative to the working directory. | Manual graphics prototype. |
| [`ReadFileContinously`](ReadFileContinously/) | Follow a growing file and experiment with file-creation notifications. | **Stale nonbuilding** | Attempted; blocked first by **MSB3644**. Inspection additionally finds an unfinished `watcher.Created +=` statement and a reference to an undefined `OnLogFileCreated` callback. | Console/I/O prototype, not an automated file-watcher test. |
| [`SignallingLoopPerformance`](SignallingLoopPerformance/) | Explore signal-driven rendering and FPS limiting without constant rendering; includes a focused historical README. | **Historical useful** | **Built**, 0 errors / 0 warnings; not run. Entry point continually signals in `while(true)` and prints FPS: an unbounded workload, not a benchmark harness. | Console performance prototype. |
| [`SkiaSharp_CanvasPartialClear`](SkiaSharp_CanvasPartialClear/) | Explore clearing/redrawing portions of a Skia GL-backed canvas after mouse input. | **Experimental** | **Built**, 0 errors / 0 warnings; not run. Pins historical OpenTK 3.0.1 and SkiaSharp/native-assets 1.68.0. | Manual graphics prototype. |
| [`SkiaSharp_RenderingPerformance`](SkiaSharp_RenderingPerformance/) | Compare drawing strategies (`Draw1`/`Draw2`/`Draw3`) and report frame rate; entry point selects `Draw2`. | **Experimental** | **Built**, 0 errors / 0 warnings; not run. Pins historical OpenTK 3.0.1 and SkiaSharp/native-assets 1.68.0. | Manual graphics/performance prototype; not a reproducible automated benchmark. |

**No runtime was verified**, including the console prototypes. This environment
has no Mono runtime; no GUI/OpenGL session or native compatibility was validated.
Bundled `.so` and image assets are preserved, not evidence of present-day runtime
compatibility. The SDK-style projects can obtain framework reference assemblies
through SDK restore behavior; that does not fix the old non-SDK project setup or
install a runtime.

## Boundaries before manual use

- Build only the group you intend to study. Do not treat these projects as
  supported startup instructions or indiscriminately launch the whole tree.
- The file-following entry point deletes and recreates a relative `test.log`;
  inspect it before any use in a working directory with valuable files.
- The signalling experiment is unbounded. Its printed FPS is observational,
  not a test pass/fail result or a current performance claim.
- Graphics experiments need compatible OpenGL, native libraries and a desktop
  runtime. Legacy package restoration and native dependency review are separate
  work; do not replace bundled binaries merely to make a build appear current.
- Known stale APIs and incomplete experiments are documented here rather than
  mass-repaired. A future revival should choose one experiment, preserve its
  intent and verify its runtime separately.
