using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerInventory : MonoBehaviour
{
    public string[] canDropItems = { "Potion", "Gun", "Shose" };

    public List<DropItem> dropList = new List<DropItem>();
    public Queue<string> pickupMessages = new Queue<string>();

    public InputAction MessageAction;
    public InputAction InventoryCheckAction;

    public int inventorySize = 9;

    public static event Action<ItemEffect> effectApplied;
    public event Action OnInventoryChanged;

    private void Start()
    {
        MessageAction.Enable();
        InventoryCheckAction.Enable();
    }

    private void Update()
    {
        if (MessageAction.WasPressedThisFrame())
        {
            PrintMessage();
        }

        if (InventoryCheckAction.WasPressedThisFrame())
        {
            PrintInventory();
        }
    }

    private void OnEnable()
    {
        DropItem.ItemUsed += RequestUseItem;
    }

    private void OnDisable()
    {
        // 메모리 누수 방지를 위해 구독 해제
        DropItem.ItemUsed -= RequestUseItem;
    }

    // 인벤토리 내용 전체 출력
    void PrintInventory()
    {
        Debug.Log("===== 현재 인벤토리 소지 아이템 내역 =====");
        for (int i = 0; i < dropList.Count; i++)
        {
            Debug.Log($"소지 아이템: {dropList[i].itemName}, 효과: {dropList[i].itemEffect.ToString()}");
        }
        Debug.Log($"현재 아이템 수: {dropList.Count}");
        Debug.Log("==========================================");
    }

    // 아이템 넣기
    public void AddItem(DropItem item)
    {
        if (!IsValidItemId(item.itemName))
        {
            Debug.LogWarning($"등록되지 않은 아이템 ID입니다. {item.itemName}");
            return;
        }

        if (dropList.Count < inventorySize)
        {
            dropList.Add(item);
            OnInventoryChanged?.Invoke(); 
        }

        pickupMessages.Enqueue(item.itemName + " 획득!");

        Debug.Log("[Inventory]" + item.itemName + "획득!");
        PrintInventory();
    }

    // 정상 코일 확인하기
    private bool IsValidItemId(string targetId)
    {
        for (int i = 0; i < canDropItems.Length; i++)
        {
            if (canDropItems[i] == targetId)
            {
                return true;
            }
        }
        return false;
    }

    // 코일획득 메세지 출력
    void PrintMessage()
    {
        if (pickupMessages.Count <= 0)
        {
            Debug.Log("처리할 메세지가 없습니다."); return;
        }

        string result = pickupMessages.Dequeue();
        Debug.Log($"[Message] {result}");
    }

    private void RequestUseItem(DropItem item)
    {
        UseItem(item);
    }

    // 아이템 사용
    public bool UseItem(DropItem dropItem)
    {
        if (dropItem == null) return false;
        if (!dropList.Contains(dropItem)) return false;

        ApplyItemEffect(dropItem.itemEffect);

        dropList.Remove(dropItem);

        OnInventoryChanged?.Invoke();

        return true;

    }
    
    private void ApplyItemEffect(ItemEffect effect)
    {
        effectApplied?.Invoke(effect);
    }
}
