using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class AU_GameController : MonoBehaviour
{
    public PhotonView myPV;
    public List<AU_PlayerController> allPlayers = new List<AU_PlayerController>();
    public Player[] alivePlayers = PhotonNetwork.PlayerList;
    public List<int> bodiesFoundActorNumber = new List<int>();
    
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
        List<Player> tempList = new List<Player>(alivePlayers);
        tempList.Add(player);
        alivePlayers = tempList.ToArray();
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

    /* [PunRPC]
    void RPC_AddAllPlayersToAllPlayersList()
    {
        AU_PlayerController.localPlayer.AddToAllPlayersList();
    } */
}