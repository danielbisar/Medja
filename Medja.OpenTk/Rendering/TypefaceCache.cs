using System;
using System.Collections.Generic;
using Medja.Primitives;
using SkiaSharp;
using System.Linq;

namespace Medja.OpenTk.Rendering
{
	// TODO regular cleanup?
	public class TypefaceCache : IDisposable
	{
		private bool _isDisposed;
		private readonly Dictionary<int, SKTypeface> _typefaces;

		public TypefaceCache()
		{
			_typefaces = new Dictionary<int, SKTypeface>();
		}

		private SKTypeface _myTypeface;

		public SKTypeface Get(Font font)
		{
			if (_isDisposed)
				throw new ObjectDisposedException(nameof(TypefaceCache));

			if (string.IsNullOrEmpty(font.Name))
				return null;

			var key = font.Name.GetHashCode() + (int)font.Style;

			if (_myTypeface != null)
				return _myTypeface;

			_myTypeface = CreateTypeface(font);
			return _myTypeface;




			//

			//SKTypeface result = null;

			//if (!_typefaces.TryGetValue(key, out result))
			//	result = CreateTypeface(font);
			//_myTypeface = result;
			//return result;
		}

		private SKTypeface CreateTypeface(Font font)
		{
			return SKTypeface.FromFamilyName(font.Name, font.Style.ToSKTypefaceStyle());
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;

				foreach (var typeface in _typefaces.Values)
					typeface.Dispose();

				_typefaces.Clear();
			}
		}
	}
}
