# Naming

- boolean variables should start with is, has, can

# Class layout

// always provide the access modifier (public, internal, private, ...)
// class names in CamelCase
public class MyClass
{
	// order of elements
	// static then instance
	// fields, properties and events over constructors
	// fields that belong to a property are directly over that property, no empty line
	// methods after constructor, order of methods in the order they are called by the class, the access modifier order is secondary (see below)
	// don't use method groups for properties
	// keep classes short (should not be bigger than ~ 400 lines, usually ~200), will keep the class readable especially with the given order of methods
    
	// use readonly whenever possible
	private static readonly _myStaticField;

	public static void SomeMethod()
	{	
	}

	// don't use properties for private fields, except you have additionaly handling which really makes sense (try not to do)
	private string _myField;

	private int _myFieldWithProp;
	public int MyProperty
	{
		get { return _myFieldWithProp; }
		set { _myFieldWithProp = value; }
	}

	// prefer auto properties
	public string MyAutoProp { get; set; }

	public MyClass()
	{
		DoSomethingCrazy();
	}

	// the method comes right after it is called so you easily can read the code from up to down without searching for the method
	// to quickly understand what is happing, public methods can easily be found via IntelliSense or the ComboBox in VS above the code editor
	private void DoSomethingCrazy()
	{
	    // use var, except you want to assure using the interface (interfaces are up to 10x slower in c#)
		var someStr = CreateString();
		PrintString(someStr);

		// only use common abbreviations
		// known are:
		// str - just for variable names, int, bool
		// rect for rectangle

		// else write out names!
	}

	// method names CamelCase, always provide access modifier
	// use string not String, use bool not Boolean, use int not Int32 and so on

	/// <...>Use documentation headers for public methods<...>
	/// and for class headers so that people will know what you do
	public string CreateString()
	{
		// comment why you did something if it is not obvious, keep your comment short and clear but don't miss important information
		// often a comment can be spared if you use a good method name
		return new string();
	}

	private void PrintString(string str)
	{
		//...
	}

	public void SomeOtherPublicMethod()
	{
		// method length should mostly be a few lines, if you need switch, this should be the only statement the method is doing (yes you are allowed to use common sense)
		// switch often can be replaced/shortend with the strategy pattern; performance is a reason not to do it, but don't overengeneer
		// don't have deep switch/if hierarchies!
	}
}

# UI Classes

public class MedjaWindow : ContentControl
    {
	    // NameProperty where name is the name and Property to show that it is the property object
		// always right over the property
        public Property<string> TitleProperty { get; }
        public string Title
        {
            get { return TitleProperty.Get(); }
            set { TitleProperty.Set(value); }
        }

		// if you don't want to support data binding / change detection for the given property don't use the Property<T> class
        public bool IsClosed { get; set; }

        public event EventHandler Closed;

        // ...
    }