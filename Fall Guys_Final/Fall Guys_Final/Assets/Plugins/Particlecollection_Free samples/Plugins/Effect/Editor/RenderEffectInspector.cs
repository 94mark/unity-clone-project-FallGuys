using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using UnityEditorInternal;
using System.Reflection;
using System;

[CustomEditor(typeof(RenderEffect))]
public class RenderEffectInspector : Editor
{
    public bool m_EnableSortLayer = false;
	private string[] m_LayerName;
	private int[] m_LayerID;

    void OnEnable()
    {
        m_LayerName = GetSortingLayerNames();
        m_LayerID = GetSortingLayerUniqueIDs();
    }

    public string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    // Get the unique sorting layer IDs -- tossed this in for good measure
    public int[] GetSortingLayerUniqueIDs()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
        return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
    }

    public override void OnInspectorGUI()
    {
        RenderEffect renderEffect = target as RenderEffect;
        ParticleSystem particleSystem = renderEffect.gameObject.GetComponent<ParticleSystem>();
        EditorGUILayout.BeginVertical();

        
        if (particleSystem == null)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Open BillBoardOption");
            renderEffect.m_EnableBillBoard = EditorGUILayout.Toggle(renderEffect.m_EnableBillBoard);
            EditorGUILayout.EndHorizontal();

            if (renderEffect.m_EnableBillBoard)
            {

                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("BillBoard Type");
                renderEffect.m_BillBoardType = (RenderBillBoardType)EditorGUILayout.EnumPopup(renderEffect.m_BillBoardType);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
        }

        if(particleSystem == null)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Open Sort Layer Option");
            renderEffect.m_EnableSetSortLayer = EditorGUILayout.Toggle(renderEffect.m_EnableSetSortLayer);
            EditorGUILayout.EndHorizontal();
            if (renderEffect.m_EnableSetSortLayer)
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Sorting Layer");
                renderEffect.m_SortingLayerID = EditorGUILayout.IntPopup(renderEffect.m_SortingLayerID, m_LayerName, m_LayerID);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Sorting Order");
                renderEffect.m_SortingOrder = EditorGUILayout.IntField(renderEffect.m_SortingOrder);
                EditorGUILayout.EndHorizontal();

                if (EditorGUI.EndChangeCheck())
                {
                    renderEffect.UpdateRenderLayer();
                }
                EditorGUI.indentLevel--;
            }
        }
		Renderer render = renderEffect.gameObject.GetComponent<Renderer>();
		if (render != null) {
			if (GUILayout.Button ("Refresh Material")) {
				renderEffect.RefreshMaterial ();
			}
			EditorGUILayout.LabelField ("Materials");
		}
        EditorGUI.indentLevel++;
        int index = 0;
        foreach(MaterialEffect matEffect in renderEffect.m_MaterialEffects)
        {
            string strIndex = "Element:" + index + "    ";
            if (matEffect.m_EffectMaterial == null)
            {
                GUILayout.Button(strIndex + "Material Not Assign");
                index++;
                continue;
            }
            else
            {
                if(GUILayout.Button(strIndex + matEffect.m_EffectMaterial.name))
                {
                    matEffect.m_EditorExtend = !matEffect.m_EditorExtend;
                }
                index++;
                if (matEffect.m_EditorExtend)
                {

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Main Texture WrapMode");
                    matEffect.m_MainTexWrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup(matEffect.m_MainTexWrapMode);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Cutoff Texture WrapMode");
                    matEffect.m_MaskTexWrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup(matEffect.m_MaskTexWrapMode);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        EditorGUI.indentLevel--;
        if (render != null && particleSystem == null)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Switch Render Type");
            if (render.GetType() != typeof(LineRenderer))
            {
                if (GUILayout.Button("Switch To Line Render"))
                {
                    LineRenderer lineRender = renderEffect.gameObject.AddComponent<LineRenderer>();
                    renderEffect.m_Render = lineRender;
                    lineRender.sharedMaterials = render.sharedMaterials;
                    UnityEngine.Object.DestroyImmediate(render);
                    MeshFilter meshFilter = renderEffect.gameObject.GetComponent<MeshFilter>();
                    if (meshFilter != null)
                    {
                        UnityEngine.Object.DestroyImmediate(meshFilter);
                    }
                    Collider meshCollider = renderEffect.gameObject.GetComponent<Collider>();
                    if (meshCollider != null)
                    {
                        UnityEngine.Object.DestroyImmediate(meshCollider);
                    }
                    EditorGUIUtility.ExitGUI();
                }
            }
            if (render.GetType() != typeof(MeshRenderer))
            {
                if (GUILayout.Button("Switch To Mesh Render"))
                {
                    MeshRenderer lineRender = renderEffect.gameObject.AddComponent<MeshRenderer>();
                    lineRender.sharedMaterials = render.sharedMaterials;
                    renderEffect.m_Render = lineRender;
                    UnityEngine.Object.DestroyImmediate(render);
                    MeshFilter meshFilter = renderEffect.gameObject.GetComponent<MeshFilter>();
                    if (meshFilter == null)
                    {
                        renderEffect.gameObject.AddComponent<MeshFilter>();
                    }
                    Collider Collider = renderEffect.gameObject.GetComponent<Collider>();
                    if (Collider != null)
                    {
                        UnityEngine.Object.DestroyImmediate(Collider);
                    }
                    MeshCollider meshCollider = renderEffect.gameObject.GetComponent<MeshCollider>();
                    if (meshCollider == null)
                    {
                        renderEffect.gameObject.AddComponent<MeshCollider>();
                    }
                    EditorGUIUtility.ExitGUI();
                }
            }
            if (render.GetType() != typeof(TrailRenderer))
            {
                if (GUILayout.Button("Switch To Trail Render"))
                {
                    TrailRenderer lineRender = renderEffect.gameObject.AddComponent<TrailRenderer>();
                    lineRender.sharedMaterials = render.sharedMaterials;
                    renderEffect.m_Render = lineRender;
                    UnityEngine.Object.DestroyImmediate(render);
                    MeshFilter meshFilter = renderEffect.gameObject.GetComponent<MeshFilter>();
                    if (meshFilter != null)
                    {
                        UnityEngine.Object.DestroyImmediate(meshFilter);
                    }
                    Collider Collider = renderEffect.gameObject.GetComponent<Collider>();
                    if (Collider != null)
                    {
                        UnityEngine.Object.DestroyImmediate(Collider);
                    }
                    EditorGUIUtility.ExitGUI();
                }
            }
			if (render.GetType () == typeof(TrailRenderer)) {
				if (GUILayout.Button ("Clear Trail")) {
					TrailRenderer trailRender =  render.GetComponent<TrailRenderer> ();
					trailRender.Clear();
				}
			}
            EditorGUI.indentLevel--;
        }

        
        EditorGUILayout.EndVertical();
    }
    
}
