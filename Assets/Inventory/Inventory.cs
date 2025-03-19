using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public int quantity;

    public Item(string name, int quantity)
    {
        this.name = name;
        this.quantity = quantity;   
    }
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [SerializeField] private string inventoryFilePath = "Assets/Inventory/Inventory.txt";

    private Dictionary<string, Item> items = new Dictionary<string, Item>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadInventory();
    }

    public void LoadInventory()
    {
        items.Clear();

        if (!File.Exists(inventoryFilePath))
        {
            Debug.Log("Inventory file not found. Creating new inventory.");
            SaveInventory();
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines(inventoryFilePath);

            // Skip header line
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] parts = line.Split('|');
                if (parts.Length >= 5)
                {
                    string name = parts[0].Trim();
                    int quantity = int.Parse(parts[1].Trim());
                    string description = parts[4].Trim();

                    items[name] = new Item(name, quantity);
                }
            }

            Debug.Log($"Loaded {items.Count} items from inventory file.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading inventory: {e.Message}");
        }
    }

    public void SaveInventory()
    {
        try
        {
            StringBuilder sb = new StringBuilder();

            // Header
            sb.AppendLine("Item: Quantity");

            // Items
            foreach (var item in items.Values)
            {
                sb.AppendLine($"{item.name}: {item.quantity}");
            }

            File.WriteAllText(inventoryFilePath, sb.ToString());
            Debug.Log("Inventory saved to file.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving inventory: {e.Message}");
        }
    }

    public string GetInventoryText()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("AVAILABLE ITEMS:");

        foreach (var item in items.Values)
        {
            sb.AppendLine($"- {item.name}: {item.quantity} units");
        }

        return sb.ToString();
    }

    public bool HasItem(string itemName, int amount = 1)
    {
        return items.ContainsKey(itemName) && items[itemName].quantity >= amount;
    }

    public bool RemoveItem(string itemName, int amount = 1)
    {
        if (HasItem(itemName, amount))
        {
            items[itemName].quantity -= amount;
            SaveInventory();
            return true;
        }
        return false;
    }

    public void AddItem(string itemName, int amount)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName].quantity += amount;
        }
        else
        {
            items[itemName] = new Item(itemName, amount);
        }

        SaveInventory();
    }

    public List<Item> GetAllItems()
    {
        List<Item> itemList = new List<Item>();
        foreach (var item in items.Values)
        {
            itemList.Add(item);
        }
        return itemList;
    }

}