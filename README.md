# Medja

A cross platform UI Framework purposed for Embedded Devices using SkiaSharp and OpenTK.

Mainly developed under Linux, partially tested under Windows and MacOS, but should work, since both SkiaSharp and OpenTK supports it.

- [Documentation](docs/README.md)
- [License](LICENSE.md)

# Features

- Cross-Platform (Windows, Linux, MacOS)
- UI-Tests without rendering any UI, but still test the behavior (e.g. including keystrokes etc; see https://github.com/danielbisar/Medja/blob/master/src/Medja.Test/Controls/TextEditor/TextEditorTest.cs for an example)
- DataBinding (~1000 times as fast as WPF on .NET 4.7)
- Theming
- Good set of default controls (Button, Slider, ProgressBar, TextBox, ...)
- Touch UI
- Integration of 3D OpenGL into 2D Views
- Extensability
- Layouting (via different layout containers, like StackPanel, DockPanel, ...)

Still missing
- UI without Code (f.e. via a markup language)
- Validation
- More controls

Goals:
.net standard or dotnet 6 support
Currently just mono/.net Framework 4.7.2 since the used OpenTK Version does not support anything else.

## Additional, non UI Features:

- easy to use ZeroMQ helpers
- Protobuf as Streaming Format
- Other useful extensions for dotnet

# Examples

https://github.com/danielbisar/Medja/tree/master/src/examples

Have a look at https://github.com/danielbisar/Medja/blob/master/src/examples/graph2d/Program.cs for a simple example on how to setup an application using Medja. (54 lines of code)

# Thanks

Big thanks to SprintWORX GmbH for supporting and letting me publish my work.

Contributors
- Matthias Fritsch (owner of SprintWORX GmbH)
- @lockejan some control development

# Used by:

https://github.com/lockejan/secure-text-editor

