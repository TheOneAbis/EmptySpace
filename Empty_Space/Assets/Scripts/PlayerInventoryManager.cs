using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public Dictionary<string, Stack<GameObject>> inventory;

    // Start is called before the first frame update
    void Start()
    {
        // Init the inventory
        inventory = new Dictionary<string, Stack<GameObject>>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Add a new game object to the player's inventory
    public void AddToInventory(GameObject obj)
    {
        if (inventory.ContainsKey(obj.tag))
        {
            inventory[obj.tag].Push(obj);
            obj.SetActive(false);
            Debug.Log(inventory[obj.tag].Count + " items in slot of tag " + inventory[obj.tag]);
        }
        else
        {
            inventory.Add(obj.tag, new Stack<GameObject>());
            Debug.Log($"New object tag added to inventory: {obj}");
        }
    }

    // Remove a specified amount of objects from the inventory, if there are enough of them. If not, does nothing.
    public void Remove(string objectTag, int amountToRemove)
    {
        if (GetAmount(objectTag) >= amountToRemove)
        {
            for (int i = 0; i < inventory[objectTag].Count; i++)
                inventory[objectTag].Pop();

            if (GetAmount(objectTag) == 0) inventory.Remove(objectTag);
        }
    }

    // Check to see if player's inventory contains items with the provided tag
    public bool Contains(string objectTag)
    {
        return inventory.ContainsKey(objectTag);
    }

    // Return how many gameobjects of the specified tag the player has in their inventory
    public int GetAmount(string objectTag)
    {
        return inventory.ContainsKey(objectTag) ? inventory[objectTag].Count : 0;
    }
}
