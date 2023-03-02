using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapPanel : MonoBehaviour
{
    //gameobject meegeven van de map
    [SerializeField]
    GameObject map;

    //maak map visible

    public void OnEnable()
    {
        map.SetActive(true);
    }

    // maak map invisible
    public void OnDisable()
    {
        map.SetActive(false);
    }

    public void debugLog()
    {
        Debug.Log("test");
    }

    public void sabotage()
    {
        Debug.Log("Sabotage");
    }
}
