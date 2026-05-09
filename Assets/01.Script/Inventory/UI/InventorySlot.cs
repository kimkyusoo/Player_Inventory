using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI References")]
    public Image itemIcon;      
    public Button slotButton;

    private DropItem currentItem;

    void Awake()
    {
        slotButton.onClick.AddListener(OnSlotClicked);
    }

    public void UpdateSlot(DropItem item)
    {
        currentItem = item;

        if (item != null)
        {
            itemIcon.gameObject.SetActive(true);

            itemIcon.sprite = item.itemIcon;

            Color color = itemIcon.color;
            color.a = 1f;
            itemIcon.color = color;

        }
        else
        {
            itemIcon.gameObject.SetActive(false);
            itemIcon.sprite = null;
        }
    }

    public void OnSlotClicked()
    {
        if (currentItem != null)
        {
            currentItem.OnClick();
        }
        else
        {
            Debug.LogWarning("슬롯이 비어있어서 사용할 아이템이 없습니다.");
        }
    }
}