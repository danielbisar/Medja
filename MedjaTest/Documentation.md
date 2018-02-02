basic example

--------------

MainWindow

AppWindow 
{
	Content = LayerPanel
	{
		3DGraphView,
		Menu 
		{
			HorizontalAlignment = Right, // or could be a container that aligns the content?			
		}
	}
}

Menu

VerticalStackPanel 
{
	SpaceBetweenChildren = 0.5,
	Items = MenuController.CurrentMenu.Items
	BindingActions 
	{
		ReloadItems = MenuController.CurrentMenuChanged // in this case we don't need a complete ObservableCollection or something just need to update the whole list, if the menu changes complete
		// this basically gets translated in MenuController.CurrentMenuChanged += (s,e) => VerticalStackPanel.ReloadItems();
		Items 
		{
			Click = .Command
		}
	}
}

// not currently we might have a lack of automatic support of DataContext changed, MenuController changed, Items changed; but
// here we just need the bit above


3DGraphView.cs

class 3DGraphView : Control
{
	Array[,,] DataPoints;
	
	void Render()
	{
		GL.Begin(Points); // or lines or plane or ...
		
		for(int x; x < DataPoints.XLength; x++)
		{
			...
			
			GL.Color(GetColor(Y));
			GL.Vertex3d(x,y,z);
			
		}
		
		GL.End();
	}	
}

-------------------------------

MainWindow()
{
	void Render()
	{
		3DGraphView.Render();
		Menu.Render();		
	}
	
	void HandleMouseClick(X, Y)
	{
		var control = GetControlAtScreenPoint(x,y);
		
		if(control != null)
		{
			// maybe TranslatePoint(x,y)??
			control.NotifyMouseDown(); // ??
		}
	}
	
	Control GetControlAtScreenPoint(x,y)
	{
		if(!IsPointWithinWindow(x,y))
			return;
	
		var control = Content;
		
checkcontrol:
		if(!control.HasChildren)
			return control;
		
		foreach(var child in control.Children)
		{
			if(IsHit(child, x, y)) // checks if IsHitTestVisible and under point
			{
				control = child;
				goto checkcontrol;
			}
		}
		
		return control;
	}
}




//DataContext = MenuController,	
	//ItemsGenerator 
	//{
		//Source = // can't take: MenuStructureFileName - menu has a state (current menu etc)
		//SourceConverter = MenuStructureParser // ???
	//}

// TODO animation of menu sliding in the stackpanel


-------------------------------------------------

Separate layouting / rendering / control logic / business logic

Layouter
{
	List<ControlState> _controls;

	Layout GetLayout()
	{
		return MeasureAndArrange();
	}
}

Renderer 
{
	List<ControlState> _controls;
	...
	void Render()
	{
		foreach(var control in _controls)
		{
			Render(control);
		}
	}

	void Render(Control control)
	{
		// different ways (pattern matching, dictionary that contains the appropriate rendering method)
	}

	void DrawRect(...)
	{
	}

	void DrawText(...)
	{
	}
}

ApplicationLoop
{
	Loop()
	{
		// this basically allows kind of a connected pipeline
		// layout -> filter (what is not visible) -> effects -> animation -> filter -> render
		// so each responsability is separated
		// the lib does not have to implement everything (f.e. no renderer)
		
		var layout = Layouter.GetLayout();
		var inputState = GetInputState(); // gets the state of all relevant input values (which keys are pressed, which pointing device, which pointing device buttons)
		Input.HandelInput(inputState); // <- contains also the layouted list of controls
		Renderer.Render(layout);
	}
}

A control does not define it's position, this is handled via layouting (positioninfo is kind of attached)

ControlState
{
	Control Control;
	PositionInfo pos;
	// other state infos (except the controls ones)
}

Infos for controls
- Lists (ComboBox, ListBox, ...)
- Text (TextBox, TextBlock, Button, Label, ...)
- IsPressed (f.e. Button)
-> input states
	- IsMouseOver
	- IsLeftMouseDown
	- IsRightMouseDown

Layoutinfos
- Position: X, Y, Width, Height


Control state changes can be adjusted via 

Control.PropertyX.Changed += OnXChanged;

Handle Input

HandleInput
{
	List<ControlState> _controls;

	void HandleInput(InputState state)
	{
		// find controls under cursor / use the last one and forward the input information
		// f.e. 

		var controls = GetControlsUnderCursor(state.CursorPos);

		foreach(var control in controls)
		{
			// this OR see below
			if(state.IsMouseDown)
				control.IsMouseOver = true;

			if(state.IsMousePressed)
				control.IsMousePressed = true;

			// alternative
			control.InputState = state;

			// this will just work fine or states that only change the appearance of a control
			// what about executing actions? like a button click calls a method if the button IsEnabled
		}
	}
}
