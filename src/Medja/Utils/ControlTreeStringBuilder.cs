using System.Text;
using Medja.Controls;

namespace Medja.Utils
{
    public class ControlTreeStringBuilder
    {
        private readonly Control _control;
        private readonly StringBuilder _stringBuilder;
        
        public ControlTreeStringBuilder(Control control)
        {
            _control = control;
            _stringBuilder = new StringBuilder();
        }

        public string GetTree()
        {
            if (_stringBuilder.Length != 0)
                return _stringBuilder.ToString();

            AddControl(_control, 0);

            return _stringBuilder.ToString();
        }

        private void AddControl(Control control, int currentIndention)
        {
            if (currentIndention > 0)
            {
                _stringBuilder.Append(' ', currentIndention);
                _stringBuilder.Append("+- ");
            }

            _stringBuilder.Append(control.ToString());
            _stringBuilder.Append(": Pos {");
            _stringBuilder.Append(control.Position.X);
            _stringBuilder.Append(", ");
            _stringBuilder.Append(control.Position.Y);
            _stringBuilder.Append(", ");
            _stringBuilder.Append(control.Position.Width);
            _stringBuilder.Append(", ");
            _stringBuilder.Append(control.Position.Height);
            _stringBuilder.Append("}");
            _stringBuilder.AppendLine();

            currentIndention += currentIndention == 0 ? 2 : 4;
            
            foreach(var child in control.GetChildren())
                AddControl(child, currentIndention);
        }
    }
}