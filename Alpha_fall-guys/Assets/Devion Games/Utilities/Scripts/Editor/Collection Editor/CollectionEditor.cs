using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;

namespace DevionGames{
	/// <summary>
	/// Base class for a collection of items.
	/// </summary>
	[System.Serializable]
	public abstract class CollectionEditor<T> : ICollectionEditor {
		private const  float LIST_MIN_WIDTH = 200f;
		private const  float LIST_MAX_WIDTH = 400f;
		private const float LIST_RESIZE_WIDTH = 10f;

		protected Rect m_SidebarRect = new Rect(0,30,200,1000);
		protected Vector2 m_ScrollPosition;
		protected string m_SearchString=string.Empty;
		protected Vector2 m_SidebarScrollPosition;
		
		private bool m_StartDrag;
		private bool m_Drag;
		private Rect m_DragRect = Rect.zero;

		protected int m_SelectedItemIndex;
		protected T selectedItem {
			get {
				if (m_SelectedItemIndex > -1 && m_SelectedItemIndex < Items.Count) {
					return Items[m_SelectedItemIndex];
				}
				return default;
			}
		}

		protected abstract List<T> Items {get;}

		protected virtual bool CanAdd => true;

		protected virtual bool CanRemove => true;

		protected virtual bool CanDuplicate => true;

		protected virtual bool UseInspectorDefaultMargins => true;

		private string m_ToolbarName;

		public virtual string ToolbarName => !string.IsNullOrEmpty(this.m_ToolbarName) ? this.m_ToolbarName: (GetType().IsGenericType ? 
			ObjectNames.NicifyVariableName(GetType().GetGenericArguments()[0].Name) : 
			ObjectNames.NicifyVariableName(GetType().Name.Replace("Editor", "")));

		public CollectionEditor() { }

		public CollectionEditor(string title) {
			this.m_ToolbarName = title;
		}

		public virtual void OnEnable() {
			string prefix = "CollectionEditor." + ToolbarName + ".";
			this.m_SelectedItemIndex = EditorPrefs.GetInt(prefix + "m_SelectedItemIndex");
			this.m_SidebarRect.width = EditorPrefs.GetFloat(prefix + "m_SidebarRect.width", LIST_MIN_WIDTH);
			this.m_ScrollPosition.y = EditorPrefs.GetFloat(prefix + "m_Scrollposition.y");
			this.m_SidebarScrollPosition.y = EditorPrefs.GetFloat(prefix + "m_SidebarScrollPosition.y");

			if (this.m_SelectedItemIndex > -1 && this.m_SelectedItemIndex < Items.Count)
				Select(Items[this.m_SelectedItemIndex]);
		}
		
		public virtual void OnDisable() {
			string prefix = "CollectionEditor." + ToolbarName + ".";
			EditorPrefs.SetInt(prefix + "m_SelectedItemIndex",this.m_SelectedItemIndex);
			EditorPrefs.SetFloat(prefix + "m_SidebarRect.width", this.m_SidebarRect.width);
			EditorPrefs.SetFloat(prefix + "m_Scrollposition.y",this.m_ScrollPosition.y);
			EditorPrefs.SetFloat(prefix + "m_SidebarScrollPosition.y",this.m_SidebarScrollPosition.y);
		}

		public virtual void OnDestroy() { Debug.Log("OnDestroy " + ToolbarName); }

		public virtual void OnGUI(Rect position){
			
			DrawSidebar(new Rect(position.x, position.y, m_SidebarRect.width, position.height));
			DrawContent(new Rect(m_SidebarRect.width, m_SidebarRect.y, position.width - m_SidebarRect.width, position.height));
			ResizeSidebar();
		}

