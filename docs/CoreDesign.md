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
- input handling
    easy
	touch
	drag@drop

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











- startup performance vs runtime performance
  - project fd: startup performance does not have to be so high, if runtime performance is
  - lazy loading (just load what you need) can add significant 
complexity 

Ideal case:

- All needed data is loaded on startup.
- Rendering renders what is visible
- Input handling handles what is visible
- hidden items are not rendered/handled -> visibility property vs screen clipping
- menu structure could be loaded on the fly, but also just when needed (different ways of implementing: xml, code, ...)
- Rendering complexity
	keep the ui simple but still intuitive and nice
	test: f.e. button: bitmap vs flat rectangle with color changes
	text-rendering
		-> size of text, font loading, nice appearance
- input device handling optimization 
	for touch no mouse over is needed, just press, release, click, double click
	-> exception zoom (two finger action)
- rendering of graph
	- just process data that can be seen, it doesn't make sense to render 1.0 mil points if you will not be able to distinguish them on the screen at all

- multithreading: how much cores do we have? expected 4
- 1 for ui handling (can we use multiple for that?)
- 3 for other things like: backend (data reading), filtering (fft, howmuch to display, others?), data processing: triggers
- what about ipc (serialization performance, transfer performance)


https://stackoverflow.com/questions/38682886/netmq-vs-clrzmq
http://blog.scottlogic.com/2015/03/20/ZeroMQ-Quick-Intro.html