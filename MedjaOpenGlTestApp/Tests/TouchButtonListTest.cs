using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;
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
            var touchButtonList1 = _controlFactory.Create<TouchButtonList<string>>(p =>
            {
                p.InitializeButtonFromItem = (text, button) => button.Text = text;
                p.IsSelectable = true;
                p.PageSize = 5;
            
                for(int i = 0; i < 10; i++)
                    p.AddItem("Item " + i);

                p.SelectedItem = p.Items[2];
            });

            var comboBox = _controlFactory.Create<ComboBox<string>>(p =>
            {
                p.HorizontalAlignment = HorizontalAlignment.Stretch;
                p.Position.Height = 40;
                
                for(int i = 0; i < 10; i++)
                    p.AddItem("2 Item " + i);

                p.SelectedItem = p.Items[0];
            });

            var comboBoxContainer = _controlFactory.Create<ContentControl>();
            comboBoxContainer.Content = comboBox;
            

            var tablePanel = _controlFactory.Create<TablePanel>();
            tablePanel.Columns.Add(new ColumnDefinition(350));
            tablePanel.Columns.Add(new ColumnDefinition(350));
            tablePanel.Rows.Add(new RowDefinition(500));
            tablePanel.Children.Add(touchButtonList1);
            tablePanel.Children.Add(comboBoxContainer);
            
            return tablePanel;
            
            /*_digitalInputs = _controlFactory.Create<TouchButtonList<MsgDigitalInput>>();
            _digitalInputs.InitializeButtonFromItem = (item, button) => { button.Text = "Eingang " + item.Id; };
            _digitalInputs.Position.Width = 200;
            _digitalInputs.PropertySelectedItem.PropertyChanged += OnSelectedDigitalInputChanged;
            _digitalInputs.IsSelectable = true;*/
        }
    }
}