using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour
{
    [Header("References")]
    public Inventory inventory;
    public List<Button> slotButtons = new List<Button>();
    public Button useButton; // Optional: a separate Use button

    private int selectedIndex = -1;

    private void Start()
    {
        // Set up slot button listeners
        for (int i = 0; i < slotButtons.Count; i++)
        {
            int index = i; // local copy for closure
            slotButtons[i].onClick.AddListener(() => SelectItem(index));
        }

        // Optional: listener for Use button
        if (useButton != null)
            useButton.onClick.AddListener(OnUseButtonPressed);

        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slotButtons.Count; i++)
        {
            // Assume each slot button has a child "Icon" Image
            Image iconImage = slotButtons[i].transform.Find("Icon")?.GetComponent<Image>();

            if (i < inventory.items.Count && inventory.items[i] != null)
            {
                Item item = inventory.items[i];
                if (iconImage != null)
                {
                    iconImage.sprite = item.icon;
                    iconImage.enabled = true;
                }
            }
            else
            {
                if (iconImage != null)
                {
                    iconImage.sprite = null;
                    iconImage.enabled = false;
                }
            }
        }

        HighlightSelectedSlot();
    }

    private void SelectItem(int index)
    {
        if (index < 0 || index >= inventory.items.Count) return;

        selectedIndex = index;
        Item selectedItem = inventory.items[index];
        Debug.Log("Selected item: " + selectedItem.itemName);

        HighlightSelectedSlot();
    }

    private void HighlightSelectedSlot()
    {
        for (int i = 0; i < slotButtons.Count; i++)
        {
            ColorBlock colors = slotButtons[i].colors;
            colors.normalColor = (i == selectedIndex) ? new Color(0.3f, 0.8f, 0.3f) : Color.white;
            slotButtons[i].colors = colors;
        }
    }

    public void OnUseButtonPressed()
    {
        if (selectedIndex >= 0 && selectedIndex < inventory.items.Count)
        {
            inventory.UseItem(selectedIndex);
        }
    }
}