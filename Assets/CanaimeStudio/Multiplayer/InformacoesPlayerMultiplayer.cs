using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class InformacoesPlayerMultiplayer : MonoBehaviour
{
    PhotonView photonView;
    public Text nomeJogador;
    public GameObject ui;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if(photonView.IsMine) {
            ui.SetActive(false);
        } else {
            Debug.LogError(PhotonNetwork.NickName);
            nomeJogador.text = photonView.Owner.NickName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
