# How to add a new control to Medja

- create test under Medja.Test/Controls
- create control class under Medja/Controls, inherit from Control
- add virtual factory method to Medja/Theming/ControlFactory and 
  register in dictionary
- override factory method at least for Medja.OpenTk - if it is a layout
  control you should use ControlRenderer class as renderer so that 
  background colors get rendered if set; otherwise create your custom
  renderer
  
  If you need to create sub-controls in your control use IControlFactory
  as parameter for the class constructor. In the ControlFactory class
  just pass this as parameter. This assures that you will be able to 
  easily test the control even without a renderer.
  
  In the test create an instance of ControlFactory and create your
  control via that.