# Testing

The supported CPU-only suites run with `bash build/test_headless.sh` from the
repository root. See [build requirements and verification scope](build.md).
They exercise control behavior, properties, IO and serialization without
initializing the native graphics backend. ZeroMQ prefix tests do not open sockets.

`src/examples/ui_tests` is a manual visual harness, not the automated xUnit
suite. OpenTK/Skia runtime and legacy `net472` tests are outside headless CI.

The historical project preferred test-driven development. The input simulation
below illustrates the separation between control behavior and rendering.

## Input simulation

You can use medja controls directly in tests. The controls will just not be 
"rendered" but still layouted.

Example:
```
[Fact]
public void ItemClickSelectsItem()
{
    // create the control via an empty or your ControlFactory
    var controlFactory = new ControlFactory();
    var comboBox = controlFactory.Create<ComboBox>();

    comboBox.Add("123");
    var item2 = comboBox.Add("456");

    Assert.NotEqual(item2, comboBox.SelectedItem);
    
    // simply send a click
    item2.InputState.SendClick();
    
    Assert.Equal(item2, comboBox.SelectedItem);
}
```

Every control contains the InputState object.

Examples:
```
inputState.SendClick(); 
inputState.SendClick(new Point(x, y));
inputState.SendKeyPress(new KeyboardEventArgs('c'));
inputState.SendKeyPress(new KeyboardEventArgs('c', ModifierKeys.LeftCtrl));
```
