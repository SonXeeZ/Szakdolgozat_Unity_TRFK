using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : NetworkBehaviour
{
    [SerializeField] private Image healthBarImage;
    public float CurrentHealth;
    public float MaxHealth;
    [SerializeField] private Character character;

    private void Start()
    {
        if(!IsOwner)
            return;
        character = transform.GetComponent<Character>();
        healthBarImage = GetComponentsInChildren<Canvas>()[1].GetComponentsInChildren<Image>()[1];
        MaxHealth = (float)character.MaxHealth.Value;
        CurrentHealth = (float)character.CurrentHealth.Value;
    }

    public override void OnNetworkSpawn(){
        if(IsLocalPlayer){
            character = transform.GetComponent<Character>();
            healthBarImage = GetComponentsInChildren<Canvas>()[1].GetComponentsInChildren<Image>()[1];
            MaxHealth = (float)character.MaxHealth.Value;
            CurrentHealth = (float)character.CurrentHealth.Value;
        }
    }

    private void Update()
    {
        if(IsOwner && IsLocalPlayer){
            UpdateHealthbar();
        }
        
    }

    void UpdateHealthbar(){
        MaxHealth = (float)character.MaxHealth.Value;
        CurrentHealth = (float)character.CurrentHealth.Value;
        healthBarImage.fillAmount = CurrentHealth / MaxHealth;
    }
    
}
