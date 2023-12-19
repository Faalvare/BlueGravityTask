using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private GameObject hoveredObject;
    [HideInInspector] public Transform parentAfterDrag;
    public ItemSO item;
    private Image image;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        image = GetComponent<Image>();
        image.sprite = item.Icon;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        gameObject.transform.SetAsLastSibling();
        transform.SetParent(transform.root);
        parentAfterDrag.GetComponent<InventorySlot>().inventoryItem = null;
        GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        gameObject.transform.position = InputManager.Instance.inputActions.UI.Point.ReadValue<Vector2>();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        hoveredObject = eventData.pointerCurrentRaycast.gameObject;
        if (hoveredObject != null)
        {
            Debug.Log("Dropped on: " + hoveredObject.name);
        }
        transform.SetParent(parentAfterDrag);
        GetComponent<Image>().raycastTarget = true;
    }
}
