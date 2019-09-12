# Testing

When working with Medja TDD is preferred. Try to write good tests.

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

