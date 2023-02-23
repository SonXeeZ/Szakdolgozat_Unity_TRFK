using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Character : NetworkBehaviour, ICharacter, ICharacterStats
{
    [SerializeField] private NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    public NetworkVariable<int> CurrentHealth { get => currentHealth; set => currentHealth = value; }

    [SerializeField] private NetworkVariable<int> maxHealth = new NetworkVariable<int>();
    public NetworkVariable<int> MaxHealth { get => maxHealth ; set => maxHealth = value; }

    [SerializeField] private NetworkVariable<int> damage = new NetworkVariable<int>();
    public NetworkVariable<int> Damage { get => damage; set => damage = value; }

    [SerializeField]private NetworkVariable<int> level = new NetworkVariable<int>();
    public NetworkVariable<int> Level { get => level; set => level = value; }

    [SerializeField] private NetworkVariable<int> experience = new NetworkVariable<int>();
    public NetworkVariable<int> Experience { get => experience; set => experience = value ; }
    [SerializeField] private NetworkVariable<float> speed = new NetworkVariable<float>();
    public NetworkVariable<float> Speed { get => speed; set => speed = value; }

    [ServerRpc]
    public void DealDamageServerRpc(ulong targetClientId, int damage)
    {
        if(currentHealth.Value <= 0){
            return;
        }else{
            Debug.Log("Not dead.");
        } // TODO: IsDead?

        var targetPlayer = NetworkManager.Singleton.ConnectedClients[targetClientId].PlayerObject.GetComponent<Character>();
        var targetCharacter = targetPlayer.GetComponent<Character>();

        targetCharacter.TakeDamageClientRpc(damage);
    }
    

    public void DealDamage(int damage, Collision collision)
    {

        if(Input.GetKeyDown(KeyCode.Space) && collision.gameObject.CompareTag("Player")){
            var targetPlayer = collision.gameObject.GetComponent<Character>();

            DealDamageServerRpc(targetPlayer.OwnerClientId, damage);
        }
    }

    public void TakeDamage(int damage){
        if(!IsServer) return; // Csak a szerveren kell számolni az életerőt.

        TakeDamageClientRpc(damage);
    }

    [ClientRpc]
    public void TakeDamageClientRpc(int damage)
    {
        currentHealth.Value -= damage;
        Debug.Log("[ClientRpc]: Took damage. " + damage );

        if(currentHealth.Value <= 0){
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Dead.");
    }

    

    private void Start(){

        
        

        
    } 

    void Update(){

        
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer){
            MaxHealth.Value = 100;
            CurrentHealth.Value = MaxHealth.Value;
            Damage.Value = 10;
            Level.Value = 1;
            Experience.Value = 0;
            Speed.Value = 5.12f;
        }  
    }

    // https://docs.unity3d.com/ScriptReference/Collider.OnCollisionEnter.html

    // https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/physics/index.html
    /*
        - NetworkRigidbody -t kellett hozzáadni a Character prefabhoz.
        - 
    */
    
    private void OnCollisionStay(Collision collision)
    {
        // Debug.Log("OnCollisionStay called.");
        DealDamage(Damage.Value, collision);
    }
}
