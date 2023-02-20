using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        //Connect to the Photon server
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        //Called when connected to the master server
        Debug.Log("Connected to the master server");
        //create a room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, null);
    }

    public override void OnJoinedRoom()
    {
        //Called when we join a room
        Debug.Log("Joined a room");
        //Spawn a player
        PhotonNetwork.Instantiate("Player", Vector2.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