		private void DrawSidebar(Rect position) {
			m_SidebarRect = position;
			GUILayout.BeginArea(m_SidebarRect, "", Styles.leftPane);
			GUILayout.BeginHorizontal();
			DoSearchGUI();

			if (CanAdd)
			{
				GUIStyle style = new GUIStyle("ToolbarCreateAddNewDropDown");
				GUIContent content = EditorGUIUtility.IconContent("CreateAddNew");

				if (GUILayout.Button(content, style, GUILayout.Width(35f)))
				{
					GUI.FocusControl("");
					Create();
					if (Items.Count > 0)
					{
						Select(Items[Items.Count - 1]);
					}
				}
			}
			GUILayout.Space(1f);
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();


			m_SidebarScrollPosition = GUILayout.BeginScrollView(m_SidebarScrollPosition);

			List<Rect> rects = new List<Rect>();
			for (int i = 0; i < Items.Count; i++)
			{
				T currentItem = Items[i];
				if (!MatchesSearch(currentItem, m_SearchString) && Event.current.type == EventType.Repaint)
				{
					continue;
				}
				
				using (var h = new EditorGUILayout.HorizontalScope(Styles.selectButton, GUILayout.Height(25f)))
				{
					Color backgroundColor = GUI.backgroundColor;
					Color textColor = Styles.selectButtonText.normal.textColor;
					GUI.backgroundColor = Styles.normalColor;

					if (selectedItem != null && selectedItem.Equals(currentItem))
					{
						GUI.backgroundColor = Styles.activeColor;
						Styles.selectButtonText.normal.textColor = Color.white;
						Styles.selectButtonText.fontStyle = FontStyle.Bold;
					}else if (h.rect.Contains(Event.current.mousePosition))
					{
						GUI.backgroundColor = Styles.hoverColor;
						Styles.selectButtonText.normal.textColor = textColor;
						Styles.selectButtonText.fontStyle = FontStyle.Normal;
					}

					GUI.Label(h.rect, GUIContent.none, Styles.selectButton);
					Rect rect = h.rect;
					rect.width -= LIST_RESIZE_WIDTH * 0.5f;
					if (rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button == 0) {
						GUI.FocusControl("");
						Select(currentItem);
						this.m_StartDrag = true;
						Event.current.Use();
					}
					DrawItemLabel(i, currentItem);

					string error = HasConfigurationErrors(currentItem);
					if (!string.IsNullOrEmpty(error))
					{
						GUI.backgroundColor = Styles.warningColor;
						Rect errorRect = new Rect(h.rect.width - 20f, h.rect.y+4.5f, 16f, 16f);
						GUI.Label(errorRect, new GUIContent("",error), (GUIStyle)"CN EntryWarnIconSmall");
					}
					GUI.backgroundColor = backgroundColor;
					Styles.selectButtonText.normal.textColor = textColor;
					Styles.selectButtonText.fontStyle = FontStyle.Normal;
					rects.Add(rect);
				}
			}

			switch (Event.current.rawType)
			{
				case EventType.MouseDown:
					if(Event.current.button == 1)
					for (int j = 0; j < rects.Count; j++)
					{
						if (rects[j].Contains(Event.current.mousePosition))
						{
								ShowContextMenu(Items[j]);
								break;
						}
					}
					break;
				case EventType.MouseUp:
					if (this.m_Drag)
					{
						this.m_Drag = false;
						this.m_StartDrag = false;
						for (int j = 0; j < rects.Count; j++)
						{
							Rect rect = rects[j];
		
							Rect rect1 = new Rect(rect.x, rect.y, rect.width, rect.height * 0.5f);
							Rect rect2 = new Rect(rect.x, rect.y + rect.height * 0.5f, rect.width, rect.height * 0.5f);
							int index = j;
							if (index < this.m_SelectedItemIndex)
								index += 1;
							if (rect1.Contains(Event.current.mousePosition) && (index-1) > -1)
							{
								MoveItem(this.m_SelectedItemIndex, index-1);
								Select(Items[index-1]);
								break;
							}
							else if (rect2.Contains(Event.current.mousePosition))
							{
								MoveItem(this.m_SelectedItemIndex, index );
								Select(Items[index]);
								break;
							}
						}
						Event.current.Use();
					}
					break;
				case EventType.MouseDrag:
					if (this.m_StartDrag)
					{
						for (int j = 0; j < rects.Count; j++)
						{
							if (rects[j].Contains(Event.current.mousePosition))
							{
								this.m_Drag = true;
								break;
							}
						}
					}
					break;
			}

			for (int j = 0; j < rects.Count; j++)
			{

				Rect rect = rects[j];
				Rect rect1 = new Rect(rect.x, rect.y, rect.width, rect.height * 0.5f);
				Rect rect2 = new Rect(rect.x, rect.y + rect.height * 0.5f, rect.width, rect.height * 0.5f);

				if (rect1.Contains(Event.current.mousePosition))
				{
					m_DragRect = rect;
					m_DragRect.y = rect.y + 10f - 25f;
					m_DragRect.x = rect.x + 5f;
					break;
				}
				else if (rect2.Contains(Event.current.mousePosition))
				{
					m_DragRect = rect;
					m_DragRect.y = rect.y + 10f;
					m_DragRect.x = rect.x + 5f;

					break;
				}
				else
				{
					m_DragRect = Rect.zero;
				}
			}

			if (m_Drag){
				GUI.Label(m_DragRect,GUIContent.none, Styles.dragInsertion);
			}

			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		private void ShowContextMenu(T currentItem) {
			GenericMenu contextMenu = new GenericMenu();
			if (CanRemove)
				contextMenu.AddItem(new GUIContent("Delete"), false, delegate { Remove(currentItem); });
			if (CanDuplicate)
				contextMenu.AddItem(new GUIContent("Duplicate"), false, delegate { Duplicate(currentItem); });
			int oldIndex = Items.IndexOf(currentItem);
			if (CanMove(currentItem, oldIndex - 1))
			{
				contextMenu.AddItem(new GUIContent("Move Up"), false, delegate { MoveUp(currentItem); });
			}
			else
			{
				contextMenu.AddDisabledItem(new GUIContent("Move Up"));
			}
			if (CanMove(currentItem, oldIndex + 1))
			{
				contextMenu.AddItem(new GUIContent("Move Down"), false, delegate { MoveDown(currentItem); });
			}
			else
			{
				contextMenu.AddDisabledItem(new GUIContent("Move Down"));
			}

			AddContextItem(contextMenu);
			contextMenu.ShowAsContext();
		}

		protected virtual void AddContextItem(GenericMenu menu) { }

		protected virtual void DrawContent(Rect position) {
			
			GUILayout.BeginArea(position, "", Styles.centerPane);
			m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition, UseInspectorDefaultMargins?EditorStyles.inspectorDefaultMargins: GUIStyle.none);
			if (selectedItem != null)
			{
				DrawItem(selectedItem);
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		/// <summary>
		/// Select an item.
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void Select(T item){
			int index = Items.IndexOf(item);
			if (this.m_SelectedItemIndex != index)
			{
				this.m_SelectedItemIndex = index;
				this.m_ScrollPosition.y = 0f;
			}
		}

		/// <summary>
		/// Create an item.
		/// </summary>
		protected virtual void Create(){}

		/// <summary>
		/// Does the specified item has configuration errors
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected virtual string HasConfigurationErrors(T item) {
			return string.Empty;
		}

		/// <summary>
		/// Remove the specified item from collection.
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void Remove(T item){}

		/// <summary>
		/// Duplicates the specified item in collection
		/// </summary>
		/// <param name="item"></param>
		protected virtual void Duplicate(T item) { }

		/// <summary>
		/// Moves the item in database up.
		/// </summary>
		/// <param name="item"></param>
		protected virtual void MoveUp(T item) {
			int oldIndex = Items.IndexOf(item);
			MoveItem(oldIndex, oldIndex - 1);
			Select(Items[oldIndex - 1]);
		}

		/// <summary>
		/// Moves the item in database down.
		/// </summary>
		/// <param name="item"></param>
		protected virtual void MoveDown(T item) {
			int oldIndex = Items.IndexOf(item);
			MoveItem(oldIndex, oldIndex + 1);
			Select(Items[oldIndex + 1]);
		}

		protected virtual bool CanMove(T item, int newIndex) {
			int oldIndex = Items.IndexOf(item);

			if ((oldIndex == newIndex) || (0 > oldIndex) || (oldIndex >= Items.Count) || (0 > newIndex) || (newIndex >= Items.Count))
				return false;
			return true;
		}

		protected void MoveItem(int oldIndex, int newIndex)
		{
			if ((oldIndex == newIndex) || (0 > oldIndex) || (oldIndex >= Items.Count) || (0 > newIndex) ||
				(newIndex >= Items.Count)) return;

			T tmp = Items[oldIndex];
			if (oldIndex < newIndex)
			{
				for (int i = oldIndex; i < newIndex; i++)
				{
					Items[i] = Items[i + 1];
				}
			}
			else
			{
				for (int i = oldIndex; i > newIndex; i--)
				{
					Items[i] = Items[i - 1];
				}
			}
			Items[newIndex] = tmp;
		}

		/// <summary>
		/// Draws the item properties.
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void DrawItem(T item){}

		protected virtual void DrawItemLabel(int index, T item) {
			GUILayout.Label(ButtonLabel(index, item), Styles.selectButtonText);
		}

		/// <summary>
		/// Gets the sidebar label displayed in sidebar.
		/// </summary>
		/// <returns>The sidebar label.</returns>
		/// <param name="item">Item.</param>
		protected abstract string GetSidebarLabel(T item);

		protected virtual string ButtonLabel(int index, T item)
		{
			return index + ":  " + GetSidebarLabel(item);
		}

		/// <summary>
		/// Checks for search.
		/// </summary>
		/// <returns><c>true</c>, if search was matchesed, <c>false</c> otherwise.</returns>
		/// <param name="item">Item.</param>
		/// <param name="search">Search.</param>
		protected abstract bool MatchesSearch (T item, string search);

		protected virtual void DoSearchGUI(){
			m_SearchString = EditorTools.SearchField (m_SearchString);
		}

		private void ResizeSidebar(){
			Rect rect = new Rect (m_SidebarRect.width - LIST_RESIZE_WIDTH*0.5f, m_SidebarRect.y, LIST_RESIZE_WIDTH, m_SidebarRect.height);
			EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);
			int controlID = GUIUtility.GetControlID(FocusType.Passive);
			Event ev = Event.current;
			switch (ev.rawType) {
			case EventType.MouseDown:
				if(rect.Contains(ev.mousePosition)){
					GUIUtility.hotControl = controlID;
					ev.Use();
				}
				break;
			case EventType.MouseUp:
				if (GUIUtility.hotControl == controlID)
				{
					GUIUtility.hotControl = 0;
					ev.Use();
				}
				break;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == controlID)
				{
					this.m_Drag = false;
					m_SidebarRect.width=ev.mousePosition.x;
					m_SidebarRect.width=Mathf.Clamp(m_SidebarRect.width,LIST_MIN_WIDTH,LIST_MAX_WIDTH);
                    EditorPrefs.SetFloat("CollectionEditorSidebarWidth"+ToolbarName,m_SidebarRect.width);
					ev.Use();
				}
				break;
			}
		}

