# Controls

Controls should be created under the folder Controls and have the same namespace. Also they should inherit from the class Control or any subclass. Additionally you should add a method to the ControlFactory class and make that method protected virtual (so that any theme can overwrite it's renderer).

For each render implementation you need to overwrite this method and set a renderer. By time of writing this we only have Medja.OpenTk