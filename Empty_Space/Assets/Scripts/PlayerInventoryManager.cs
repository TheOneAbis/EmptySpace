using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public Dictionary<string, List<GameObject>> inventory;

    // Start is called before the first frame update
    void Start()
    {
        // Init the inventory
        inventory = new Dictionary<string, List<GameObject>>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Add a new game object to the player's inventory
    public void AddToInventory(GameObject obj)
    {
        if (!inventory.ContainsKey(obj.tag))
        {
            inventory.Add(obj.tag, new List<GameObject>());
            Debug.Log($"New object tag added to inventory: {obj}");
        }
        inventory[obj.tag].Add(obj);
        obj.SetActive(false);

        Debug.Log(inventory[obj.tag].Count + " items in slot of tag " + inventory[obj.tag]);
    }

    // Remove a specified amount of objects from the inventory, if there are enough of them. If not, does nothing.
    public void Remove(string objectTag, int amountToRemove)
    {
        if (GetAmount(objectTag) >= amountToRemove)
        {
            inventory[objectTag].RemoveRange(0, amountToRemove);
            if (GetAmount(objectTag) == 0) inventory.Remove(objectTag);
        }
    }

    public void Remove(GameObject objToRemove)
    {
        if (Contains(objToRemove.tag))
        {
            inventory[objToRemove.tag].Remove(objToRemove);
            objToRemove.SetActive(true);
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
