using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text.RegularExpressions;

/// <summary>
/// Effect controller inspector.
/// </summary>
[CustomEditor(typeof(EffectController))]
public class EffectControllerInspector : Editor
{
	private string[] m_LayerName;
	private int[] m_LayerID;

	void OnEnable()
	{
		m_LayerName = XUIUtils.GetSortingLayerNames ();
		m_LayerID = XUIUtils.GetSortingLayerUniqueIDs ();
	}

	public override void OnInspectorGUI()
	{
		bool bShowAll = false;
		bool bHideAll = false;
		
		EffectController effectCtrl = target as EffectController;

		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUI.BeginDisabledGroup (effectCtrl.m_bLockNums);
		effectCtrl.m_nNumOfEffects = EditorGUILayout.IntField("Effect Count", effectCtrl.m_nNumOfEffects);
		EditorGUI.EndDisabledGroup();
		effectCtrl.m_bLockNums = EditorGUILayout.Toggle (effectCtrl.m_bLockNums);
		if (GUILayout.Button ("One Click Expansion"))
			bShowAll = true;
		else
			bShowAll = false;

		if (GUILayout.Button ("One Click Close"))
			bHideAll = true;
		else
			bHideAll = false;

		EditorGUILayout.EndHorizontal();

		int nCnt = 0;
		for (; nCnt < effectCtrl.m_nNumOfEffects; nCnt++) {
			if (nCnt >= effectCtrl.m_kEffectGenList.Count) {
				effectCtrl.m_kEffectGenList.Add (new EffectData ());
			}

			EffectData effectData = effectCtrl.m_kEffectGenList [nCnt];
			if (effectData == null)
				continue;
			if (bShowAll)
				effectData.m_bFoldoutOpen = true;
			if (bHideAll)
				effectData.m_bFoldoutOpen = false;
			
			effectData.m_bFoldoutOpen = EditorGUILayout.Foldout (effectData.m_bFoldoutOpen, ("Effect " + nCnt + " Setting"));
			if (effectData.m_bFoldoutOpen) {
				effectData.m_fTimeSec = EditorGUILayout.FloatField ("Shot Time", effectData.m_fTimeSec);
				effectData.m_goEffect = EditorGUILayout.ObjectField ("Obj", effectData.m_goEffect, typeof(GameObject), true) as GameObject;

				EditorGUI.indentLevel++;
				/// Transform panel.
				effectData.m_bTransformFoldout = EditorGUILayout.Foldout (effectData.m_bTransformFoldout, "Transform");
				if (effectData.m_bTransformFoldout) {
					EditorGUI.indentLevel++;
					EditorGUI.BeginChangeCheck ();
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button ("P", GUILayout.Width(25), GUILayout.ExpandWidth(false)))
						effectData.m_goPos = new Vector3 (0, 0, 0);
					effectData.m_goPos = EditorGUILayout.Vector3Field ("", effectData.m_goPos);
					GUILayout.EndHorizontal ();
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button ("R", GUILayout.Width(25), GUILayout.ExpandWidth(false)))
						effectData.m_goRotation = new Vector3 (0, 0, 0);
					effectData.m_goRotation = EditorGUILayout.Vector3Field ("", effectData.m_goRotation);
					GUILayout.EndHorizontal ();
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button ("S", GUILayout.Width(25), GUILayout.ExpandWidth(false)))
						effectData.m_goScale = new Vector3 (0, 0, 0);
					effectData.m_goScale = EditorGUILayout.Vector3Field ("", effectData.m_goScale);
					GUILayout.EndHorizontal ();
					if (EditorGUI.EndChangeCheck ()) {
						effectCtrl.UpdateEffectTransformByIndex (nCnt);
					}
					EditorGUI.indentLevel--;
				}

				ParticleSystem particleSystem = effectCtrl.CheckHasParticleSystem (nCnt);
				RenderEffect renderEffect = effectCtrl.CheckHasRenderEffectScript (nCnt);
				if (particleSystem == null) {
					effectData.m_bSortingFoldout = EditorGUILayout.Foldout (effectData.m_bSortingFoldout, "Sorting Layer");
					/// Sorting panel.
					if (effectData.m_bSortingFoldout) {
						EditorGUI.indentLevel++;
						EditorGUI.BeginChangeCheck ();
						EditorGUILayout.BeginHorizontal ();
						EditorGUILayout.LabelField ("Sorting Layer");
						effectData.m_SortingLayerID = EditorGUILayout.IntPopup (effectData.m_SortingLayerID, m_LayerName, m_LayerID);
						EditorGUILayout.EndHorizontal ();

						EditorGUILayout.BeginHorizontal ();
						EditorGUILayout.LabelField ("Sorting Order");
						effectData.m_SortingOrder = EditorGUILayout.IntField (effectData.m_SortingOrder);
						EditorGUILayout.EndHorizontal ();
						if (EditorGUI.EndChangeCheck ()) {
							if (renderEffect != null) {
								renderEffect.m_SortingLayerID = effectData.m_SortingLayerID;
								renderEffect.m_SortingOrder = effectData.m_SortingOrder;
								renderEffect.m_EnableSetSortLayer = true;
								renderEffect.UpdateRenderLayer ();
							} else {
								effectCtrl.UPdateRenderLayerByIndex (nCnt);
							}
						}
						EditorGUI.indentLevel--;
					}
				}
				EditorGUI.indentLevel--;
			}

			if (nCnt != effectCtrl.m_nNumOfEffects - 1) {
				EditorGUILayout.LabelField ("", GUILayout.Height (2));
				GUILayout.Box ("", GUILayout.ExpandWidth (true), GUILayout.Height (1));
				EditorGUILayout.LabelField ("", GUILayout.Height (2));
			}
		}

		for (; nCnt < effectCtrl.m_kEffectGenList.Count; nCnt++) {
			effectCtrl.m_kEffectGenList.RemoveAt (nCnt);
		}

		EditorGUILayout.EndVertical ();
	}
}
