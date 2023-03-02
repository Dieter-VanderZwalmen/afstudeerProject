using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapPanel : MonoBehaviour
{
    //gameobject meegeven van de map
    [SerializeField] GameObject map;

    [SerializeField] GameObject doors;



    //import a script 


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
        //in the script Fov call the function ReduceVision
        //get Component AU_PLAYER
        
        //get Component Fov
        //Fov.ReduceVision();

    }

    public void CloseDoors() {
       StartCoroutine(Doors());

    }
    //coroutine to close doors
    //maakt dingen met wachten makkelijker
    IEnumerator Doors() {
        doors.SetActive(true);
        //wait 5seconds
        yield return new WaitForSeconds(5);
        doors.SetActive(false);
    }
}
