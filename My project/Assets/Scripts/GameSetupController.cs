using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameSetupController : MonoBehaviour
{
    PhotonView myPV;
    Camera myCamera;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector2.zero, Quaternion.identity, 0);
    }
}
