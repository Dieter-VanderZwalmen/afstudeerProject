using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class mapPanel : MonoBehaviour
{
    //gameobject meegeven van de map
    [SerializeField] GameObject map;

    [SerializeField] GameObject doors;

    //myPv
    PhotonView myPV;


    //start
    private void Start()
    {
        myPV = GetComponent<PhotonView>();
    }

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


    
    public void DisableLights()
    {
        Debug.Log("Sabotaging the lights");
        myPV.RPC("RPC_DisableLights", RpcTarget.All);

    }

    [PunRPC]
    void RPC_DisableLights()
    {
        Debug.Log("Reducing vision");

        AU_PlayerController.localPlayer.ReduceVision();
    }

    public void Doors() {
        Debug.Log("Doors");
       //invoke
       doors.SetActive(true);
       Invoke("openDoors",5f); //na 5 seconden doe deuren terug open
    }

    private void openDoors(){
        doors.SetActive(false);
    }


 
}
