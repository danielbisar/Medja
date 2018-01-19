
# Rendering / Layouting

MainLoop -> Update values
         -> Render updates

Layouting only needs to be done on changes of the UI (resize of the window, movement of objects, ...)
Update values can happen always (mouseover, click, etc)
Rendering only needs to be done when objects change (with usage of OpenGl, because keeps the last state)

How do we know if an object needs to be rendered again?
-> a property changed that influences rendering
--> TODO measure how big is the difference to rerender everything vs keep track of change values


# Performance Guidelines
- Interfaces are 10x slower than direct class usages. Use interfaces only were really necessary.
  Unclear is yet how this is with basic inheritance vs interfaces
  TODO This of course is something we should have a deeper look at for the rendering API.
  TODO measure composition vs inheritance (accessing fields and call their submethods vs access virtual methods)
- 