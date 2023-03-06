using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class SpelScreen : MonoBehaviour
{
    public static AU_PlayerController localPlayer = AU_PlayerController.localPlayer;
    private Button buttonKill;

    [SerializeField] GameObject miniMap;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonSettings = root.Q<Button>("Settings");
        Button buttonMap = root.Q<Button>("Map");
        Button buttonReport = root.Q<Button>("Report");
        Button buttonUse = root.Q<Button>("Use");
        buttonKill = root.Q<Button>("Kill");

        //buttonSettings.clickable.clicked += () => Settings();
        buttonMap.clickable.clicked += () => Map();
        buttonReport.clickable.clicked += () => Report();
        buttonUse.clickable.clicked += () => Use();
        buttonKill.clickable.clicked += () => Kill();
        
    }

    private void Update()
    {
        if (AU_PlayerController.localPlayer.isImposter)
        {
            buttonKill.visible = true;
        }
        else
        {
            buttonKill.visible = false;
        }
    }

    /*public void Settings()
    {
        // Handle Settings button click
        Debug.Log("Settings button clicked");
        SceneManager.LoadScene("Settings");
    }*/

    public void Map()
    {
        Debug.Log("Map button clicked");
        miniMap.SetActive(true);
    }

    public void Report()
    {
        // Handle Report button click
        Debug.Log("Report button clicked");
        AU_PlayerController localPlayer = AU_PlayerController.localPlayer;
        localPlayer.ReportBody();
    }

    public void Use()
    {
        // Handle Use button click
        Debug.Log("Use button clicked");
        AU_PlayerController localPlayer = AU_PlayerController.localPlayer;
        localPlayer.Interact();
    }

    public void Kill()
    {
        // Handle Kill button click
        Debug.Log("Kill button clicked");
        //how to get the local player from the scene?
        AU_PlayerController localPlayer = AU_PlayerController.localPlayer;
        localPlayer.KillTarget();
    }
}