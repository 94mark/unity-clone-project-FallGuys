using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EffectToolBar{

	public static void SelectedObjAddComponent<T>(string notSelectStr,string hasComponentStr) where T: UnityEngine.MonoBehaviour{
		UnityEngine.Object[] selectObjList = Selection.GetFiltered(typeof(GameObject), SelectionMode.Unfiltered);
		if (selectObjList.Length == 0)
		{
			EditorUtility.DisplayDialog("", notSelectStr, "OK");
			return;
		}
		foreach (var go in selectObjList)
		{
			GameObject Go = go as GameObject;
			if (Go.GetComponent<T>())
			{
				EditorUtility.DisplayDialog("", hasComponentStr, "OK");
				continue;
			}
			Go.AddComponent<T>();
		}
	}

	public static GameObject[] InstaniceEmptyPrimitiveType(string objName,PrimitiveType primitiveType){
		List<GameObject> gameobjectList = new List<GameObject> ();
		UnityEngine.Object[] selectObjList = Selection.GetFiltered(typeof(GameObject), SelectionMode.Unfiltered);
		if (selectObjList.Length > 1) {
			EditorUtility.DisplayDialog("", "Select To", "OK");
			return null;
		} else if (selectObjList.Length == 0) {
            GameObject go = GameObject.CreatePrimitive(primitiveType);
			go.name = objName;
			go.transform.position = Vector3.zero;
			gameobjectList.Add (go);
		} else {
			GameObject go = selectObjList [0] as GameObject;// as GameObject;
			GameObject childGo = GameObject.CreatePrimitive(primitiveType);
			childGo.transform.parent = go.transform;
			childGo.name = objName;
			childGo.transform.position = Vector3.zero;
			gameobjectList.Add(childGo);
		}
		return gameobjectList.ToArray ();
	}

	public static GameObject[] InstaniceEmptyGameobject(string objName){
		List<GameObject> gameobjectList = new List<GameObject> ();
		UnityEngine.Object[] selectObjList = Selection.GetFiltered(typeof(GameObject), SelectionMode.Unfiltered);
		if (selectObjList.Length > 1) {
			EditorUtility.DisplayDialog("", "Select To", "OK");
			return null;
		} else if (selectObjList.Length == 0) {
			GameObject go = new GameObject ();
			go.name = objName;
			go.transform.position = Vector3.zero;
			gameobjectList.Add (go);
		} else {
			GameObject go = selectObjList [0] as GameObject;// as GameObject;
			GameObject childGo = new GameObject();
			childGo.transform.parent = go.transform;
			childGo.name = objName;
			childGo.transform.position = Vector3.zero;
			gameobjectList.Add(childGo);
		}
		return gameobjectList.ToArray ();
	}

	public static void AddComponentToGameObjectArray<T>(GameObject[] goArray) where T: UnityEngine.Component{
		if (goArray == null)
			return;
		foreach (var go in goArray) {
			go.AddComponent<T> ();
		}
	}

	[MenuItem("GameObject/Create Other/Dummy")]
	static void CreateEmptyObject(){
		GameObject[] goArray = InstaniceEmptyGameobject ("empty_dummy");
		Selection.activeGameObject = goArray[0];
	}

	[MenuItem("GameObject/Create Other/Billboard(Dummy)")]
	static void CreateEffectObject(){
		GameObject[] goArray = InstaniceEmptyGameobject ("Billboard_dummy");
		AddComponentToGameObjectArray<RenderEffect> (goArray);
        Selection.activeGameObject = goArray[0];
	}
	[MenuItem("GameObject/Create Other/Effect_Quad")]
	static void CreateEffectObjectQuad(){
		GameObject[] goArray = InstaniceEmptyPrimitiveType ("EF_Quad", PrimitiveType.Quad);
		AddComponentToGameObjectArray<RenderEffect> (goArray);
        Selection.activeGameObject = goArray[0];
	}
	[MenuItem("GameObject/Create Other/TrailRender")]
	static void CreateEffectObjectTrail(){
		GameObject[] goArray = InstaniceEmptyGameobject ("EF_Trail");
		AddComponentToGameObjectArray<TrailRenderer> (goArray);
		AddComponentToGameObjectArray<RenderEffect> (goArray);
        Selection.activeGameObject = goArray[0];
	}
	[MenuItem("GameObject/Create Other/LineRender")]
	static void CreateEffectObjectLine(){
		GameObject[] goArray = InstaniceEmptyGameobject ("EF_Laser");
		AddComponentToGameObjectArray<LineRenderer> (goArray);
		AddComponentToGameObjectArray<RenderEffect> (goArray);
        Selection.activeGameObject = goArray[0];
	}
	[MenuItem("GameObject/Create Other/UV_Scorll")]
	static void CreateEffectObjectParticle(){
		GameObject[] goArray = InstaniceEmptyGameobject ("Particle_UV");
		AddComponentToGameObjectArray<ParticleSystem> (goArray);
		AddComponentToGameObjectArray<RenderEffect> (goArray);
        Selection.activeGameObject = goArray[0];
	}

}
