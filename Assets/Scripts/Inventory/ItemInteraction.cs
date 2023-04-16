using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemInteraction : NetworkBehaviour
{
    Transform playerTransform;
    Camera playerCamera;
    [SerializeField] LayerMask itemLayer;
    InventorySystem inventorySystem;

    ClimbingSystem climbingSystem;
    GameObject HoveredItem;
    [SerializeField] TextMeshProUGUI txt_HoveredItem;

    [SerializeField] public float distanceToPickUp = 5f;

    void Awake(){
    }

    void Start(){
        if(IsOwner){
            playerTransform = GetComponent<Transform>();
            playerCamera = GetComponentInChildren<Camera>();
            HoveredItem = GameObject.Find("ItemHoverText");
            txt_HoveredItem = HoveredItem.GetComponentInChildren<TextMeshProUGUI>();
            txt_HoveredItem.text = string.Empty;
            inventorySystem = GetComponent<InventorySystem>();
            climbingSystem = GetComponent<ClimbingSystem>();
        }
    }

    void Update(){
        if(!IsOwner) return;

        
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if(!Physics.Raycast(ray, out hit, 1000, itemLayer)) {
            txt_HoveredItem.text = string.Empty;
            return;
        }
        

        if(Physics.Raycast(ray, out hit, 1000, itemLayer))
        {
            if(Vector3.Distance(playerTransform.position, hit.collider.transform.position) > distanceToPickUp){
            return;
            }

            if(hit.collider.GetComponent<ItemObject>()){
                txt_HoveredItem.text = $"Press 'F' to pick up {hit.collider.GetComponent<ItemObject>().amount}x {hit.collider.GetComponent<ItemObject>().itemStats.itemName}";
                Debug.Log("Text of Item :" + txt_HoveredItem.text);

                if(Input.GetKeyDown(KeyCode.F)){
                   inventorySystem.PickUpItem(hit.collider.GetComponent<ItemObject>());
                   Debug.Log("Item picked up. Item:" + hit.collider.GetComponent<ItemObject>().itemStats.itemName + " Amount:" + hit.collider.GetComponent<ItemObject>().amount);
                }
            }
            else if(hit.collider.GetComponent<ClimbObject>()){
                txt_HoveredItem.text = $"Press 'F' to climb";

                if(Input.GetKeyDown(KeyCode.F)){
                    climbingSystem.Climb(hit.collider.GetComponent<ClimbObject>());
                    Debug.Log("Climbing...");
                }
            }   
        }
        else
        {
            txt_HoveredItem.text = string.Empty;
            Debug.Log("No item in range.");
        }
    }
}
