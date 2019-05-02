using System;

namespace Medja.Utils.Reflection
{
	public static class TypeExtensions
	{
		/// <summary>
		/// Gets a <see cref="Type"/>s name, without the generic part (if any).
		/// </summary>
		/// <param name="type">The <see cref="Type"/></param>
		/// <returns>If the <see cref="Type"/>s name is My.Type`[...] the result will be My.Type.</returns>
		public static string GetNameWithoutGenericArity(this Type type)
		{
			var name = type.Name;
			var index = name.IndexOf('`');
			
			return index == -1 ? name : name.Substring(0, index);
		}
	}
}
