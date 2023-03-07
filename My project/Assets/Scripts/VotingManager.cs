using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.IO;

public class VotingManager : MonoBehaviour
{
    public static VotingManager Instance;
    private static AU_GameController gameController = new AU_GameController();
    PhotonView myPV;

    [SerializeField] private VotePlayerItem _votePlayerItemPrefab;
    [SerializeField] private Transform _votePlayerItemContainer;
    [SerializeField] private Button _skipVoteButton;

    [HideInInspector] private bool HasAlreadyVoted;

    private List<VotePlayerItem> _playersList = new List<VotePlayerItem>();
    private List<int> _reportedBodiesList = new List<int>();
    private List<int> _playersThatVotedList = new List<int>();
    private List<int> _playersThatHaveBeenVotedList = new List<int>();
    private List<int> _playersThatHaveBeenKickedList = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        myPV = GetComponent<PhotonView>();
        DeadBodyReported(AU_PlayerController.gameController.bodiesFoundActorNumber[AU_PlayerController.gameController.bodiesFoundActorNumber.Count - 1]);
    }

    public bool BodyReported(int actorNumber)
    {
        return _reportedBodiesList.Contains(actorNumber);
    }

    public void AddToReportedList(int actorNumber)
    {
        _reportedBodiesList.Add(actorNumber);
    }

    public void DeadBodyReported(int actorNumber)
    {   
        //myPV.RPC("RPC_DeadBodyReported", RpcTarget.All, actorNumber);
        RPC_DeadBodyReported(actorNumber);
    }
    
    //[PunRPC]
    void RPC_DeadBodyReported(int actorNumber)
    {   
        _reportedBodiesList.Add(actorNumber);
        _playersThatHaveBeenVotedList.Clear();
        _playersThatVotedList.Clear();
        HasAlreadyVoted = false;
        PopulatePlayerList();
        // if the photonNetwork.localplayer is dead, then disable all butons
        if (_reportedBodiesList.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            ToggleAllButtons(false);
        }
    }

    public void PopulatePlayerList()
    {
        //clear the list
        for (int i = 0; i < _playersList.Count; i++)
        {
            Destroy(_playersList[i].gameObject);
        }

        _playersList.Clear();
        //create the list of vote player items and write dead as status for deadplayers and toggle buttons to false for the deadplayers
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            VotePlayerItem votePlayerItem = Instantiate(_votePlayerItemPrefab, _votePlayerItemContainer);
            votePlayerItem.Initialize(this, player);
            if (_reportedBodiesList.Contains(player.ActorNumber))
            {
                votePlayerItem.updateStatus("Dead");
                votePlayerItem.ToggleButton(false);
            }
            _playersList.Add(votePlayerItem);
        }
    }

    //toggleAllButtons
    public void ToggleAllButtons(bool isInteractable)
    {
        _skipVoteButton.interactable = isInteractable;
        foreach (VotePlayerItem votePlayerItem in _playersList)
        {
            votePlayerItem.ToggleButton(isInteractable);
        }
    }

    public void CastVote(int actorNumber)
    {
        if (HasAlreadyVoted)
        {
            return;
        }
        HasAlreadyVoted = true;
        ToggleAllButtons(false);
        myPV.RPC("RPC_CastPlayerVote", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, actorNumber);
    }

    //rpc function for casting vote
    [PunRPC]
    public void RPC_CastPlayerVote(int voterActorNumber, int votedActorNumber)
    {
        Debug.Log("voterActorNumber: " + voterActorNumber);
        Debug.Log("votedActorNumber: " + votedActorNumber);

        foreach (VotePlayerItem votePlayerItem in _playersList)
        {
            if (votePlayerItem.GetActorNumber == voterActorNumber)
            {
                votePlayerItem.updateStatus("Voted");
            }
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        // log the player that just voted / and who voted for
        if (!_playersThatVotedList.Contains(voterActorNumber))
        {
            _playersThatVotedList.Add(voterActorNumber);
            _playersThatHaveBeenVotedList.Add(votedActorNumber);
        }

        Debug.Log("_allDeadBodiesList.Count: " + _reportedBodiesList.Count);
        Debug.Log("_playersThatHaveBeenKickedList.Count: " + _playersThatHaveBeenKickedList.Count);
        Debug.Log("photonNetwork.CurrentRoom.PlayerCount: " + PhotonNetwork.CurrentRoom.PlayerCount);

        int votingPlayers = PhotonNetwork.CurrentRoom.PlayerCount - _playersThatHaveBeenKickedList.Count - _reportedBodiesList.Count;

        if (_playersThatVotedList.Count < votingPlayers)
        {
            return;
        }

        //count all votes
        Dictionary<int, int> votes = new Dictionary<int, int>();
        foreach (int player in _playersThatHaveBeenVotedList)
        {
            if (votes.ContainsKey(player))
            {
                votes[player]++;
            }
            else
            {
                votes.Add(player, 1);
            }
        }

        int mostVotedPlayer = -1;
        int mostVotes = int.MinValue;

        foreach (KeyValuePair<int, int> vote in votes)
        {
            if (vote.Value > mostVotes)
            {
                mostVotedPlayer = vote.Key;
                mostVotes = vote.Value;
            }
        }

        //end voting session
        if (mostVotes >= votingPlayers/2)
        {
            _playersThatHaveBeenKickedList.Add(mostVotedPlayer);
            myPV.RPC("RPC_KickPlayer", RpcTarget.All, mostVotedPlayer);
        }
    }  

    [PunRPC]
    public void RPC_KickPlayer(int actorNumber)
    {
        string playerName = string.Empty;
        foreach (var player in _playersList)
        {
            if (player.GetActorNumber == actorNumber)
            {
                playerName = player.GetPlayerName;
                break;
            }
        }
        string message = actorNumber == -1 ? "No one was voted out" : playerName + " was voted out";
        PhotonNetwork.LoadLevel("Game");
    }
}
