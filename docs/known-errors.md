# Known errors

## Error: System.EntryPointNotFoundException: gr_context_make_gl

Or similar with a stack trace referencing some skia sharp methods.

Reason: the libSkiaSharp.so or libHarfBuzzSharp.so is not valid for the current architecture or wasn't copied in the build output folder. Normally this should be done by msbuild but sometimes it failes. You can do it manually by (after nuget restore was successful) change in the directory csharp/packages/Medja.OpenTK.VERSION/content and copy the two files inside the folder of the SensorFrontend.exe (either the Debug or build/out folder).