using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class UIDraggableToTilemap : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Tilemap targetTilemap; // Assign your Tilemap in the Inspector
    [SerializeField] public GameObject ThingToSpawn;

    private RectTransform rectTransform;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>(); // Get the parent Canvas
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Optional: Change appearance of the UI element during drag
        // For example, make it slightly transparent or change its color.
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the UI element with the mouse
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Convert UI screen position to world position
        Vector2 screenPoint = eventData.position;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);

        // Convert world position to Tilemap cell position
        Vector3Int cellPosition = targetTilemap.WorldToCell(worldPoint);

        // Place the tile if a valid Tilemap and Tile are assigned
        if (targetTilemap != null && ThingToSpawn != null)
        {
            Instantiate(ThingToSpawn, worldPoint, Quaternion.identity);
        }

        // Optional: Reset UI element position or destroy it
        // For example, snap it back to its original position or remove it from the UI.
    }
}