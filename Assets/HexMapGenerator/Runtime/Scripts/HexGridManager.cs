#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using System.Collections.Generic;
using UnityEngine;


public enum HexMapShape
{
    Hexagon,
    Rectangle,
}

[ExecuteAlways]
public class HexGridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int radius = 5;
    public float cellRadius = 0.5f;
    public Vector3 origin = Vector3.zero;

    [Header("Prefab")]
    public GameObject hexPrefab;

    [Header("Hierarchy")]
    public string parentName = "HexGrid";

    public Dictionary<HexCoord, HexCell> cellLookup = new Dictionary<HexCoord, HexCell>();
    public List<HexCell> cellsList = new List<HexCell>();

    private void Awake()
    {
        BuildLookup();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            HexCell[] current = GetComponentsInChildren<HexCell>();
            if (current.Length != cellsList.Count)
            {
                BuildLookup();
            }
        }
#endif
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (hexPrefab != null)
        {
            float detected = GetMeshRadius(hexPrefab);
            if (detected > 0f)
                cellRadius = detected;
        }
    }
#endif

    public void BuildLookup()
    {
        cellLookup.Clear();

        HexCell[] allCells = GetComponentsInChildren<HexCell>();

        foreach (var cell in allCells)
        {
            if (!cellLookup.ContainsKey(cell.coord))
                cellLookup[cell.coord] = cell;
        }

        cellsList = new List<HexCell>(allCells);
    }

    public void GenerateGrid()
    {
        ClearGrid();

        Transform parent = transform.Find(parentName);
        if (parent == null)
        {
            GameObject go = new GameObject(parentName);
            go.transform.SetParent(transform, false);
            parent = go.transform;
        }

        cellLookup.Clear();
        cellsList.Clear();

        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);

            for (int r = r1; r <= r2; r++)
            {
                HexCoord coord = new HexCoord(q, r);
                Vector3 pos = coord.ToWorld(cellRadius, origin);

#if UNITY_EDITOR
                GameObject instance = !Application.isPlaying
                    ? (GameObject)PrefabUtility.InstantiatePrefab(hexPrefab, parent)
                    : Instantiate(hexPrefab, parent);

                instance.transform.localPosition = pos;
#else
                GameObject instance = Instantiate(hexPrefab, pos, Quaternion.identity, parent);
#endif

                HexCell cell = instance.GetComponent<HexCell>();

                cell.q = q;
                cell.r = r;
                cell.coord = coord;
                cell.Init(this);

                instance.name = $"Hex_{q}_{r}";

                cellLookup[coord] = cell;
                cellsList.Add(cell);
            }
        }

#if UNITY_EDITOR
        if (!Application.isPlaying)
            EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
    }

    public void ClearGrid()
    {
        cellLookup.Clear();
        cellsList.Clear();

        Transform parent = transform.Find(parentName);
        if (parent != null)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(parent.gameObject);
            else
#endif
                Destroy(parent.gameObject);
        }
    }

    private float GetMeshRadius(GameObject prefab)
    {
        var mf = prefab.GetComponentInChildren<MeshFilter>();
        if (mf == null) return cellRadius;

        Bounds b = mf.sharedMesh.bounds;
        return Mathf.Max(b.extents.x, b.extents.z);
    }

    public HexCell GetCell(HexCoord coord)
    {
        cellLookup.TryGetValue(coord, out var cell);
        return cell;
    }
}
