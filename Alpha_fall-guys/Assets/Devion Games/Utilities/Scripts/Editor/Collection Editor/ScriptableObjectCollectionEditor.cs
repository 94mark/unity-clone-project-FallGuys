using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DevionGames
{
	/// <summary>
	/// A collection class for ScriptableObjects.
	/// </summary>
	[System.Serializable]
	public class ScriptableObjectCollectionEditor<T> : CollectionEditor<T> where T: ScriptableObject, INameable
	{
		[SerializeField]
		protected List<T> items;
		protected override List<T> Items
		{
			get{ return items; }
		}

		[SerializeField]
		protected UnityEngine.Object target;

		protected Editor editor;

		protected bool m_UseInspectorDefaultMargins = false;
        protected override bool UseInspectorDefaultMargins  => this.m_UseInspectorDefaultMargins;

		public ScriptableObjectCollectionEditor(UnityEngine.Object target, List<T> items, bool useInspectorDefaultMargins = true):this(string.Empty, target, items, useInspectorDefaultMargins)
		{
		}

		public ScriptableObjectCollectionEditor (string title, UnityEngine.Object target, List<T> items, bool useInspectorDefaultMargins=true):base(title)
		{
			this.target = target;
			this.items = items;
			this.m_UseInspectorDefaultMargins = useInspectorDefaultMargins;
        }

		protected override bool MatchesSearch(T item, string search)
		{
			return (item.Name.ToLower().Contains(search.ToLower()) || search.ToLower() == item.GetType().Name.ToLower());
		}

		protected override void Create ()
		{
			Type[] types = AppDomain.CurrentDomain.GetAssemblies ().SelectMany (assembly => assembly.GetTypes ()).Where (type => typeof(T).IsAssignableFrom (type) && type.IsClass && !type.IsAbstract).ToArray ();		
			if (types.Length > 1) {
				GenericMenu menu = new GenericMenu ();
				foreach (Type type in types) {
					Type mType = type;
					menu.AddItem (new GUIContent (ObjectNames.NicifyVariableName(mType.Name)), false, delegate() {
						CreateItem (mType);
					});		
				}
				menu.ShowAsContext ();
			} else {
				CreateItem (types [0]);
			}
		}

		protected virtual void CreateItem (Type type)
		{

			T item = (T)ScriptableObject.CreateInstance (type);
			item.hideFlags = HideFlags.HideInHierarchy;
			AssetDatabase.AddObjectToAsset (item, target);
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
			Items.Add (item);
			Select(item);

			EditorUtility.SetDirty (target);
		}

		protected override void Remove (T item)
		{
			if (EditorUtility.DisplayDialog ("Delete Item", "Are you sure you want to delete " + item.Name + "?", "Yes", "No")) {
				GameObject.DestroyImmediate (item, true);
				AssetDatabase.SaveAssets ();
				AssetDatabase.Refresh ();
				Items.Remove (item);
				base.m_SelectedItemIndex = -1;
				if (editor != null)
					ScriptableObject.DestroyImmediate(editor);
				EditorUtility.SetDirty (target);
			}
		}

        protected override void Duplicate(T item)
        {
			T duplicate = (T)ScriptableObject.Instantiate(item);
			duplicate.hideFlags = HideFlags.HideInHierarchy;
			AssetDatabase.AddObjectToAsset(duplicate, target);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			Items.Add(duplicate);
			Select(duplicate);
			EditorUtility.SetDirty(target);
		}

        protected override void Select(T item)
        {
            base.Select(item);
			if (editor != null)
				ScriptableObject.DestroyImmediate(editor);

			editor = Editor.CreateEditor(item);
		}

        protected override void DrawItem (T item)
		{
			if (editor != null) {
				editor.OnInspectorGUI ();
			}
		}

        public override void OnDestroy()
        {
			if(editor != null)
				ScriptableObject.DestroyImmediate(editor);
        }

        protected override string GetSidebarLabel (T item)
		{
			return item.Name;
		}

        protected override void AddContextItem(GenericMenu menu)
        {
            base.AddContextItem(menu);
			menu.AddItem(new GUIContent("Sort/A-Z"), false, delegate {
				T selected = selectedItem;
				Items.Sort(delegate (T a, T b) {return a.Name.CompareTo(b.Name); });
				Select(selected);
				});
			menu.AddItem(new GUIContent("Sort/Type"), false, delegate {
				T selected = selectedItem;
				Items.Sort(delegate (T a, T b) {
					return a.GetType().FullName.CompareTo(b.GetType().FullName);
				});
				Select(selected);
			});
		}


    }
}