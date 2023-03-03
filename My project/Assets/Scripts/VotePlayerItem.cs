using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using TMPro;

public class VotePlayerItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerNameText;
    [SerializeField] private TextMeshProUGUI _statusText;

    private int _actorNumber;

    public int GetActorNumber{
        get { return _actorNumber; }
    }

    public string GetPlayerName{
        get { return _playerNameText.text; }
    }

    private Button _voteButton;
    private VotingManager _votingManager;

    private void Awake()
    {
        _voteButton = GetComponentInChildren<Button>();
        Debug.Log("vote button in _voteButton: " + _voteButton);
        _voteButton.onClick.AddListener(OnVotePressed);
    }

    private void OnVotePressed()
    {
        _votingManager.CastVote(_actorNumber);
    }

    public void Initialize(VotingManager votingManager, Player player)
    {
        _actorNumber = player.ActorNumber;
        _playerNameText.text = player.NickName;
        _statusText.text = "Not Decided";
        _votingManager = votingManager;
    }

    public void updateStatus(string status)
    {
        Debug.Log("status: " + status);
        _statusText.text = status;
    }

    public void ToggleButton(bool isInteractable)
    {
        Debug.Log("vote button: " + _voteButton);
        _voteButton.interactable = isInteractable;
    }

}