using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;
using Medja.Utils;
using OpenTK.Platform.Windows;

namespace MedjaOpenGlTestApp.Tests
{
    public class TouchButtonListTest
    {
        private readonly ControlFactory _controlFactory;

        public TouchButtonListTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            var touchButtonList1 = _controlFactory.Create<TouchButtonList<MyItemType>>(p =>
            {
                //p.InitButtonFromItem = (item, button) => button.Text = item.Name;
                p.IsSelectable = true;
                p.PageSize = 5;
                p.ButtonClicked += (s, e) =>
                {
                    if((e.Item as MyItemType)?.Name == "Remove some item")
                        p.RemoveItem(p.SelectedItem);
                };
                
                p.AddItem(new MyItemType { Name = "Remove some item"});
                
                for(int i = 0; i < 10; i++)
                    p.AddItem(MyItemType.Create());

                p.SelectedItem = p.Items[2];
            });

            var tablePanel = _controlFactory.Create<TablePanel>();
            tablePanel.Columns.Add(new ColumnDefinition(700));
            tablePanel.Columns.Add(new ColumnDefinition(100));
            tablePanel.Rows.Add(new RowDefinition(300));
            tablePanel.Rows.Add(new RowDefinition(30));
            tablePanel.Children.Add(touchButtonList1);
            tablePanel.Children.Add(new Control());
            tablePanel.Children.Add(new Control());
            tablePanel.Children.Add(_controlFactory.Create<Button>(p =>
            {
                p.Text = "Change random item.";
                p.InputState.MouseClicked += (s, e) =>
                {
                    var item = MyItemType.Random.NextItem(touchButtonList1.Items);
                    item.Name = MyItemType.Random.NextString(10);
                    touchButtonList1.UpdateItem(item);
                };
            }));
            
            return tablePanel;
            
            /*_digitalInputs = _controlFactory.Create<TouchButtonList<MsgDigitalInput>>();
            _digitalInputs.InitializeButtonFromItem = (item, button) => { button.Text = "Eingang " + item.Id; };
            _digitalInputs.Position.Width = 200;
            _digitalInputs.PropertySelectedItem.PropertyChanged += OnSelectedDigitalInputChanged;
            _digitalInputs.IsSelectable = true;*/
        }

        private class MyItemType
        {
            public static readonly Random Random = new Random();
            
            public string Name { get; set; }
            public int N { get; set; }

            public static MyItemType Create()
            {
                return new MyItemType
                {
                        Name = Random.NextString(10),
                        N = Random.Next()
                };
            }

            public override string ToString()
            {
                if (Name != null && Name.StartsWith("Remove"))
                    return Name;
                
                return $"Item with Name = {Name} and N = {N}";
            }
        }
    }
}