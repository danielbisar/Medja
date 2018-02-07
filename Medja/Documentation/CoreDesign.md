After some tests starting a UI Framework/Library we should have clear goals since it is a very complex project.

- easy to use
	- easy to integrate into a project
	- only dependencies really needed
	- all in one package
	- initialization should require no up to one line of code (for default scenarios)
	- 
- high performance
- modern ui needs
	- animations
	- stylable (company want's to be flexible)
- separation of concerns (designer: ui behavior, optic; programmer: business logic, data)
	- designers often use tools maybe HTML, CSS like things; not too much programming
- usable on embedded devices (clarify Raspberry PI or bigger?) -> Limited hardware CPU, Memory, GPU

# Easy to use

## File formats / Designer

A program to design the UI but also easy to apply changes (no designer required).
XML, JSON or something like that is often appealing for humans :D. - they like HTML

How should this files be structured. How can they relate to each other?

See QT (Controls, namespaces), XAML (ResourceDictionaries, Controls, namespaces), HTML (links, others?)



# Performance

## Memory

How much memory do we have at least?
Distinguish between simple and rich ui (an embedded device will maybe not need effects and animations, even though it is nice to have)

## CPU

Monitor layout/rendering CPU usage. Ideal: reduce the costs.

## GPU

Allow implementation of very light weight rendering.


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

# Language for designer
- easy to use for users without designer (xml, json?, others)
- allow perprocessor directives (debug only/release only/etc)
