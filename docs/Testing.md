# Testing

When working with Medja TDD is preferred. Try to write good tests.

## Input simulation

### Keyboard

If you want to test keyboard input you can just do the following (f.e. for Ctrl+C)

```
myControl.InputState.NotifyKeyPressed(new KeyboardEventArgs('c', ModifierKeys.Ctrl));
```

