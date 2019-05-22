# SignallingLoopPerformance

This project is a proof of concept for waiting on signals from any class in a kind of message loop + limiting
fps without using too much cpu load.

OpenTK for example uses a Render loop. This takes if not limited to a specific FPS a lot of CPU (usually 100%) of
one CPU core. The synchronization mechanism used there does not work really well to reduce CPU load.

For most UI applications a rendering is only necessary if the state of an object changed. This is limited to specific
properties of that object. So we try to keep the CPU load low but still allow handling of input etc. a rerender
only on change of UI affecting properties (that's the goal at least).

This properties depend on the theme. Some themes might only render on state for a button f.e. others might
change the look on mouse over, on mouse down etc.

This project tries to evaluate signalling this kind of loop allowing the CPU to not do anything as long
as no signal is send.

