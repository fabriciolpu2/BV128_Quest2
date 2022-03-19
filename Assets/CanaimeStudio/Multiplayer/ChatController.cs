using UnityEngine;
using Photon.Voice.Unity;
public class ChatController : MonoBehaviour
{

    public GameObject voiceConfig;
    //public bool mic;
    public bool radio;
    public VoiceConnection voiceConnection;

    private void Awake()
    {
        voiceConnection = this.GetComponent<VoiceConnection>();
        
    }

    public void ToggleTransmit(bool mic)
    {
        
        voiceConnection.PrimaryRecorder.TransmitEnabled = mic;        
    }    
}
