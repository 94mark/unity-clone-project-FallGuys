using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
	public class InspectorLabelAttribute : PropertyAttribute
	{
		public readonly string label;
        public readonly string tooltip;

        public InspectorLabelAttribute(string label) :this(label,string.Empty)
        {
        }

        public InspectorLabelAttribute (string label, string tooltip)
		{
			this.label = label;
            this.tooltip = tooltip;
		}
	}
}