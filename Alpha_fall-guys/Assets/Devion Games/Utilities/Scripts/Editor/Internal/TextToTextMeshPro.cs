using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames
{
	public class TextToTextMeshPro : EditorWindow
	{

		[UnityEditor.MenuItem("Tools/Devion Games/Internal/Update to TextMeshPro", false)]
		public static void ShowWindow()
		{
			TextToTextMeshPro window = EditorWindow.GetWindow<TextToTextMeshPro>("Update to TextMeshPro");
			Vector2 size = new Vector2(300f, 80f);
			window.minSize = size;
			window.wantsMouseMove = true;
		}

		private Text[] m_Texts;
		private Dictionary<Font, TMP_FontAsset> m_FontMap;
		private float outlineRatio = 0.1f;
		private float shadowRatio = 0.2f;

        private void OnEnable()
        {
			this.m_FontMap = new Dictionary<Font, TMP_FontAsset>();
			this.m_Texts = GameObject.FindObjectsOfType<Text>();
			for (int i = 0; i < this.m_Texts.Length; i++) {
				if (!this.m_FontMap.ContainsKey(this.m_Texts[i].font))
					this.m_FontMap.Add(this.m_Texts[i].font, null);
			}
        }

        private void OnGUI()
        {
			EditorGUILayout.LabelField("Font:", EditorStyles.boldLabel);
			EditorGUILayout.HelpBox("Replace the Text Font with TextMeshPro FontAsset.", MessageType.Info);
			foreach (Font key in this.m_FontMap.Keys.ToList()) {
				EditorGUILayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup(true);
				EditorGUILayout.ObjectField(GUIContent.none, key, typeof(Font), false);
				EditorGUI.EndDisabledGroup();
				this.m_FontMap[key] = (TMP_FontAsset)EditorGUILayout.ObjectField(GUIContent.none, this.m_FontMap[key], typeof(TMP_FontAsset), false);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.LabelField("Effects:", EditorStyles.boldLabel);
			outlineRatio = EditorGUILayout.FloatField("Outline Ratio",outlineRatio);
			shadowRatio = EditorGUILayout.FloatField("Shadow Ratio", shadowRatio);

			if (GUILayout.Button("Update to TextMeshPro")) {
				for (int i = 0; i < this.m_Texts.Length; i++) {
					UpdateToTextMeshPro(this.m_Texts[i]);
				}
			}
		}

		private void UpdateToTextMeshPro(Text component) {
			bool enabled = component.enabled;
			string text = component.text;
			TMP_FontAsset font = this.m_FontMap[component.font];
			TMPro.FontStyles fontStyles = FontStyles.Normal;
			switch (component.fontStyle) {
				case FontStyle.Bold:
					fontStyles = FontStyles.Bold;
					break;
				case FontStyle.BoldAndItalic:
					fontStyles = FontStyles.Bold | FontStyles.Italic;
					break;
				case FontStyle.Italic:
					fontStyles = FontStyles.Italic;
					break;
			}
			int fontSize = component.fontSize;
			Color color = component.color;
			bool richText = component.supportRichText;
			TextAlignmentOptions alignment = TextAlignmentOptions.TopLeft;
			switch (component.alignment) {
				case TextAnchor.LowerCenter:
					alignment = TextAlignmentOptions.Bottom;
					break;	
				case TextAnchor.LowerLeft:
					alignment = TextAlignmentOptions.BottomLeft;
					break;
				case TextAnchor.LowerRight:
					alignment = TextAlignmentOptions.BottomRight;
					break;
				case TextAnchor.MiddleCenter:
					alignment = TextAlignmentOptions.Center;
					break;
				case TextAnchor.MiddleLeft:
					alignment = TextAlignmentOptions.Left;
					break;
				case TextAnchor.MiddleRight:
					alignment = TextAlignmentOptions.Right;
					break;
				case TextAnchor.UpperCenter:
					alignment = TextAlignmentOptions.Top;
					break;
				case TextAnchor.UpperLeft:
					alignment = TextAlignmentOptions.TopLeft;
					break;
				case TextAnchor.UpperRight:
					alignment = TextAlignmentOptions.TopRight;
					break;
			}
			bool wrap = component.horizontalOverflow == HorizontalWrapMode.Wrap ? true : false;
			TextOverflowModes overflowModes = component.verticalOverflow == VerticalWrapMode.Overflow? TextOverflowModes.Overflow:TextOverflowModes.Truncate;
			bool autoSize = component.resizeTextForBestFit;
			float minFontSize = component.resizeTextMinSize;
			float maxFontSize = component.resizeTextMaxSize;
			bool raycastTarget = component.raycastTarget;
			bool maskable = component.maskable;
			Outline outline = component.GetComponent<Outline>();
			bool hasOutline = outline != null;
			float outlineThickness = 0f;
			Color outlineColor= Color.black;
			if (hasOutline) {
				outlineThickness = ((Mathf.Abs(outline.effectDistance.x) +Mathf.Abs(outline.effectDistance.y))*0.5f)* outlineRatio;
				outlineColor = outline.effectColor;
				GameObject.DestroyImmediate(outline);
			}

			Shadow shadow = component.GetComponent<Shadow>();
			bool hasShadow = shadow != null;
			Color shadowColor= Color.black;
			float offsetX = 0f;
			float offsetY = 0f;
			if (hasShadow) {
				offsetX = shadow.effectDistance.x * shadowRatio;
				offsetY = shadow.effectDistance.y * shadowRatio;
				GameObject.DestroyImmediate(shadow);
			}

			GameObject go = component.gameObject;
			DestroyImmediate(component);
			TextMeshProUGUI textMeshPro = go.AddComponent<TextMeshProUGUI>();
			textMeshPro.SetText(text);
			textMeshPro.font = font;
			textMeshPro.fontStyle = fontStyles;
			textMeshPro.fontSize = fontSize;
			textMeshPro.color = color;
			textMeshPro.richText = richText;
			textMeshPro.alignment = alignment;
			textMeshPro.enableWordWrapping = wrap;
			textMeshPro.overflowMode = overflowModes;
			textMeshPro.enableAutoSizing = autoSize;
			textMeshPro.fontSizeMin = minFontSize;
			textMeshPro.fontSizeMax = maxFontSize;
			textMeshPro.raycastTarget = raycastTarget;
			textMeshPro.maskable = maskable;

			Material material = textMeshPro.fontMaterial;
			if (hasOutline)
			{
				material.EnableKeyword(ShaderUtilities.Keyword_Outline);
				material.SetColor("_OutlineColor", outlineColor);
				material.SetFloat("_OutlineWidth", outlineThickness);
			}
			else {
				material.DisableKeyword(ShaderUtilities.Keyword_Outline);
			}

			if (hasShadow)
			{
				
				material.EnableKeyword(ShaderUtilities.Keyword_Underlay);

				material.SetColor("_UnderlayColor", shadowColor);
				material.SetFloat("_UnderlayOffsetX", offsetX);
				material.SetFloat("_UnderlayOffsetY", offsetY);
				material.SetFloat("_UnderlayDilate", 0f);
				material.SetFloat("_UnderlaySoftness", 0.2f);
			}
			else {
				material.DisableKeyword(ShaderUtilities.Keyword_Underlay);
			}

			textMeshPro.enabled = enabled;
		}
    }
}