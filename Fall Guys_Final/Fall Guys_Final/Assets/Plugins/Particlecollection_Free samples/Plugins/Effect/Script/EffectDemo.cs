using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class EffectDemo : MonoBehaviour {
	public const string EFFECT_ASSET_PATH = "Assets/Prefab/";
	public List<GameObject> m_EffectPrefabList = new List<GameObject> ();
	public bool m_LookAtEffect = true;
	private GameObject m_NowShowEffect = null;
	private int m_NowIndex = 0;
	private string m_NowEffectName;
	// Use this for initialization
	void Awake () {
        #if (UNITY_EDITOR_WIN && !UNITY_WEBPLAYER)
		    m_EffectPrefabList.Clear();
		    string[] aPrefabFiles = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);
            foreach (string prefabFile in aPrefabFiles)
		    {
			    string assetPath = "Assets" + prefabFile.Replace(Application.dataPath, "").Replace('\\', '/');
                if(assetPath.Contains("_noshow"))
                {
                    continue;
                }
			    GameObject sourcePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
			    m_EffectPrefabList.Add (sourcePrefab);
		    }
        #endif
        if (Application.isPlaying == false)
			return;
		m_NowIndex = 1;
		GenPrevEffect ();
	}
	
	void OnDestroy(){
		Object.DestroyImmediate (m_NowShowEffect);	
	}
	
	void LateUpdate(){
		if (Application.isPlaying == false)
			return;
		if (m_LookAtEffect && m_NowShowEffect) {
			transform.LookAt (m_NowShowEffect.transform.position);			
		}
	}
	
	// Update is called once per frame
	void OnGUI() {
		if (Application.isPlaying == false)
			return;
		if (GUI.Button (new Rect (0, 25, 80, 50), "Prev")) {
			GenPrevEffect ();
		}
		if (GUI.Button (new Rect (90, 25, 80, 50), "Next")) {
			GenNextEffect ();
		}
		GUI.Label (new Rect (5, 0, 350, 50), m_NowEffectName);
	}
	
	void GenPrevEffect(){
		m_NowIndex--;
		if (m_NowIndex < 0) {
			m_NowIndex = 0;
			return;	
		}
		if (m_NowShowEffect != null) {
			Object.Destroy (m_NowShowEffect);	
		}
		m_NowShowEffect =  Instantiate(m_EffectPrefabList [m_NowIndex]);
		m_NowEffectName = m_NowShowEffect.name;
	}
	
	void GenNextEffect(){
		m_NowIndex++;
		if (m_NowIndex >= m_EffectPrefabList.Count) {
			m_NowIndex = m_EffectPrefabList.Count - 1;	
			return;
		}
		if (m_NowShowEffect != null) {
			Object.Destroy (m_NowShowEffect);	
		}
		m_NowShowEffect =  Instantiate(m_EffectPrefabList [m_NowIndex]);		
		m_NowEffectName = m_NowShowEffect.name;
	}
}
