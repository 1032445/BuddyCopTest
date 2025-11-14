using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    public List<Item> items = new List<Item>();

    [Header("Throw Settings")]
    public Transform throwOrigin;

    private Item selectedItem;
    public CameraSwitcher cameraSwitcher;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && items.Count > 0)
        {
            selectedItem = items[0];
            Debug.Log("Selected: " + selectedItem.itemName);
        }

        if (Input.GetMouseButtonDown(0) && selectedItem != null)
        {
            Vector3 mouseWorldPos = cameraSwitcher.cameras[cameraSwitcher.currentIndex].ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            ThrowItem(selectedItem, mouseWorldPos);

            selectedItem = null;
        }
    }

    public void ThrowItem(Item item, Vector3 targetPosition)
    {
        if (throwOrigin == null || item.throwablePrefab == null) return;

        GameObject thrown = Instantiate(item.throwablePrefab, throwOrigin.position, Quaternion.identity);

        Vector3 direction = targetPosition - throwOrigin.position;
        direction.z = 0f;

        float maxThrowDistance = 3f;
        float distance = Mathf.Min(direction.magnitude, maxThrowDistance);
        direction.Normalize();

        Rigidbody2D rb = thrown.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float throwForce = item.throwForce * distance;
            rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        }

        Destroy(thrown, item.lifetime);

        Debug.Log("Threw: " + item.itemName);
    }

    public void AddItem(Item item)
    {
        if (item == null) return;

        items.Add(item);
        Debug.Log("Picked up: " + item.itemName);
    }

    public void UseItem(int index)
    {
        if (index < 0 || index >= items.Count) return;

        Item item = items[index];
        if (item == null) return;

        selectedItem = item;
        Debug.Log("Selected via UI: " + selectedItem.itemName);
    }
}