        public static class Styles{
			public static GUIStyle minusButton;
			public static GUIStyle selectButton;
			public static GUIStyle background;

			private static GUIStyle m_LeftPaneDark;
			private static GUIStyle m_LeftPaneLight;
			public static GUIStyle leftPane {
				get {return EditorGUIUtility.isProSkin ? m_LeftPaneDark : m_LeftPaneLight;}
			}

			private static GUIStyle m_CenterPaneDark;
			private static GUIStyle m_CenterPaneLight;
			public static GUIStyle centerPane {
				get { return EditorGUIUtility.isProSkin ? m_CenterPaneDark : m_CenterPaneLight; }
			}

			public static GUIStyle selectButtonText;
			public static Color normalColor;
			public static Color hoverColor;
			public static Color activeColor;
			public static Color warningColor;
			public static GUIStyle dragInsertion;
			public static Texture2D errorIcon;

			public static GUIStyle indicatorColor;

			private static GUISkin skin;


			static Styles(){
				skin = Resources.Load<GUISkin>("EditorSkin");
				m_LeftPaneLight = skin.GetStyle("Left Pane");
				m_CenterPaneLight = skin.GetStyle("Center Pane");
				m_LeftPaneDark = skin.GetStyle("Left Pane Dark");
				m_CenterPaneDark = skin.GetStyle("Center Pane Dark");

				normalColor = EditorGUIUtility.isProSkin? new Color(0.219f, 0.219f, 0.219f, 1f) : new Color(0.796f, 0.796f, 0.796f, 1f);
				hoverColor = EditorGUIUtility.isProSkin ? new Color(0.266f, 0.266f, 0.266f, 1f):new Color(0.69f,0.69f,0.69f,1f);
				activeColor = EditorGUIUtility.isProSkin ? new Color(0.172f, 0.364f, 0.529f, 1f):new Color(0.243f,0.459f,0.761f,1f);
				warningColor = new Color(0.9f,0.37f,0.32f,1f);
				errorIcon = EditorGUIUtility.LoadRequired("console.erroricon") as Texture2D;

				minusButton = new GUIStyle ("OL Minus"){
					margin=new RectOffset(0,0,4,0)
				};
				selectButton = new GUIStyle("MeTransitionSelectHead")
				{
					alignment = TextAnchor.MiddleLeft,
					padding = new RectOffset(5, 0, 0, 0),
					overflow = new RectOffset(0, -1, 0, 0),
				};
				selectButton.normal.background = ((GUIStyle)"ColorPickerExposureSwatch").normal.background;

				selectButtonText = new GUIStyle("MeTransitionSelectHead")
				{
					alignment = TextAnchor.MiddleLeft,
					padding = new RectOffset(5, 0, 0, 0),
					overflow = new RectOffset(0, -1, 0, 0),
					richText = true
				};
				selectButtonText.normal.background = null;
				selectButtonText.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.788f, 0.788f, 0.788f, 1f) : new Color(0.047f, 0.047f, 0.047f, 1f);
				background = new GUIStyle("PopupCurveSwatchBackground");
				dragInsertion = new GUIStyle("PR Insertion");
				indicatorColor = new GUIStyle(selectButton);
				indicatorColor.margin = new RectOffset(0,0,4,0);
				indicatorColor.padding = new RectOffset(1,1,1,1);
			}
		}
	}
}