using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class VotingScreen : MonoBehaviour
{
    [SerializeField] private Text _playerNameText;
    [SerializeField] private Text _statusText;

    private int _actorNumber;

    public int ActorNumber{
        get { return _actorNumber; }
    }

    private Button _voteButton;
    private VotingManager _votingManager;

    private void awake()
    {
        _voteButton = GetComponentInChildren<Button>();
        _voteButton.onClick.AddListener(OnVotePressed);
    }

    private void OnVotePressed()
    {
        _votingManager.CastVote(_actorNumber);
    }

    public void Initialize(VotingManager votingManager, AU_PlayerController player)
    {
        _actorNumber = player.ActorNumber;
        _playerNameText.text = player.NickName;
        _statusText.text = "Not Decided";
        _votingManager = votingManager;
    }

    public void updateStatus(string status)
    {
        _statusText.text = status;
    }

    public void ToggleButton(bool isInteractable)
    {
        _voteButton.interactable = isInteractable;
    }

}