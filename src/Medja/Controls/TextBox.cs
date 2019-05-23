using System;
using System.Threading;
using Medja.Input;
using Medja.Primitives;

namespace Medja.Controls
{
	public class TextBox : TextControl
	{
		private bool _selfChangedText;
		private readonly Timer _timer;
		
		public readonly Property<int> PropertyCaretPos;
		public int CaretPos
		{
			get { return PropertyCaretPos.Get(); }
			set { PropertyCaretPos.Set(value); }
		}

		public readonly Property<bool> PropertyIsCaretVisible;

		public bool IsCaretVisible
		{
			get { return PropertyIsCaretVisible.Get(); }
			set { PropertyIsCaretVisible.Set(value); }
		}
		
		public TextBox()
		{
			PropertyCaretPos = new Property<int>();
			PropertyText.UnnotifiedSet(string.Empty);
			PropertyText.PropertyChanged += OnTextChanged;
			PropertyTextWrapping.UnnotifiedSet(TextWrapping.None);
			PropertyIsCaretVisible = new Property<bool>();
			
			_timer = new Timer(OnTimerTick, null, TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(1000));
			PropertyIsFocused.PropertyChanged += OnFocusChanged;
			
			InputState.KeyPressed += OnKeyPressed;
			InputState.OwnsMouseEvents = true;
		}

		private void OnFocusChanged(object sender, PropertyChangedEventArgs e)
		{
			if (IsFocused == false)
				IsCaretVisible = false;
		}

		private void OnTimerTick(object state)
		{
			MedjaApplication.Instance.Library.TaskQueue.TryEnqueue(p =>
			{
				if(IsFocused)
					IsCaretVisible = !IsCaretVisible;

				return null;
			}, null);
		}

		private void OnTextChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_selfChangedText)
				return;
			
			CaretPos = Text.Length;
		}

		private void OnKeyPressed(object sender, KeyboardEventArgs e)
		{
			_selfChangedText = true;

			if (e.KeyChar == '\b')
			{
				if (Text.Length > 0)
				{
					Text = Text.Substring(0, CaretPos - 1) + Text.Substring(CaretPos);
					CaretPos--;
				}
			}
			else if (e.KeyChar == (char) Keys.Delete)
			{
				if (CaretPos < Text.Length)
				{
					Text = Text.Substring(0, CaretPos) + Text.Substring(CaretPos + 1);
				}
			}
			else if (e.KeyChar == (char)Keys.Left)
			{
				if(CaretPos > 0)
					CaretPos--;
			}
			else if (e.KeyChar == (char) Keys.Right)
			{
				if(CaretPos < Text.Length)
					CaretPos++;	
			}
			else
			{
				Text = Text.Substring(0, CaretPos) + e.KeyChar + Text.Substring(CaretPos);
				CaretPos++;
			}
			
			_selfChangedText = false;
		}

		protected override void Dispose(bool disposing)
		{
			_timer.Dispose();
			base.Dispose(disposing);
		}
	}
}
