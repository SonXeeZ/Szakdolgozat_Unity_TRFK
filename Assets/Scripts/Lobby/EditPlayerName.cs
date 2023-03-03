using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class EditPlayerName : MonoBehaviour {


    public static EditPlayerName Instance { get; private set; }


    public event EventHandler OnNameChanged;


    [SerializeField] private TextMeshProUGUI playerNameText;


    //private string playerName = "Code Monkey";

    public static string email;

    private void Awake() {
        Instance = this;

        GetComponent<Button>().onClick.AddListener(() => {
            UI_InputWindow.Show_Static("Player Name", email, "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ .,-@", 20,
            () => {
                // Cancel
            },
            (string newName) => {
                email = newName;

                playerNameText.text = email;

                OnNameChanged?.Invoke(this, EventArgs.Empty);
            });
        });

        playerNameText.text = email;
    }

    private void Start() {
        //OnNameChanged += EditPlayerName_OnNameChanged;
        if (FirebaseAuth.DefaultInstance.CurrentUser != null) {
            email = FirebaseAuth.DefaultInstance.CurrentUser.Email;
        }
    }

    private void EditPlayerName_OnNameChanged(object sender, EventArgs e) {
        LobbyManager.Instance.UpdatePlayerName(GetPlayerName());
    }

    public string GetPlayerName() {
        return email;
    }


}