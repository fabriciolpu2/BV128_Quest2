using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;
//using CanaimeStudio;
using UnityEngine.SceneManagement;

public class NetController : MonoBehaviourPunCallbacks
{
    public byte playerRoomMax = 2;

    public NetMenuController lobbyGameObject;


    public override void OnEnable()
    {
        base.OnEnable();
        CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimeIsExpired;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimeIsExpired;
    }

    void OnCountdownTimeIsExpired()
    {
        StartGame();

    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // === 1
    public override void OnConnected()
    {
        Debug.Log("Conectado");
    }

    // === 2
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado ao Master");
        lobbyGameObject.PainelLobbyActive();
        //CriaSala();
        PhotonNetwork.JoinLobby();
    }

    // === 3
    public override void OnJoinedLobby()
    {        
        Debug.Log("Entro no Lobby");
        Debug.Log("Salas Existentes: " + PhotonNetwork.CountOfRooms);
        CriaSala();
        //PhotonNetwork.JoinRandomRoom();

    }
    public void CriaSala()
    {
        string roomNameTemp = "Rom01";
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 16 };
        PhotonNetwork.JoinOrCreateRoom(roomNameTemp, roomOptions, TypedLobby.Default);
        lobbyGameObject.statusServer.text = "Entrando na Sala";
    }

    // === 4
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        lobbyGameObject.statusServer.text = "Erro ao Entrar na sala: Servidor Cheio" + returnCode + message;
        string roomName = "Room" + Random.Range(1000, 10000);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = playerRoomMax
        };
        //Carregando Jogo OFFILINE
        string roomNameTemp = "VRom02";
        PhotonNetwork.JoinOrCreateRoom(roomNameTemp, roomOptions, TypedLobby.Default);        
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        lobbyGameObject.statusServer.text = "OnJoinRandomFailed" + returnCode + message;
        //Debug.Log();
        string roomName = "Room" + Random.Range(1000, 10000);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = playerRoomMax
        };
        
        //Carregando Jogo OFFILINE
        //string roomNameTemp = "Rom01";
        
        //PhotonNetwork.JoinOrCreateRoom(roomNameTemp, roomOptions, TypedLobby.Default);
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }


    // ok
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Outro PLAYER ENTROU NA SALA");
        // Verifica numero maximo de Jogadores
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            lobbyGameObject.lobbyTime.gameObject.SetActive(true);
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.IsMasterClient)
                {                    
                    Hashtable props = new Hashtable {
                        {CountdownTimer.CountdownStartTime,  (float) PhotonNetwork.Time}
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                    Debug.Log("IsMasterClient");
                }
            }
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDesconnected: " + cause.ToString());
        lobbyGameObject.painelLobby.SetActive(false);
    }
    // === 6
    public override void OnJoinedRoom()
    {
        Debug.Log("entrou na sala");
        Debug.Log("Nome da Sala: " + PhotonNetwork.CurrentRoom.Name);
        //StartGame();
        
           foreach (var item in PhotonNetwork.PlayerList)
           {
               if (item.IsMasterClient)
               {
                   Hashtable props = new Hashtable {
                       {CountdownTimer.CountdownStartTime,  (float) PhotonNetwork.Time}
                   };
                   PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                   Debug.Log("IsMasterClient");
               }
           }
        
    }

    // === 5 // Apos criar Sala
    // === 7 Apos entrar na sala
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.Log("Aqui no onRoomProperties");

        if (propertiesThatChanged.ContainsKey(CountdownTimer.CountdownStartTime))
        {
            Debug.Log("propertiesThatChanged");
            //lobbyGameObject.lobbyTime.gameObject.SetActive(true);
        }
    }


    void StartGame()
    {
        Debug.LogWarning("Carregan Cenario");
        StartCoroutine(CarregarCenarioMultiplayer3Pessoa());
    }
    IEnumerator CarregarCenarioMultiplayer3Pessoa()
    {
        Debug.Log("Jogando em 3ª Pessoa Metodo");
        yield return new WaitForSeconds(2.0f);
        Debug.LogWarning("Carregan Cenario");
        PhotonNetwork.LoadLevel("Multiplayer");
    }

    public void BotaoCancelar()
    {
        PhotonNetwork.Disconnect(); // DisconnectByClientLogic
    }

    public void BotaoLogin()
    {
        lobbyGameObject.PainelLobbyActive();
        /*
        if(AlunoPlayerController.ALUNO != null)
        {
            PhotonNetwork.NickName = AlunoPlayerController.ALUNO.nome;
        }
        */
        
        PhotonNetwork.NickName = "VR Player";
        lobbyGameObject.playerStatusText.gameObject.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }
}
