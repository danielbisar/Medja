using System;

namespace Medja.Reflection
{
	public static class ReflectionExtensions
	{
		public static string GetNameWithoutGenericArity(this Type type)
		{
			string name = type.Name;
			int index = name.IndexOf('`');
			return index == -1 ? name : name.Substring(0, index);
		}
	}
}
