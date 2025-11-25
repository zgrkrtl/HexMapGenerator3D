using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HexCellClickTester : MonoBehaviour
{
    public Camera cam;
    public HexGridManager grid;

    [Header("Highlight Settings")]
    public Color clickedColor = Color.red;
    public float clickedDuration = 0.15f;
    public Color neighborColor = new Color(1f, 1f, 0f, 0.5f);
    public float neighborDuration = 0.10f;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                HexCell cell = hit.collider.GetComponent<HexCell>();
                if (cell == null) return;

                Debug.Log($"Clicked: {cell.coord}");
                StartCoroutine(HighlightCellAndNeighbors(cell));
            }
        }
    }

    private IEnumerator HighlightCellAndNeighbors(HexCell cell)
    {
        Color[] originalCellColors = new Color[cell.renderers?.Length ?? 0];
        for (int i = 0; i < originalCellColors.Length; i++)
        {
            if (cell.renderers[i] != null && cell.renderers[i].material.HasProperty("_Color"))
                originalCellColors[i] = cell.renderers[i].material.color;
        }

        if (cell.renderers != null)
        {
            foreach (var rend in cell.renderers)
            {
                if (rend == null || !rend.material.HasProperty("_Color")) continue;
                rend.material.color = clickedColor;
            }

            yield return new WaitForSeconds(clickedDuration);

            for (int i = 0; i < originalCellColors.Length; i++)
            {
                if (cell.renderers[i] != null && cell.renderers[i].material.HasProperty("_Color"))
                    cell.renderers[i].material.color = originalCellColors[i];
            }
        }

        // --- Step 2: Highlight neighbors sequentially ---
        for (int i = 0; i < 6; i++)
        {
            if (grid == null) continue;

            HexCoord n = cell.coord.Neighbor(i);

            if (!grid.cellLookup.TryGetValue(n, out HexCell neigh) || neigh == null)
            {
                Debug.Log($"Neighbor {i}: NONE");
                yield return new WaitForSeconds(neighborDuration);
                continue;
            }

            Debug.Log($"Neighbor {i}: {neigh.coord}");

            // Store original colors
            Color[] originalNeighborColors = new Color[neigh.renderers?.Length ?? 0];
            for (int j = 0; j < originalNeighborColors.Length; j++)
            {
                if (neigh.renderers[j] != null && neigh.renderers[j].material.HasProperty("_Color"))
                    originalNeighborColors[j] = neigh.renderers[j].material.color;
            }

            // Apply highlight
            if (neigh.renderers != null)
            {
                foreach (var rend in neigh.renderers)
                {
                    if (rend == null || !rend.material.HasProperty("_Color")) continue;
                    rend.material.color = neighborColor;
                }
            }

            // Wait while highlighted
            yield return new WaitForSeconds(neighborDuration);

            // Restore original colors
            if (neigh.renderers != null)
            {
                for (int j = 0; j < neigh.renderers.Length; j++)
                {
                    if (neigh.renderers[j] != null && neigh.renderers[j].material.HasProperty("_Color"))
                        neigh.renderers[j].material.color = originalNeighborColors[j];
                }
            }
        }
    }
}
