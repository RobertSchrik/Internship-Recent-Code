using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [Header("Connections")]
    public ItemSpawner itemSpawner;

    [Header("Grid Size")]
    public int gridWidth = 5;
    public int gridHeight = 3;

    [Header("Inventory & Inventory Tile")]
    public GameObject tilePrefab;
    public GameObject inventory;
    public GameObject[] inventorySlots;

    public GameObject[,] grid;

    void Start()
    {
        var gridSpawnLocation = GetComponent<GridSpawner>();

        // Generate the grid
        grid = new GameObject[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 spawnPosition = new Vector3(x * 182 - 363, y * 179 - 177, gridSpawnLocation.transform.position.z);
                GameObject tile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                grid[x, y] = tile;
            }
        }
        itemSpawner.SpawnObject();
        inventorySlots = GameObject.FindGameObjectsWithTag("InventorySlot");
        //When Grid is spawned put grid slots inside Inventory, this step is later to avoid resizing.
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].transform.SetParent(inventory.transform, false);
        }
    }

    public void OffTheGrid()
    {
        foreach (GameObject slot in inventorySlots)
        {
            BoxCollider2D collider = slot.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }

    public void OnTheGrid()
    {
        foreach (GameObject slot in inventorySlots)
        {
            BoxCollider2D collider = slot.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
    }
}
