using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AU_GameController : MonoBehaviour
{
    public PhotonView myPV;
    public static List<AU_PlayerController> allPlayers = new List<AU_PlayerController>();
    public static Player[] alivePlayers = PhotonNetwork.PlayerList;
    public List<int> bodiesFoundActorNumber = new List<int>();
    int whichPlayerIsImposter;

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            PickImposter();
        }
        myPV.RPC("RPC_AddAllPlayersToAllPlayersList", RpcTarget.All);
    }

    public void AddPlayer(AU_PlayerController player)
    {
        allPlayers.Add(player);
    }
    
    public void AddToBodiesFoundActorNumber(int actorNumber)
    {
        bodiesFoundActorNumber.Add(actorNumber);
    }

    public void AddToAlivePlayerList(Player player)
    {
        Debug.Log("in add alive player");
        Debug.Log("player: " + player);
        List<Player> tempList = new List<Player>(alivePlayers);
        tempList.Add(player);
        alivePlayers = tempList.ToArray();
        Debug.Log("added alive players");
        Debug.Log("added alive player:" + alivePlayers[0]);
    }

    public void RemoveFromAlivePlayerList(Player player)
    {
        List<Player> tempList = new List<Player>(alivePlayers);
        tempList.Remove(player);
        alivePlayers = tempList.ToArray();
    }

    public List<int> GetBodiesFoundActorNumber()
    {
        return bodiesFoundActorNumber;
    }

    void PickImposter()
    {
        whichPlayerIsImposter = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
        myPV.RPC("RPC_SyncImposter", RpcTarget.All, whichPlayerIsImposter);
        Debug.Log("Imposter " + whichPlayerIsImposter);
    }

    [PunRPC]
    void RPC_SyncImposter(int playerNumber)
    {
        whichPlayerIsImposter = playerNumber;
        AU_PlayerController.localPlayer.BecomeImposter(whichPlayerIsImposter);
    }

    [PunRPC]
    void RPC_AddAllPlayersToAllPlayersList()
    {
        AU_PlayerController.localPlayer.AddToAllPlayersList();
    }
}