using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Firebase;
using Firebase.Auth;

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
    [SerializeField] private NetworkVariable<float> speed = new NetworkVariable<float>();
    public NetworkVariable<float> Speed { get => speed; set => speed = value; }

    private NetworkVariable<NetworkString> characterName = new NetworkVariable<NetworkString>();
    public NetworkVariable<NetworkString> CharacterName { get => characterName; set => characterName = value; }

    private NetworkVariable<int> maxLevel = new NetworkVariable<int>();
    public NetworkVariable<int> MaxLevel { get => maxLevel; set => maxLevel = value; }

    private NetworkVariable<int> currentExperience = new NetworkVariable<int>();
    public NetworkVariable<int> CurrentExperience { get => currentExperience; set => currentExperience = value; }

    private NetworkVariable<int> neededExperienceToLevelUp = new NetworkVariable<int>();
    public NetworkVariable<int> NeededExperienceToLevelUp { get => neededExperienceToLevelUp; set => neededExperienceToLevelUp = value; }

    private Transform enemyTransform;
    private bool overlaySet = false; // -- FK -- 2023.03.01 18:10


    // TODO: PlayerState

    private NetworkVariable<bool> canAttack = new NetworkVariable<bool>(); // -- FK -- 2023.02.23 23:15
    
    private TextMeshProUGUI characterNameUI;

    [SerializeField] public TextMeshProUGUI UI_Experience_Text;

    
    public override void OnNetworkSpawn()
    {

        if(IsServer){
            // TODO: Lekérni az adatbázisból a karakter adatait.
            MaxHealth.Value = 100;
            CurrentHealth.Value = MaxHealth.Value;
            Damage.Value = 10;
            Level.Value = 1;
            MaxLevel.Value = 30;
            Speed.Value = 5.12f;
            canAttack.Value = false;
            currentExperience.Value = 0;
            neededExperienceToLevelUp.Value = CalculateNeededExperienceToLevelUP();
            AssignUIExperienceComponent();
            
            UI_Experience_Text.text = "XP: " + currentExperience.Value + "/" + neededExperienceToLevelUp.Value;

            

            characterNameUI = gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
            canAttack.OnValueChanged += CanAttackOnValueChanged; // -- FK -- 2023.02.23 1:20
            CurrentHealth.OnValueChanged += CurrentHealthOnValueChanged; // -- FK -- 2023.02.24 18:19
            CharacterName.OnValueChanged += CharacterNameOnValueChanged;
        
        } 
        
        if(IsOwner){
            var user = FirebaseAuth.DefaultInstance.CurrentUser;
            if(user != null){
                ChangeCharacterNameServerRpc(user.DisplayName, OwnerClientId);
                UpdateCharacterNameServerRpc(user.DisplayName);
                Debug.Log("Currently joined player: " + user.DisplayName);
            }
        }

        if(IsClient && IsLocalPlayer){
            
            MaxHealth.Value = 100;
            CurrentHealth.Value = MaxHealth.Value;
            Damage.Value = 10;
            Level.Value = 1;
            MaxLevel.Value = 30;
            Speed.Value = 5.12f;
            canAttack.Value = false;
            currentExperience.Value = 0;
            neededExperienceToLevelUp.Value = CalculateNeededExperienceToLevelUP();
            AssignUIExperienceComponent();
            UI_Experience_Text.text = "XP: " + currentExperience.Value + "/" + neededExperienceToLevelUp.Value;
            Debug.Log("Needed experience to level up: " + neededExperienceToLevelUp.Value + " on client: " + OwnerClientId);
        }

        
        
    }

    #region Experience

    private void AssignUIExperienceComponent(){
        GameObject canvas = GameObject.Find("Canvas");
        GameObject UIExp = canvas.transform.Find("UI_Experience").gameObject;
        UI_Experience_Text = UIExp.GetComponentInChildren<TextMeshProUGUI>();
    }

    private int CalculateNeededExperienceToLevelUP(){
        return NeededExperienceToLevelUp.Value = (int) (100 * Math.Log(Level.Value + 1));
    }

    private void AddExperience(int amount,ulong clientId){
        AddExperienceServerRpc(amount,clientId);
    }

    [ServerRpc]
    private void AddExperienceServerRpc(int amount, ulong clientId){

            var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            var playerCharacter = playerObject.GetComponent<Character>();
            if(playerCharacter == null) return;
            if(playerCharacter.Level.Value == playerCharacter.MaxLevel.Value) return;
            if(playerCharacter.currentExperience.Value + amount > playerCharacter.NeededExperienceToLevelUp.Value){
                var remainingExperience = (playerCharacter.currentExperience.Value + amount) - playerCharacter.NeededExperienceToLevelUp.Value;
                playerCharacter.currentExperience.Value = 0;
                playerCharacter.Level.Value += 1;
                playerCharacter.UI_Experience_Text.text = "XP: " + playerCharacter.currentExperience.Value + "/" + playerCharacter.NeededExperienceToLevelUp.Value;
                
                AddExperienceServerRpc(remainingExperience, playerCharacter.OwnerClientId);
            } else if(playerCharacter.currentExperience.Value + amount == playerCharacter.NeededExperienceToLevelUp.Value){
                playerCharacter.currentExperience.Value = 0;
                playerCharacter.Level.Value += 1;
                playerCharacter.UI_Experience_Text.text = "XP: " + playerCharacter.currentExperience.Value + "/" + playerCharacter.NeededExperienceToLevelUp.Value;
                
            } else {
                playerCharacter.currentExperience.Value += amount;
                playerCharacter.UI_Experience_Text.text = "XP: " + playerCharacter.currentExperience.Value + "/" + playerCharacter.NeededExperienceToLevelUp.Value;
            }
            
            
            
    }
    #endregion



    [ServerRpc(RequireOwnership = false)]
    private void UpdateClientCanvasRoationServerRpc(ulong clientId){
        UpdateClientCavasRotationClientRpc(clientId);
    }

    [ClientRpc]
    private void UpdateClientCavasRotationClientRpc(ulong clientId)
    {
        if(clientId == OwnerClientId){
            FreezeCanvasRotation(GetComponentInChildren<Canvas>());
        }
            
    }

    private void FreezeCanvasRotation(Canvas canvas){ 
        canvas.transform.rotation = Quaternion.Euler(15, 0, 0);
    }


    private void OnLocalCharacterNameChanged(NetworkString previousValue, NetworkString newValue)
    {
        characterNameUI.text = newValue.ToString();
    }

    [ServerRpc]
    private void ChangeCharacterNameServerRpc(string name, ulong clientId)
    {
        var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;

        if(playerObject != null){
            var character = playerObject.GetComponent<Character>();
            if(character != null){
                character.CharacterName.Value = name;
                Debug.Log("[Server]: Character name changed to: " + name);
            }
        }else{
            Debug.Log("Nincs ilyen playerObject.");
        }
    }

    // -- FK -- 2023.03.01 18:10
    public void SetOverlay(){
        var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        localPlayerOverlay.text = CharacterName.Value;
    }


    private void Start(){ 
    }

    private void CharacterNameOnValueChanged(NetworkString previousValue, NetworkString newValue)
    {
        UpdateCharacterNamesOnClientServerRpc(newValue, OwnerClientId);
        Debug.Log("Character name changed to: " + newValue + "on clientid: " + OwnerClientId);
    }

    
    

    [ServerRpc]
    private void UpdateCharacterNamesOnClientServerRpc(string newName, ulong clientId){

        var playerNetworkObjectId = GetPlayerNetworkObjectId(clientId);
        var networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerNetworkObjectId.Value];
            if(networkObject != null){
                var character = networkObject.GetComponent<Character>();
                if(character != null){
                    character.CharacterName.Value = newName;
                    Debug.Log("[Client]: Other player changed name to " + newName);
                }
            }

    }

    private ulong? GetPlayerNetworkObjectId(ulong clientId)
    {
        return NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<NetworkObject>().NetworkObjectId;
    }

    [ServerRpc]
    public void UpdateCharacterNameServerRpc(string newName)
    {
        characterName.Value = newName;
        
    }

    void Update(){
        // -- FK -- 2023.03.01 18:10
        
        
        if(!overlaySet && !string.IsNullOrEmpty(characterName.Value)){ 
            SetOverlay();
            overlaySet = true;
        }


        if(IsOwner && IsLocalPlayer){
            DealDamage(Damage.Value);
            UpdateClientCanvasRoationServerRpc(OwnerClientId);
            Debug.Log("CharacName value: " + CharacterName.Value);

            if(Input.GetKeyDown(KeyCode.Space)){
                AddExperience(17,OwnerClientId);
                Debug.Log("Experience added for client: " + OwnerClientId);
            }

            //ClientRequestNameUpdate();
            //transform.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = CharacterName.Value;
        }
    }
    

    


    [ServerRpc]
    public void DealDamageServerRpc(int damage)
    {
        if(enemyTransform.GetComponent<Slime>().EnemyCurrentHealth.Value <= 0){
            return;
        }else{
        } 

        if(!canAttack.Value) return;
        //var enemyId = NetworkManager.Singleton.ConnectedClients[enemyTransform.gameObject.GetComponent<NetworkObject>().OwnerClientId];
        enemyTransform.GetComponent<Slime>().EnemyCurrentHealth.Value -= damage;
    }
    
    // https://docs.unity3d.com/ScriptReference/Collider.OnTriggerEnter.html
    private void OnTriggerEnter(Collider other)
    {
        if(IsOwner){
            //Check if Collider is a Box Collider
            if(transform.GetComponent<Collider>() is BoxCollider){
                Debug.Log("Box Collider");
            }
            /*if(other.GetComponent<Collider>() is CapsuleCollider){
                Debug.Log("Capsule Collider");
                return;
            }*/
            if(other.gameObject.CompareTag("Enemy")){ // -- FK -- 2023.02.23 23:30

            Debug.Log("You entered in enemy trigger.");
            //TODO: GetDistance Vector3 a legközelebbi "player" kiválasztásához.

            EnteredInEnemyTriggerWithPlayerServerRPC(other.gameObject.GetComponent<NetworkObject>().NetworkObjectId,30);
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
    private void EnteredInEnemyTriggerWithPlayerServerRPC(ulong enemyPlayerId, int damage)
    {
        transform.GetComponent<Character>().canAttack.Value = true;
        var enemy = NetworkManager.Singleton.SpawnManager.SpawnedObjects[enemyPlayerId];
        enemyTransform = enemy.transform;
        enemyTransform.GetComponent<Slime>().TakeDamage(damage);
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


    // https://www.youtube.com/watch?v=rFCFMkzFaog&list=PLQMQNmwN3FvyyeI1-bDcBPmZiSaDMbFTi -- FK -- 2023.02.25 2:50 [TODO: BEIRNI A DOCSBA]

    
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
