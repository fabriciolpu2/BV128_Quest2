using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasInformacoesNPC : MonoBehaviour
{
    public RawImage image;
    public Text nome;
    public Text informacoes;

    //public Player player;

    void Start()
    {
       // player = FindObjectOfType<GameManager>().playerAtivo;     
    }

    public void DesabilitaCanvas()
    {
        Debug.Log("E pra desabilitar");
        //if (player == null)
        //{
        //    player = FindObjectOfType<GameManager>().playerAtivo;
        //}
        //else
        //{
        //    Debug.Log("E pra desabilitar");
        //    //player.playerController.SendMessage("StopWalk");
        //}
    }   

}
