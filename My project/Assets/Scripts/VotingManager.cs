using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class VotingManager : MonoBehaviour
{
    public static VotingManager Instance;

    private List<int> _reportedBodiesList = new List<int>();
    [SerializeField] private VotePlayerItem _votePlayerItemPrefab;
    [SerializeField] private Transform _votePlayerItemContainer;
    private List<VotePlayerItem> _playersList = new List<VotePlayerItem>();

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
        foreach (var player in PhotonNetwork.PlayerList)
        {
            var votePlayerItem = Instantiate(_votePlayerItemPrefab, _votePlayerItemContainer);
            _playersList.Add(votePlayerItem);
        }
    }

    public void CastVote(int actorNumber)
    {

    }
}
