using System;

namespace DevionGames
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class CategoryAttribute : Attribute
	{
		private readonly string category;

		public string Category
		{
			get
			{
				return this.category;
			}
		}

		public CategoryAttribute(string category)
		{
			this.category = category;
		}
	}
}