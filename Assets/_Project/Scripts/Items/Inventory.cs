using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]private List<ItemSO> items = new List<ItemSO>();
    [SerializeField] private float Gold = 3000;

    public bool AddItem(ItemSO item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                return true;
            }
        }
        Debug.LogWarning("The container is full!");
        return false;
    }

    public List<ItemSO> GetInventoryItems()
    {
        return items;
    }

    public void SetInventoryItems(List<ItemSO> items)
    {
        this.items = items;
    }

    public void EquipItem(EquipableSO equipableItem)
    {
        equipableItem.Equip();
    }
}
