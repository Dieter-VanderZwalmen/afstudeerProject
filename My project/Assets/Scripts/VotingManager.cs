using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VotingManager : MonoBehaviour
{
    public static VotingManager Instance;

    private List<int> _reportedBodiesList = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddBodyToReportedList(int actorNumber)
    {
        _reportedBodiesList.Add(actorNumber);
    }

    public void CastVote(int actorNumber)
    {

    }
}
