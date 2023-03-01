using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class VotingManager : MonoBehaviour
{
    public static VotingManager Instance;
    
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

    public bool BodyReported(int actorNumber)
    {
        return _reportedBodiesList.Contains(actorNumber);
    }

    public void DeadBodyReported(PhotonView pv, int actorNumber)
    {
        _reportedBodiesList.Add(actorNumber);
        _playersThatHaveBeenVotedList.Clear();
        _playersThatVotedList.Clear();
        HasAlreadyVoted = false;
        pv.RPC("RPC_DeadBodyReported", RpcTarget.All, actorNumber); 
    }

    private void PopulatePlayerList()
    {
        //clear the list
        for (int i = 0; i < _playersList.Count; i++)
        {
            Destroy(_playersList[i].gameObject);
        }

        _playersList.Clear();

        //populate the list
        foreach (var player in AU_GameController.allPlayers)
        {
            VotePlayerItem votePlayerItem = Instantiate(_votePlayerItemPrefab, _votePlayerItemContainer);
            votePlayerItem.Initialize(this, player.GetComponent<AU_PlayerController>());
            if (player.GetComponent<AU_PlayerController>().isDead)
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

    public void CastVote(int actorNumber, PhotonView pv)
    {
        if (HasAlreadyVoted)
        {
            return;
        }
        HasAlreadyVoted = true;
        ToggleAllButtons(false);
        pv.RPC("RPC_CastPlayerVote", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, actorNumber, pv);
    }

    //rpc function for casting vote
    [PunRPC]
    public void RPC_CastPlayerVote(int voterActorNumber, int votedActorNumber, PhotonView pv)
    {
        int remainingplayers = AU_GameController.allPlayers.Count - _playersThatVotedList.Count - _playersThatHaveBeenKickedList.Count;

        foreach (VotePlayerItem votePlayerItem in _playersList)
        {
            if (votePlayerItem.GetActorNumber == votedActorNumber)
            {
                votePlayerItem.updateStatus("Voted");
            }
        }

        // log the player that just voted / and who voted for
        if (!_playersThatVotedList.Contains(voterActorNumber))
        {
            _playersThatVotedList.Add(voterActorNumber);
            _playersThatHaveBeenVotedList.Add(votedActorNumber);
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if (_playersThatVotedList.Count < remainingplayers)
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
        if (mostVotes >= remainingplayers/2)
        {
            _playersThatHaveBeenKickedList.Add(mostVotedPlayer);
            pv.RPC("RPC_KickPlayer", RpcTarget.All, mostVotedPlayer);
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
