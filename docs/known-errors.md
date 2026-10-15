# Native-load troubleshooting

This is legacy graphics guidance, not part of the supported
[headless build/test path](build.md).

## EntryPointNotFoundException or DllNotFoundException in SkiaSharp

For example, a historical failure mentioned `gr_context_make_gl`. Check:

1. The managed SkiaSharp version and native library export/ABI compatibility.
2. OS and architecture: the tracked `libSkiaSharp.so` is Linux x86-64 only.
3. Whether the native library and its system dependencies can be found from the
   application's output directory.

The main backend references managed SkiaSharp 1.68.2.1, while the committed native
binary matches the upstream 1.68.0 release. Current runtime compatibility is
unverified. Do not copy arbitrary native libraries between package versions.
Use a matching verified build and retain its notices; see
[native provenance](../THIRD-PARTY-NOTICES.md).

The main demo's Release output is
`src/examples/demo/bin/Release/net472/`. Earlier package/application directory
names in old troubleshooting notes do not describe this repository's current
layout.
