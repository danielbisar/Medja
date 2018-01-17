# Features
- conversion of types (bind str to int and vice-versa)
- high performance (the overhead - memory and performance should be low)
- easy to use
- ideal: no changes to existing classes needed, just bind them together
- readonly, writeonly, twoway

Questions:
- when is the property updated (on-the-fly, regular but after a specific interfal?, ...)
- how to recognize that a property changed (event, saving states, flag)
- binding of properties only or binding of property to methods?
- dynamic (runtime) or compile time method patching?
- lists/ienumerable


How to use:

var binding = BindingFactory.Create(target, nameof(target.Property), source, nameof(source.Property), Mode.TwoWay);
binding.Converter = ...


# TODOs

- if needed add cache for BoundProperty objects


simple property binding concepts

patching

patch the existing property getter/setter and implement some kind of notification

setter prefix (if value != current value)