using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class VotePlayerItem : MonoBehaviour
{
    [SerializeField] private GameObject _playerNameText;
    [SerializeField] private GameObject _statusText;

    //lol 2
    private int _actorNumber;

    public int GetActorNumber{
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
        _actorNumber = player.actorNumber;
        _playerNameText.GetComponent<Text>().text = player.nickName;
        _statusText.GetComponent<Text>().text = "Not Decided";
        _votingManager = votingManager;
    }

    public void updateStatus(string status)
    {
        _statusText.GetComponent<Text>().text = status;
    }

    public void ToggleButton(bool isInteractable)
    {
        _voteButton.interactable = isInteractable;
    }

}