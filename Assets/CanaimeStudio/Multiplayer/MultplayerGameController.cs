using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityStandardAssets.Cameras;

public class MultplayerGameController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    string playerNameTemp;
    public GameObject playerController;
    public GameObject myPlayer;
    public Transform[] spawnPlayer;
    public GameObject myCamera;

    PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        myCamera = Instantiate(myCamera) as GameObject;
    }
    void Start()
    {
        playerNameTemp = PhotonNetwork.NickName;

        int i = Random.Range(0, spawnPlayer.Length);
        GameObject player = PhotonNetwork.Instantiate(myPlayer.name, spawnPlayer[i].position, spawnPlayer[i].rotation, 0);

        player.gameObject.name = playerNameTemp;
        player.GetComponentsInChildren<InformacoesPlayerMultiplayer>()[0].nomeJogador.text = playerNameTemp;

        // Configura o nome do player
        playerController = GameObject.Find(playerNameTemp + "/Player");
        playerController.gameObject.name = playerNameTemp;
        //myCamera.GetComponent<CameraGTA>().SetTarget(playerController.transform);

    }
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.Log("OnDesconnected: " + cause.ToString());
        //yield return new WaitForSeconds(4.0f);
        SceneManager.LoadScene(0);
    }
}
