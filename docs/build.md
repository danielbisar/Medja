# Build and verification

## Supported headless path

Requirements: Linux x86-64, .NET SDK **10.0.112** and its .NET 10 runtime, plus
network access to NuGet for the first restore. The checked-in `global.json`
allows later patches in the same SDK feature band. No Mono, native ZeroMQ,
OpenGL context or display server is required for these suites.

From the repository root:

```bash
dotnet --version
bash build/test_headless.sh
```

The script runs `dotnet test --configuration Release --framework net10.0` for
`Medja.Test`, `Medja.Utils.Test`, `Medja.Properties.Test`,
`Medja.ProtoBuf.Test` and `Medja.ZeroMQ.Test`. Restore is performed by
`dotnet test`; a failed restore, build or test stops the script.

For an individual project, the explicit equivalent is:

```bash
dotnet restore src/Medja.Test/Medja.Test.csproj
dotnet build src/Medja.Test/Medja.Test.csproj --configuration Release --no-restore
dotnet test src/Medja.Test/Medja.Test.csproj --configuration Release --framework net10.0 --no-build --no-restore
```

The October 2026 verification ran these CPU-only suites on Fedora 44 x86-64.
GitHub Actions uses Ubuntu 24.04 and the same SDK/script, plus compilation of the
legacy graphics libraries. CI is not a claim of Windows/macOS/device or graphics
runtime coverage.

The main `src/Medja.sln` and the four-project `src/examples/examples.sln` also
compiled in Release during the review. This does not mean every suite in the
main solution was executed.

The production core libraries still target `netstandard2.0`; only the CPU test
hosts use modern .NET. The original test target remains available with
`-p:LegacyTestTarget=true`, but executing `net472` tests requires a compatible
legacy runtime and is not the supported headless path.

NuGet may report compatibility warnings for historical dependencies restored
through .NET Framework assets, notably ZeroMQ and the protobuf project's old
test-platform dependency. Those warnings remain visible.
Passing pure-managed prefix tests does not verify native ZeroMQ transport.

## Graphics compilation and examples

The backend and themes target `net472`. Their existing framework-reference
packages allow compilation with the SDK on Linux without installing Mono:

```bash
dotnet restore src/Medja.OpenTk.Themes/Medja.OpenTk.Themes.csproj
dotnet build src/Medja.OpenTk.Themes/Medja.OpenTk.Themes.csproj --configuration Release --no-restore
dotnet restore src/examples/demo/demo.csproj
dotnet build src/examples/demo/demo.csproj --configuration Release --no-restore
dotnet restore src/examples/graph2d/graph2d.csproj
dotnet build src/examples/graph2d/graph2d.csproj --configuration Release --no-restore
```

Compilation does **not** execute SkiaSharp, OpenGL, window/input handling or the
legacy graphics tests. Existing XML-documentation and compatibility warnings are
not suppressed. See the [example index](../src/examples/README.md) for the
maintained compile scope and the [prototype index](../src/prototypes/README.md)
for intentionally unsupported projects.

## Legacy GUI execution: not currently revalidated

Historically the demo ran under Linux/Mono. After a successful Release build,
the executable is `src/examples/demo/bin/Release/net472/demo.exe`.
On a separately provisioned compatible Mono/graphics system, the historical
launch command is:

```bash
mono src/examples/demo/bin/Release/net472/demo.exe
```

It additionally needs a working display, OpenGL implementation, platform
libraries and an ABI-compatible native SkiaSharp build. The committed native
library is Linux x86-64 from SkiaSharp 1.68.0; the managed backend references
1.68.2.1. This pairing has not been runtime-verified during cleanup.

No currently supported Mono version, embedded board, Windows configuration or
macOS configuration is claimed. Do not install obsolete system repositories to
try the demo. See [native provenance](../THIRD-PARTY-NOTICES.md) and
[native-load troubleshooting](known-errors.md).

## Archived build tooling

`build/build.sh`, `build/run_tests.sh` and `build/linux/*.sh` preserve the
historical Mono/MSBuild workflow. The old installers use obsolete OS repositories
or unpinned source/tool downloads and are **not setup instructions for current
systems**. Use the headless SDK path above instead. The legacy build script
requests `LegacyTestTarget=true` to avoid mixing modern test-host binaries into
its Mono runner output.
