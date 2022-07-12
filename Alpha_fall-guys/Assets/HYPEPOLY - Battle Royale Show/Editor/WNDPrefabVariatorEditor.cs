using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
[CustomEditor(typeof(WNDPrefabVariator))]
public class WNDPrefabVariatorEditor : Editor
{
    private GUIStyle currentStyle = null;
    public override void OnInspectorGUI()
    {
        WNDPrefabVariator variator = (WNDPrefabVariator)target;

        if (variator.headerLogo != null)
        {
            if (currentStyle == null)
            {
                currentStyle = new GUIStyle(GUI.skin.box);
                currentStyle.normal.background = MakeTex(2, 2);
            }

            GUILayout.Box(variator.headerLogo, currentStyle, GUILayout.Width(Screen.width));
        }

            GUILayout.Box("RANDOMIZE", GUILayout.Width(Screen.width));
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Random Prefab"))
            {
                UnpackForFX(variator);
                variator.RandomPrefab();
            }
            EditorGUILayout.EndHorizontal();

        if (variator.typesSelector)
        {
            GUILayout.Box("TYPE", GUILayout.Width(Screen.width));
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("<"))
            {
                UnpackForFX(variator);
                variator.ChooseType(false);
            }
            if (GUILayout.Button("Random"))
            {
                UnpackForFX(variator);
                variator.RandomType();
            }
            if (GUILayout.Button(">"))
            {
                UnpackForFX(variator);
                variator.ChooseType(true);
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Box("COLOR", GUILayout.Width(Screen.width));
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("<"))
        {
            UnpackForFX(variator);
            variator.ChooseColor(false);
        }
        if (GUILayout.Button("Random"))
        {
            UnpackForFX(variator);
            variator.RandomColor();
        }
        if (GUILayout.Button(">"))
        {
            UnpackForFX(variator);
            variator.ChooseColor(true);
        }
        EditorGUILayout.EndHorizontal();

        if (variator.hasStyle)
        {
            GUILayout.Box("STYLE", GUILayout.Width(Screen.width));
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("<"))
            {
                UnpackForFX(variator);
                variator.ChooseStyle(false);
            }
            if (GUILayout.Button("Random"))
            {
                UnpackForFX(variator);
                variator.RandomStyle();
            }
            if (GUILayout.Button(">"))
            {
                UnpackForFX(variator);
                variator.ChooseStyle(true);
            }
            EditorGUILayout.EndHorizontal();
        }

        if(variator.isDynamicFence)
        {
            GUILayout.Box("PILLARS", GUILayout.Width(Screen.width));
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("<"))
            {
                UnpackForFX(variator);
                variator.ChoosePillars(false);
            }
            if (GUILayout.Button("Random"))
            {
                UnpackForFX(variator);
                variator.RandomPillars();
            }
            if (GUILayout.Button(">"))
            {
                UnpackForFX(variator);
                variator.ChoosePillars(true);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Box("FENCE LENGTH", GUILayout.Width(Screen.width));
            EditorGUILayout.BeginHorizontal();

            GUILayout.Box(variator.fenceLength.ToString(), GUILayout.Width(40f));

            float curLength = variator.fenceLength;

            variator.fenceLength = GUILayout.HorizontalSlider(variator.fenceLength, 2f, 100f, GUILayout.Width(Screen.width - 70f), GUILayout.Height(16f));

            variator.fenceLength = Mathf.CeilToInt(variator.fenceLength);

            EditorGUILayout.EndHorizontal();


            GUILayout.Box("PILLARS FREQUENCY", GUILayout.Width(Screen.width));
            EditorGUILayout.BeginHorizontal();

            GUILayout.Box(variator.fencePillarsFrequency.ToString(), GUILayout.Width(40f));

            float curFrequency = variator.fencePillarsFrequency;

            variator.fencePillarsFrequency = GUILayout.HorizontalSlider(variator.fencePillarsFrequency, -1f, 5f, GUILayout.Width(Screen.width - 70f), GUILayout.Height(16f));

            variator.fencePillarsFrequency = Mathf.CeilToInt(variator.fencePillarsFrequency);

            EditorGUILayout.EndHorizontal();

            if (variator.fenceLength != curLength || variator.fencePillarsFrequency != curFrequency)
            {
                if (PrefabUtility.IsPartOfRegularPrefab(variator.gameObject))
                {
                    PrefabUtility.UnpackPrefabInstance(variator.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                    Debug.Log("[WND PREFAB VARIATOR] " + variator.gameObject.name + " was unpacked to update sizes and pillars.");
                }

                variator.RefreshPillars();
            }
        }
    }
    void UnpackForFX(WNDPrefabVariator variator)
    {
        if (variator.myPs != null)
        {
            if (PrefabUtility.IsPartOfRegularPrefab(variator.gameObject))
            {
                PrefabUtility.UnpackPrefabInstance(variator.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                Debug.Log("[WND PREFAB VARIATOR] " + variator.gameObject.name + " was unpacked to update sizes and pillars.");
            }
        }
    }
    private Texture2D MakeTex(int width, int height)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = Color.black;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}