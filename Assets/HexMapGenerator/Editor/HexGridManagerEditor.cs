using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexGridManager))]
public class HexGridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HexGridManager mgr = (HexGridManager)target;

        if (mgr.hexPrefab == null)
        {
            EditorGUILayout.HelpBox("Assign a Hex Prefab before generating the grid.", MessageType.Warning);
            return;
        }

        if (mgr.hexPrefab.GetComponent<HexCell>() == null)
        {
            EditorGUILayout.HelpBox("Prefab must contain a HexCell component.", MessageType.Error);
            return;
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Generate Grid"))
        {
            mgr.GenerateGrid();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            mgr.ClearGrid();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Re-detect Prefab Radius"))
        {
            float radius = (float)typeof(HexGridManager)
                .GetMethod("GetMeshRadius", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(mgr, new object[] { mgr.hexPrefab });

            mgr.cellRadius = radius;
            EditorUtility.SetDirty(mgr);
        }

        if (GUI.changed)
            EditorUtility.SetDirty(mgr);
    }
}
