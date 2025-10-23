using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class DraggableSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("Stats")]
    [SerializeField] private bool isLocked;
    [SerializeField] public int price;
    [SerializeField] public GameObject Type;

    [Header("Sets where not to drag")]
    [SerializeField] GameObject leftPanel;
    [SerializeField] GameObject rightPanel;
    [SerializeField] GameObject topPanel;

    [Header("Sets where it can be dropped")]
    [SerializeField] Tilemap theLevel;

    private Vector2 DropPoint;
    private Transform parentAfterDrag;

    public Currencies theCurrencies;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end drag");
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector2.zero;
    }

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform AvoidOne = leftPanel.transform as RectTransform ;
        RectTransform AvoidTwo = rightPanel.transform as RectTransform;
        RectTransform AvoidThree = topPanel.transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(AvoidOne, Input.mousePosition)  && !RectTransformUtility.RectangleContainsScreenPoint(AvoidTwo, Input.mousePosition) && !RectTransformUtility.RectangleContainsScreenPoint(AvoidThree, Input.mousePosition))
        {
            Vector2 screenPoint = eventData.position;
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
            Vector3Int cellPosition = theLevel.WorldToCell(worldPoint);

            if(theCurrencies.SpendMoney(price))
            {
                Instantiate(Type, worldPoint, Quaternion.identity);
            }
            else
            {
                Debug.Log("Not enough money");
            }
        }
        else
        {
            Debug.Log("Cant drop here");
        }
    }
}
