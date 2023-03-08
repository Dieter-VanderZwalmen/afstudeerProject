using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MyPhotonPlayer : MonoBehaviour
{
    PhotonView myPV;
    GameObject myPlayerAvatar;

    Player[] allPlayers;
    int myNumberInRoom;

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();

        this.myNumberInRoom = PhotonNetwork.LocalPlayer.ActorNumber;

        if (myPV.IsMine)
        {
            Debug.Log("myNumberInRoom" + myNumberInRoom);
            Debug.Log("instantiating my player");
            myPlayerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AU_Player"), AU_SpawnPoints.instance.spawnPoints[myNumberInRoom].position, Quaternion.identity);
        }
    }


}