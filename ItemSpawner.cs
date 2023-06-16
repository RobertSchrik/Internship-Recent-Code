using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Connections")]
    public GridSpawner gridSpawner;
    public ManaManager manaManager;
    public ItemManager itemManager;
    public ItemScripts itemScripts;
    public MergeManager mergeManager;
    public Player player;

    private GameObject selectedObject;

    [Header("Base mana cost & Display")]
    public TextMeshProUGUI costDisplay;
    public float manaCost = 5;

    public void SpawnObject()
    {
        //Break check.
        if (gridSpawner.inventorySlots.Length == 0) return;

        int randomIndex;
        int slotsFilled = 0;

        //grabs the TMP text and converts it to string (convoluted asf)
        string currentManaCost = costDisplay.text;
        int currentManaInt = int.Parse(currentManaCost);

        for (int i = 0; i < gridSpawner.inventorySlots.Length; i++)
        {
            if (gridSpawner.inventorySlots[i].transform.childCount > 0)
            {
                slotsFilled++;
            }
        }

        while (true)
        {
            //Randomly select inventory slot.
            randomIndex = Random.Range(0, gridSpawner.inventorySlots.Length);
            selectedObject = gridSpawner.inventorySlots[randomIndex];

            //Check if object already has a child.
            if (selectedObject.transform.childCount == 0) break;

            //Give exception when all slots are filled.
            if (slotsFilled >= gridSpawner.inventorySlots.Length)
            {
                Debug.LogError("All slots are filled, cannot add any more objects");
                return;
            }
        }

        int randomNumber = Random.Range(0, itemManager.mergeRank1.Length);

        //check if there is surfficient mana to be able to spawn the item, reducing the amount of mana from the mana pool and increasing the mana cost
        if (manaManager.manaCount >= currentManaInt)
        {
            int newManaCost = currentManaInt + 5;
            costDisplay.text = newManaCost.ToString();

            manaManager.manaCount = manaManager.manaCount - currentManaInt;
            manaManager.ManaUpdate();
        }
        else
        {
            Debug.Log("Insurfficient Mana");
            return;
        }

        //define what object should be spawned
        GameObject objectToSpawn = itemManager.mergeRank1[randomNumber];

        itemScripts.ItemSpawnCount(randomNumber);
        //spawn object on parent location and set as child.
        GameObject newObject = Instantiate(objectToSpawn, selectedObject.transform.position, Quaternion.identity);
        newObject.transform.SetParent(selectedObject.transform);
        newObject.transform.localScale = new Vector3(.85f, .85f, 1f);

        float offsetZ = -1f;
        newObject.transform.localPosition = new Vector3(0, 0, offsetZ);

        Renderer renderer = newObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = -1; // Adjust the sorting order as needed
        }

        player.UpdatePlayerStats();
    }
}
