using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DevionGames
{
	[CustomPropertyDrawer (typeof(HeaderLineAttribute))]
	public class HeaderLineDrawer : DecoratorDrawer
	{
		public HeaderLineDrawer ()
		{
		}

		public override float GetHeight ()
		{
			return EditorGUIUtility.singleLineHeight+8f;
		}

		public override void OnGUI (Rect position)
		{
			position.y = position.y + 2f;
			position = EditorGUI.IndentedRect (position);
            GUI.Label(position,(base.attribute as HeaderLineAttribute).header,TextStyle);
            position.y += EditorGUIUtility.singleLineHeight+2f;
			Color color = GUI.color;
			GUI.color = EditorGUIUtility.isProSkin ? new Color(0.788f, 0.788f, 0.788f, 0.2f) : new Color(0.047f, 0.047f, 0.047f, 1f);
			GUI.Label (position, "", LineStyle);
			GUI.color = color;
		}

		private GUIStyle m_LineStyle;

		private GUIStyle LineStyle {
			get {
				if (this.m_LineStyle == null) {

                    this.m_LineStyle = new GUIStyle();
                    this.m_LineStyle.fixedHeight = 1f;
					this.m_LineStyle.margin = new RectOffset();
					this.m_LineStyle.padding = new RectOffset();

					this.m_LineStyle.normal.background = EditorGUIUtility.whiteTexture;
					/* line = new GUIStyle ("ProjectBrowserHeaderBgMiddle");
                     line.fontSize = 14;
                     line.fontStyle = FontStyle.Bold;
                     line.normal.textColor = ((GUIStyle)"label").normal.textColor;*/
				}
				return this.m_LineStyle;
			}
		}

        private GUIStyle m_TextStyle;
        private GUIStyle TextStyle {
            get {
                if (this.m_TextStyle == null) {
                    this.m_TextStyle = new GUIStyle(EditorStyles.boldLabel);
                    this.m_TextStyle.fontSize = 12;

                }
                return this.m_TextStyle;
            }
        }
	}
}