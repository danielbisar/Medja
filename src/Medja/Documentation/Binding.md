# Features
- conversion of types (bind str to int and vice-versa)
- high performance (the overhead - memory and performance should be low)
- easy to use
- ideal: no changes to existing classes needed, just bind them together
- readonly, writeonly, twoway

Questions:
- when is the property updated (on-the-fly, regular but after a specific interval?, ...)
- how to recognize that a property changed (event, saving states, flag)
- binding of properties only or binding of property to methods?
- dynamic (runtime) or compile time method patching?
- lists/ienumerable


How to use:

var binding = BindingFactory.Create(target, nameof(target.Property), source, nameof(source.Property), Mode.TwoWay);
binding.Converter = ...

