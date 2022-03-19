using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetMenuController : MonoBehaviour
{

    public string playerName;
    public Text lobbyAguardar;
    public Text lobbyTime;
    public Text playerStatusText;
    public GameObject painelLobby;
    public Text countdownText;
    public Text statusServer;
    public string lobbyTimeStartText = "Start Game in {0}...";

    private void Awake()
    {

        playerName = "Player" + UnityEngine.Random.Range(1000, 10000);
        
        //playerNameInput.text = playerName;
        Debug.Log(playerName);
        //Debug.Log(playerNameInput.text);
    }

    public void PainelLobbyActive()
    {
        //painelLogin.gameObject.SetActive(false);
        painelLobby.gameObject.SetActive(true);
    }
}
