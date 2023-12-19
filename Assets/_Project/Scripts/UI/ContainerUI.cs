using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    [SerializeField] private InventorySlot[] inventorySlots;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject inventoryItemPrefab;
    List<ItemSO> inventoryItems;
    private void OnEnable()
    {
        List<ItemSO> inventoryItems = inventory.GetInventoryItems();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i] != null)
            {
                var gObj = Instantiate(inventoryItemPrefab, inventorySlots[i].transform);
                InventoryItem inventoryItem = gObj.GetComponent<InventoryItem>();
                inventoryItem.Init();
            }else
            {
                if (inventorySlots[i].inventoryItem != null)
                {
                    Destroy(inventorySlots[i].inventoryItem.gameObject);
                }
            }
        }
    }

    private void OnDisable()
    {
        if (inventoryItems != null)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                inventoryItems[i] = inventorySlots[i].inventoryItem.item;
            }
            inventory.SetInventoryItems(inventoryItems);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
