# HowTo: Add a new control to Medja

Medja separates the rendering layer from the logic layer. This allows the usage of Themes and testing of controls without the need to render them. For this to work well you need keep a few things in mind. This guide shows you how to author a new controls.

_MyControl:_ Replace this by the real controls name.

## Checklist

For details see further down this document.

```
[ ] add new class: Medja/Controls/MyControl.cs
[ ] inherit from Control or any other sub-class
[ ] add corresponding tests to: Medja.Test/Controls/MyControlTest.cs 
[ ] add the control to: Medja/Theming/ControlFactory.cs
[ ] use Property<T> for new properties
[ ] implement renderer (optional) in: Medja.OpenTK/Rendering/Renderer
```

## Naming

_The coding guidelines apply._

Choose the name wisely. Avoid names that include the word "Control" (i.e. a combo box should not be named ComboBoxControl). Use names that makes it easy to identify the control.

## Folder structure

### Medja/Controls

Controls that consist of mainly one .cs file and that do not belong to any of the following categories.

### Medja/Controls/Container

Controls that display other controls and act as a container. The difference in regards to Panels

- often display just one sub control (have Content property)
- add functionality around this control (f.e. ScrollableContainer allows scrolling even if the control displayed in it does not have any knowledge about scrolling behavior)
- the main purpose is not layouting
- examples: Tabs, Scrollable, ...

### Medja/Controls/Panels

This controls are responsible for positioning (layouting) one or more sub controls. The key differences in regards to Containers

- often display more than one sub control
- main purpose: positioning of the sub controls
- examples: DockPanel, VerticalStackPanel, TablePanel
- usually do not have a special render, just a renderer that renders the background color

## ControlFactory

In order to support theming and different render targets the control must be registered within the class ControlFactory. See also the documentation for ControlFactory.

## Properties

In order to support change notification and data binding implement properties using the Property&lt;T&gt; class. Guideline see class documentation.