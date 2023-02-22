using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int multiplayerSceneIndex; // Number of the build index for the multiplayer scene

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room");
        StartGame();
    }

    void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting game");
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
    }
}
