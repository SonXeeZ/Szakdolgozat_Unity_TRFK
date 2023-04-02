using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InventorySystem : NetworkBehaviour
{
    [SerializeField] public Slot[] slots = new Slot[40];
    [SerializeField] GameObject InventoryUI;



    public override void OnNetworkSpawn(){
        if(IsOwner){
            enabled = true;
            InventoryUI = GameObject.Find("InventoryPanel");
            InventoryUI.SetActive(false);
        }
           
    }

    private void Update(){
        if(IsOwner){ 
            EnableInventoryUI();
        }
    }
        
    private void Start(){
        if(IsOwner){
            AssignSlots();
        }
    }

    private void EnableInventoryUI(){
        if(!InventoryUI.activeInHierarchy && Input.GetKeyDown(KeyCode.E)){
            InventoryUI.SetActive(true);
        }
        else if (InventoryUI.activeInHierarchy && Input.GetKeyDown(KeyCode.E) ||Input.GetKeyDown(KeyCode.Escape)){
            InventoryUI.SetActive(false);
        }
    }

    private void AssignSlots(){
        for(int i = 0; i < slots.Length; i++){
            if(InventoryUI.transform.GetChild(i).GetComponent<Slot>() != null){
                slots[i] = InventoryUI.transform.GetChild(i).GetComponent<Slot>();
                if(slots[i].itemInSlot == null){
                    for(int j = 0; j < slots[i].transform.childCount; j++){
                        slots[i].transform.GetChild(j).gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    
    public void PickUpItem(ItemObject obj)
    {
        int emptySlotIndex = -1;
        int stackableSlotIndex = -1;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemInSlot == null)
            {
                if (emptySlotIndex == -1)
                {
                    emptySlotIndex = i;
                }
            }
            else if (slots[i].itemInSlot.itemId == obj.itemStats.itemId && slots[i].AmountInSlot < slots[i].itemInSlot.maxItemStack)
            {
                if (stackableSlotIndex == -1)
                {
                    stackableSlotIndex = i;
                }
            }
        }

        if (stackableSlotIndex != -1)
        {
            int spaceAvailableInSlot = slots[stackableSlotIndex].itemInSlot.maxItemStack - slots[stackableSlotIndex].AmountInSlot;
            int amountToAdd = Mathf.Min(spaceAvailableInSlot, obj.amount);
            slots[stackableSlotIndex].AmountInSlot += amountToAdd;
            obj.amount -= amountToAdd;

            if (obj.amount == 0)
            {
                Destroy(obj.gameObject);
                slots[stackableSlotIndex].SetStats();
                return;
            }
        }

        if (emptySlotIndex != -1)
        {
        slots[emptySlotIndex].itemInSlot = obj.itemStats;
        slots[emptySlotIndex].AmountInSlot = obj.amount;
        //Destroy(obj.gameObject);
        NetworkManager.Destroy(obj.gameObject);
        //DestroyPickedUpItemOnAllClientsServerRpc(obj);
        slots[emptySlotIndex].SetStats();
        return;
        }
    }



    bool WillHitMaxStack(int index, int amount)
    {
        if (slots[index].itemInSlot.maxItemStack <= slots[index].AmountInSlot + amount)
            return true;
        else
            return false;
    }

    int NeededToFill(int index)
    {
        return slots[index].itemInSlot.maxItemStack - slots[index].AmountInSlot;
    }
   int RemainingAmount(int index, int amount)
    {
        return  (slots[index].AmountInSlot + amount)-slots[index].itemInSlot.maxItemStack;
    }

}
