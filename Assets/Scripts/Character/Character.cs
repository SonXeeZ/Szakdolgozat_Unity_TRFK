using System;
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

    private Transform enemyTransform;


    // TODO: PlayerState
    private NetworkVariable<bool> canAttack = new NetworkVariable<bool>(); // -- FK -- 2023.02.23 23:15


    [ServerRpc]
    public void DealDamageServerRpc(int damage)
    {
        if(enemyTransform.GetComponent<Character>().currentHealth.Value <= 0){
            return;
        }else{
        } 

        if(!canAttack.Value) return;
        var enemyId = NetworkManager.Singleton.ConnectedClients[enemyTransform.gameObject.GetComponent<NetworkObject>().OwnerClientId];
        enemyTransform.GetComponent<Character>().CurrentHealth.Value -= damage;
    }
    
    // https://docs.unity3d.com/ScriptReference/Collider.OnTriggerEnter.html
    private void OnTriggerEnter(Collider other)
    {
        if(IsOwner){
            if(other.gameObject.CompareTag("Player")){ // -- FK -- 2023.02.23 23:30
            
            //TODO: GetDistance Vector3 a legközelebbi "player" kiválasztásához.

            EnteredInEnemyTriggerWithPlayerServerRPC(other.gameObject.GetComponent<NetworkObject>().OwnerClientId);
            }
            else{
                Debug.Log("You can't attack this object.");
            }
        }
    }

    [ServerRpc]
    private void EnableAttackToSelfServerRpc()
    {
        canAttack.Value = true;
    }

    [ServerRpc]
    private void EnteredInEnemyTriggerWithPlayerServerRPC(ulong enemyPlayerId)
    {
        transform.GetComponent<Character>().canAttack.Value = true;
        enemyTransform = NetworkManager.Singleton.ConnectedClients[enemyPlayerId].PlayerObject.transform;
        EnemySetMessageClientRpc(enemyPlayerId);
    }

    public void DealDamage(int damage)
    {

        if(Input.GetKeyDown(KeyCode.Space) && canAttack.Value == true){
            DealDamageServerRpc(damage);
        } else{
            return;
        }
    }

    public void Die()
    {
        Debug.Log("You're dead.");
    }

    

    private void Start(){ 
    } 

    void Update(){

        if(IsOwner){
            DealDamage(Damage.Value);
        }
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer){
            // TODO: Lekérni az adatbázisból a karakter adatait.
            MaxHealth.Value = 100;
            CurrentHealth.Value = MaxHealth.Value;
            Damage.Value = 10;
            Level.Value = 1;
            Experience.Value = 0;
            Speed.Value = 5.12f;
            canAttack.Value = false;

            canAttack.OnValueChanged += CanAttackOnValueChanged; // -- FK -- 2023.02.23 1:20
            CurrentHealth.OnValueChanged += CurrentHealthOnValueChanged; // -- FK -- 2023.02.24 18:19
        }  
    }

    private void CurrentHealthOnValueChanged(int previousValue, int newValue)
    {
        Debug.Log("[CurrentHealthOnValueChanged] new:  " + newValue + "previous value: " + previousValue);
    }

    private void CanAttackOnValueChanged(bool previousValue, bool newValue) // -- FK -- 2023.02.23 1:23
    {
        Debug.Log("[CanAttackOnValueChanged] new:" + newValue + "previous value: " + previousValue);
    }

    

    private void OnTriggerExit(Collider other){
        if(IsOwner){
            LeftTriggerServerRPC();
        }
        
    }



    

    
    [ServerRpc]
    private void LeftTriggerServerRPC()
    {
        transform.GetComponent<Character>().canAttack.Value = false;  
        enemyTransform = null;   
    }

    [ClientRpc]
    private void EnemySetMessageClientRpc(ulong enemyPlayerId)
    {
        Debug.Log("Enemy player set to id: " + enemyPlayerId);
    }

}
