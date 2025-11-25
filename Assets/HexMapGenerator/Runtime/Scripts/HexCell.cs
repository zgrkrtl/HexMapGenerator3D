using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class HexCell : MonoBehaviour
{
    [Header("Coordinates")]
    public int q;
    public int r;
    public HexCoord coord;
    public float cellRadius;

    [Header("State")]
    public bool blocked = false;
    public bool isOccupied = false;

    [Header("Rendering (Optional)")]
    public Renderer[] renderers;

    public HexGridManager grid;

    public TextMeshPro coordText;

#if UNITY_EDITOR
    private Vector3 lastPosition;
    private double lastSnapTime = 0;
    private const double snapInterval = 0.5;
#endif

    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.update += EditorUpdate;
        lastPosition = transform.position;
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= EditorUpdate;
#endif
    }

#if UNITY_EDITOR
    private void EditorUpdate()
    {
        if (!Application.isPlaying && grid != null)
        {
            if (EditorApplication.timeSinceStartup - lastSnapTime < snapInterval)
                return;

            if (transform.position != lastPosition)
            {
                SnapToGrid();
                lastSnapTime = EditorApplication.timeSinceStartup;
            }
        }
    }
#endif

    private void SnapToGrid()
    {
        if (grid == null) return;
        transform.position = coord.ToWorld(grid.cellRadius, Vector3.zero);
#if UNITY_EDITOR
        lastPosition = transform.position;
#endif
    }

    public void Init(HexGridManager gridManager)
    {
        CacheComponents();
        coord = new HexCoord(q, r);
        if (coordText != null)
            coordText.text = $"{coord.q},{coord.r}";
        grid = gridManager;
        SnapToGrid();
    }

    private void CacheComponents()
    {
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<Renderer>();
    }

    public List<HexCell> GetNeighbors()
    {
        var list = new List<HexCell>();
        if (grid == null) return list;

        for (int i = 0; i < 6; i++)
        {
            HexCoord neighborCoord = coord.Neighbor(i);
            HexCell neighbor = grid.GetCell(neighborCoord);
            if (neighbor != null) list.Add(neighbor);
        }

        return list;
    }

    public override string ToString() => $"HexCell {coord.q},{coord.r}";
}
