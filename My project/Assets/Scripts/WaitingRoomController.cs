using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class WaitingRoomController : MonoBehaviour
{
    PhotonView myPV;

    [SerializeField] float timeToStart;
    float timerToStart;
    bool readyToStart;

    [SerializeField] GameObject startButton;
    [SerializeField] TextMeshProUGUI countDownDisplay;

    [SerializeField] int nextLevel;

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        timerToStart = timeToStart;       
    }

    // Update is called once per frame
    void Update()
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient);
            if(readyToStart)
            {
                timerToStart -= Time.deltaTime;
                countDownDisplay.text = ((int)timerToStart).ToString();
            }
            else
            {
                timerToStart = timeToStart;
                countDownDisplay.text = "TEST";
            }
        if (PhotonNetwork.IsMasterClient)
        {
            if (timerToStart <= 0)
            {
                timerToStart = 100;
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.LoadLevel(nextLevel);
            }
        }
    }

    public void Play()
    {
        Debug.Log("Play button clicked");
        if (PhotonNetwork.IsMasterClient)
        {
            myPV.RPC("RPC_Play", RpcTarget.All);
        }
    }
    
    [PunRPC]
    void RPC_Play()
    {
        readyToStart = !readyToStart;
    }
}