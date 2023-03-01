using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using TMPro;

public class VotePlayerItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerNameText;
    [SerializeField] private TextMeshProUGUI _statusText;

    private int _actorNumber;
    private PhotonView _pv;

    public int GetActorNumber{
        get { return _actorNumber; }
    }

    public string GetPlayerName{
        get { return _playerNameText.GetComponent<Text>().text; }
    }

    private Button _voteButton;
    private VotingManager _votingManager;

    private void start()
    {
        _voteButton = GetComponentInChildren<Button>();
        Debug.Log("vote button in _voteButton: " + _voteButton);
        _voteButton.onClick.AddListener(OnVotePressed);
    }

    private void OnVotePressed()
    {
        _votingManager.CastVote(_actorNumber, _pv);
    }

    public void Initialize(VotingManager votingManager, AU_PlayerController player)
    {
        _actorNumber = player.actorNumber;
        Debug.Log("non component" + _playerNameText);
        _playerNameText.text = player.nickName;
        _statusText.text = "Not Decided";
        _votingManager = votingManager;
    }

    public void updateStatus(string status)
    {
        _statusText.text = status;
    }

    public void ToggleButton(bool isInteractable)
    {
        Debug.Log("vote button: " + _voteButton);
        _voteButton.interactable = isInteractable;
    }

}