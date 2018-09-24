using System;

namespace Conqueror
{
	internal class JsonPropertyAttribute : Attribute
	{
		private string v;

		public JsonPropertyAttribute(string v)
		{
			this.v = v;
		}
	}
}