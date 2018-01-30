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