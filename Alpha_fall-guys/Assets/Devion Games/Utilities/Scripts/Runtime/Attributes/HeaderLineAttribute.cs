using UnityEngine;
using System.Collections;

namespace DevionGames{
	public class HeaderLineAttribute : PropertyAttribute {
		/// <summary>
		///   <para>The header text.</para>
		/// </summary>
		public readonly string header;
		
		/// <summary>
		///   <para>Add a header above some fields in the Inspector.</para>
		/// </summary>
		/// <param name="header">The header text.</param>
		public HeaderLineAttribute(string header)
		{
			this.header = header;
		}
	}
}