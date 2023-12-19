using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour,IDropHandler
{
    public InventoryItem inventoryItem;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        inventoryItem = dropped.GetComponent<InventoryItem>();
        inventoryItem.parentAfterDrag = transform;
    }
}
