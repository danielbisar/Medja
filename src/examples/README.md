# Examples readiness

These are .NET Framework 4.7.2 desktop applications, not automated tests.
The historical examples are kept in place; this index distinguishes compilation
from runtime validation.

## Status and verification

Verification used .NET SDK **10.0.112** on Linux. All four projects built through:

```sh
dotnet build src/examples/examples.sln --nologo -v:q
```

The final incremental build completed with **0 errors**. Clean builds can report
existing library documentation/compatibility warnings; the incremental result
does not establish a warning-free clean build.
The reference-assemblies packages allow compilation without a locally installed
.NET Framework targeting pack; they do **not** provide a runtime.

**No runtime was verified.** No GUI was launched: this environment has no Mono
runtime, and graphics/native-library compatibility has not been established.
Building does not validate OpenGL context creation, fonts, input, image loading,
or compatibility between the managed SkiaSharp packages and native libraries.

| Project | Purpose | Category / readiness | Verification | Kind |
| --- | --- | --- | --- | --- |
| [`demo`](demo/) | Main control showcase, layouts, tabs, transformations and embedded OpenGL content. | **Supported working (build only)**: primary example for current APIs. | Built standalone and in `examples.sln`; not run. | Manual GUI example. |
| [`graph2d`](graph2d/) | Minimal plotted sine wave: 257 synthetic samples over 0 to 4 pi, with labeled range -1 to 1. | **Supported working (example build only)**; the library's general-purpose Graph2D plotting support remains **experimental**. | Built in `examples.sln`; not run. | Manual GUI example, not a graph correctness or performance test. |
| [`editor`](editor/) | Text editor control with a small bottom button bar; optionally reads `dummy.txt` beside the executable. | **Historical useful**: stale namespace imports repaired; no editor redesign. | Built in `examples.sln`; not run. | Manual GUI example. |
| [`ui_tests`](ui_tests/) | Hand-selected visual control, interaction and performance experiments. Defaults to `ImageButtonTest`. | **Historical useful**: namespace imports repaired and direct SkiaSharp version aligned with the theme dependency. | Built in `examples.sln`; individual scenarios not run. | Manual visual checks / performance prototypes, **not** an automated test suite. |

“Supported working” here means the bounded example is maintained against current
source APIs and compiles, **not** that its desktop runtime is certified.
“Historical useful” means retained reference material with no completeness claim.
“Stale nonbuilding” means a known build/source blocker; no example currently has
that classification after the bounded fixes. “Experimental” means an incomplete
or exploratory feature, regardless of whether its source compiles. The separate
[prototype index](../prototypes/README.md) records all nine historical groups.

## Graph2D scope

`graph2d/Program.cs` now uses current namespaces and assigns the application's
`MainWindow`. `SyntheticPlotRenderer` is deliberately example-local: it uses the
current ordered `DataPoints` API and draws a small static series through Skia.
The current library `Graph2D` is an axis/container skeleton and its old series
renderer is commented out. This example replaces that container's content with
the local plot; it does **not** restore the unsupported production renderer,
downsampling UI, pan/zoom, live acquisition, or million-point benchmarks.
`Graph2DTest.cs` is preserved as commented historical experiment code, not active
coverage.

## Running manually on a compatible desktop

Build the desired project and launch its `bin/Debug/net472/*.exe` with a suitable
.NET Framework/Mono desktop runtime, OpenGL, fonts and matching native Skia
dependencies. SDK `dotnet run` alone is not a substitute for that runtime.
For `ui_tests`, run from its output directory so the copied relative
`Images/...` paths resolve. Select other visual scenarios in `ui_tests/Program.cs`
before rebuilding. Do not interpret successful compilation as successful visual
or performance testing.
