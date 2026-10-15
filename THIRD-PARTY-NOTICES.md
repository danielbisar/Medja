# Third-party notices and provenance

The [MIT license](LICENSE.md) applies to Medja's original work, with the
historical SprintWORX and author notices preserved. It does not relicense
dependencies, embedded fonts, or third-party material. SprintWORX authorized
publication of the Medja source; that permission is not a blanket license
clearance for unrelated third-party work.

## Dependency inventory

Versions below describe the historical framework, not recommendations for new
applications. PackageReference and packages.config files are the source of truth
for each project. Test-only dependencies are not part of the framework runtime.

| Component | Version/scope | License and evidence |
| --- | --- | --- |
| OpenTK | 3.1.0 in the main backend; older prototypes use 3.0.1 | MIT plus bundled Mono/Tao MIT notices and OpenEXR BSD-style terms. [Version 3.1 notices](licenses/OpenTK-3.1.txt). |
| SkiaSharp | 1.68.2.1 in the main backend; older prototypes use 1.68.0 | MIT for the managed binding. [SkiaSharp license](licenses/SkiaSharp-1.68.txt); native components have additional terms below. |
| Native SkiaSharp/Skia | Committed Linux x86-64 binary from the 1.68.0 release | [Complete upstream third-party notice inventory](licenses/SkiaSharp-External-Dependency-Info.txt), including Skia, font/image codecs and other native components. Not all components are MIT. |
| Google.Protobuf | 3.9.1 | BSD-3-Clause; [upstream license](https://github.com/protocolbuffers/protobuf/blob/v3.9.1/LICENSE). The generated test model is identified by its generated-code header. |
| ZeroMQ / clrzmq4 | 4.1.0.31 | LGPLv3; [license](licenses/ZeroMQ-LGPL-3.0.txt) and [GPLv3](licenses/GPL-3.0.txt). The [upstream package manifest](https://github.com/zeromq/clrzmq4/blob/master/ZeroMQ.nuspec) identifies this version and license. Native libzmq has its own applicable notices. |
| xUnit and its runners | Test projects; see package versions | Apache-2.0 for xUnit; retain the notices supplied with the restored packages. |
| Microsoft.NET.Test.Sdk and framework reference assemblies | Build/test tooling | MIT; see the restored packages' license metadata. Reference assemblies are not a .NET Framework runtime. |
| Historical GitHub gitignore template | Earlier attribution in this repository | [CC0-1.0](https://github.com/github/gitignore/blob/a1c526681bcb84c894288a827580568184f0e42e/LICENSE). The current file also derives from the SprintWORX template and retains its copyright and author. |

Restoring a package is different from redistributing its binaries. When shipping
packages or applications, include the applicable dependency notices and satisfy
their distribution conditions. In particular, LGPL-linked binary distributions
need a separate compliance review; Medja's MIT license does not eliminate those
obligations or imply that all dependencies are MIT.

## Committed native SkiaSharp binaries

All five tracked `libSkiaSharp.so` copies are byte-identical:

- `src/Medja.OpenTk/libSkiaSharp.so`
- `src/Medja.OpenTk.Test/libSkiaSharp.so`
- `src/prototypes/MultiOpenGLContext/MultiOpenGLContext/libSkiaSharp.so`
- `src/prototypes/SkiaSharp_CanvasPartialClear/SkiaSharp_CanvasPartialClear/libSkiaSharp.so`
- `src/prototypes/SkiaSharp_RenderingPerformance/SkiaSharp_RenderingPerformance/libSkiaSharp.so`

Each is an 8,306,856-byte ELF shared library for Linux x86-64, with SHA-256:

```text
d2b1126b44f1f30247b375f7368dfb4d965ecd947adbfc4701e0d139e856be18
```

During the October 2026 review, this hash was compared with a fresh download of
the [official SkiaSharp v1.68.0 release asset](https://github.com/mono/SkiaSharp/releases/download/v1.68.0/libSkiaSharp.so)
and matched exactly. This establishes artifact identity, not a reproducible
native build or compatibility with every managed package version.

The main managed dependency is 1.68.2.1, whereas these binaries are from 1.68.0.
The graphics runtime pairing has not been revalidated. They remain in place to
avoid silently removing the historical Linux deployment path. Prefer a matching,
verified package/release-based native restoration process before deploying new
applications. These binaries do not establish ARM, ARM64, MIPS, macOS or Windows
runtime support.

The copied native notice inventory is from
[SkiaSharp v1.68.0](https://github.com/mono/SkiaSharp/blob/v1.68.0/External-Dependency-Info.txt);
v1.68.2.1 supplies the same inventory. It is retained in full rather than assuming
which optional native components were enabled. Confirm exact build-component
and source-offer requirements before redistributing a new binary product.

## Assets, embedded fonts and historical examples

Daniel Bişar created the flyer and screenshots, as confirmed by the maintainer.
See [asset context](docs/assets.md) for historical branding, contact information,
platform claims and the distinction between demo imagery and application data.

`docs/flyer.pdf` embeds subsets of Liberation Sans Regular and Bold **1.07.4**.
The embedded font metadata identifies Copyright (c) 2007 Red Hat, Inc. and the
Liberation Fonts license: GPLv2 with a document-embedding exception, **not** the
OFL used by newer Liberation releases. See
[the font license and exception](licenses/Liberation-Fonts-1.x.txt) and
[GPLv2](licenses/GPL-2.0.txt). The exception prevents font embedding alone from
making the document GPL-covered; it does not relicense the font as MIT.

Historical `SensorFrontend/IPC/Client.cs`, no longer in the current tree,
closely follows the ZeroMQ Guide Hello World client example. It is generic
localhost demonstration code, not evidence that Medja authored that example.
The historical README links dependency licenses, but does not establish the
exact copied example revision/license. Confirm that attribution before
republishing the historical example separately.

## Manual confirmation still needed

- Identify the exact upstream revision/license of the historical copied ZeroMQ
  example and retain any required author attribution.
- Confirm source-availability and other applicable conditions for a future
  redistributed native stack, LGPL-linked application, or embedded font package.
- Confirm provenance of the small standalone button fixtures and
  `src/prototypes/OpenGL_FBO/OpenGL_FBO/wire.jpg` before separately distributing
  them as an asset pack. A "Created with GIMP" comment is not ownership evidence.
- Confirm whether application screenshots contain synthetic or actual recorded
  data before promoting them as anything beyond historical illustrations.

These are specific third-party/distribution questions, not uncertainty about
SprintWORX's permission to publish Medja or the maintainer's authorship of the
flyer and screenshots.
