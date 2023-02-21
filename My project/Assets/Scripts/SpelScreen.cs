using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class SpelScreen : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonSettings = root.Q<Button>("Settings");
        Button buttonMap = root.Q<Button>("Map");
        Button buttonReport = root.Q<Button>("Report");
        Button buttonUse = root.Q<Button>("Use");
        Button buttonKill = root.Q<Button>("Kill");

        buttonSettings.clickable.clicked += () => Settings();
        buttonMap.clickable.clicked += () => Map();
        buttonReport.clickable.clicked += () => Report();
        buttonUse.clickable.clicked += () => Use();
        buttonKill.clickable.clicked += () => Kill();
    }

    public void Settings()
    {
        // Handle Settings button click
        Debug.Log("Settings button clicked");
        SceneManager.LoadScene("Settings");
    }

    public void Map()
    {
        // Handle Map button click
        Debug.Log("Map button clicked");
        SceneManager.LoadScene("Map");
    }

    public void Report()
    {
        // Handle Report button click
        Debug.Log("Report button clicked");
    }

    public void Use()
    {
        // Handle Use button click
        Debug.Log("Use button clicked");
    }

    public void Kill()
    {
        // Handle Kill button click
        Debug.Log("Kill button clicked");
    }
}