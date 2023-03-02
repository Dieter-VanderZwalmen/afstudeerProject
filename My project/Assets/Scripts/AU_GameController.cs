using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class AU_GameController : MonoBehaviour
{
    PhotonView myPV;

    [SerializeField] GameObject roleScreen;
    [SerializeField] TextMeshProUGUI roleText;

    int whichPlayerIsImposter;

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            PickImposter();
        }
        Invoke("ShowRole", 1f);
    }
    void PickImposter()
    {
        whichPlayerIsImposter = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
        myPV.RPC("RPC_SyncImposter", RpcTarget.All, whichPlayerIsImposter);
        Debug.Log("Imposter " + whichPlayerIsImposter);
    }
    
    void ShowRole()
    {
        roleScreen.SetActive(true);
        if (AU_PlayerController.localPlayer.isImposter)
        {
            roleText.text = "BEDRIEGER";
            roleText.color = Color.red;
        }
        else
        {
            roleText.text = "TEAMGENOOT";
            roleText.color = Color.green;
        }
    }

    [PunRPC]
    void RPC_SyncImposter(int playerNumber)
    {
        whichPlayerIsImposter = playerNumber;
        AU_PlayerController.localPlayer.BecomeImposter(whichPlayerIsImposter);
    }
}