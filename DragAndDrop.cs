using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    [Header("Connections")]
    public MergeManager mergeManager;
    public ItemScripts itemScripts;
    public GridSpawner gridSpawner;
    public PlayerDeathScreen playerDeathScreen;

    private Vector3 screenPoint;
    private Vector3 offset;
    private Transform originalParent;
    private Vector3 originalPosition;
    private Collider2D itemCollider;

    public void Start()
    {
        gridSpawner = GameObject.FindObjectOfType<GridSpawner>();
        mergeManager = GameObject.FindObjectOfType<MergeManager>();
        itemScripts = GameObject.FindObjectOfType<ItemScripts>();
        playerDeathScreen = GameObject.FindObjectOfType<PlayerDeathScreen>();
        itemCollider = GetComponent<Collider2D>();
        gridSpawner.OffTheGrid();
    }


    void OnMouseDown()  
    {
        int layerMask = 1 << LayerMask.NameToLayer("InventorySlots");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100.0f, layerMask);
        if (!playerDeathScreen.playerDeathConfirmed)
        {
            originalParent = transform.parent;
            originalPosition = transform.position;

            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

            // Disable the item's collider to exclude it from raycast detection
            itemCollider.enabled = false;
        }
    }

    void OnMouseDrag()
    {
        if (!playerDeathScreen.playerDeathConfirmed)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }
    }

    public void OnMouseUp()
    {
        gridSpawner.OnTheGrid();
        if (!playerDeathScreen.playerDeathConfirmed)
        {
            int layerMask = 1 << LayerMask.NameToLayer("InventorySlots");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100.0f, layerMask);

            int layerMask3 = 1 << LayerMask.NameToLayer("ItemLayer");
            RaycastHit2D hit3 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100.0f, layerMask3);

            // Re-enable the item's collider
            itemCollider.enabled = true;

            if (hit.collider != null)
            {
                if (hit.transform.childCount == 0)
                {
                    transform.SetParent(hit.transform);
                    transform.localPosition = new Vector3(0, 0, -1);
                }
                else if (hit3.collider != null && hit3.collider.gameObject.name == gameObject.name && hit3.collider.gameObject.tag == gameObject.tag)
                {
                    transform.localPosition = new Vector3(0, 0, -1);
                    Destroy(this.gameObject);
                    mergeManager.MergeItems(hit3.collider.gameObject);
                    itemScripts.ItemValueHandler(hit3.collider.gameObject);
                }
                else
                {
                    transform.SetParent(originalParent);
                    transform.localPosition = originalPosition;
                    transform.localPosition = new Vector3(0, 0, -1);
                }
            }
            else
            {
                transform.SetParent(originalParent);
                transform.localPosition = originalPosition;
            }
            transform.localPosition = new Vector3(0, 0, -1);
        }
        gridSpawner.OffTheGrid();
    }
}
