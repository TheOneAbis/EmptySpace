using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public List<List<GameObject>> inventory;
    // Start is called before the first frame update
    void Start()
    {
        // Init the inventory
        inventory = new List<List<GameObject>>();
        for (int i = 0; i < 4; i++)
            inventory.Add(new List<GameObject>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Add a new game object to the player's inventory
    public void AddToInventory(GameObject obj)
    {
        void AddToSlot(GameObject obj, List<GameObject> slot) 
        { 
            slot.Add(obj);
            obj.SetActive(false);
        }

        // Look for a slot with objects matching this type to add it to that pool
        foreach (List<GameObject> slot in inventory)
        {
            if (slot.Count > 0 && slot[0].GetType() == obj.GetType())
            {
                AddToSlot(obj, slot);
                return;
            }
        }

        // Did not find any existing slot with this gameobject type; find an empty slot to put it in
        foreach (List<GameObject> slot in inventory)
        {
            if (slot.Count == 0)
            {
                AddToSlot(obj, slot);
                return;
            }
        }

        // If the function reaches this point, player's inventory is too full for this item
        Debug.Log($"Inventory full; cannot add {obj}");
    }
}
