using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Menu_Tutorial : MonoBehaviour
{
    public GameObject cineVR; 

    public GameObject cine3p;


    public GameObject fala_inicial;
    public GameObject opcoes;
    public GameObject falas3P;
    public GameObject falasVr;




    public void Sair()
    {
        this.gameObject.GetComponent<Canvas>().enabled = false;
        this.gameObject.GetComponent<AudioSource>().Stop();
        cine3p.SetActive(false);
        cineVR.SetActive(false);
        cine3p.GetComponent<PlayableDirector>().Stop();
        cineVR.GetComponent<PlayableDirector>().Stop();
        fala_inicial.SetActive(true);
        opcoes.SetActive(true);
        falas3P.SetActive(false);
        falasVr.SetActive(false);
    }

    public void Tutorial_3p()
    {
        cine3p.SetActive(true);
        fala_inicial.SetActive(false);
        opcoes.SetActive(false);
        falas3P.SetActive(true);
        falasVr.SetActive(false);
        this.gameObject.GetComponent<AudioSource>().Stop();


    }
    public void Tutorial_Vr()
    {
        cineVR.SetActive(true);
        fala_inicial.SetActive(false);
        opcoes.SetActive(false);
        falas3P.SetActive(false);
        falasVr.SetActive(true);
        this.gameObject.GetComponent<AudioSource>().Stop();

    }
}
