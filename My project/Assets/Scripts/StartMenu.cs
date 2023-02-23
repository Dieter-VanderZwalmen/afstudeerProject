using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Photon.Pun;


public class StartMenu : MonoBehaviour
{
    private QuickStartLobbyController quickStartLobbyController = new QuickStartLobbyController();

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonHowToPlay = root.Q<Button>("HowToPlay");
        Button buttonPlay = root.Q<Button>("Play");
        Button buttonSettings = root.Q<Button>("Settings");
        Button buttonSound = root.Q<Button>("Sound");
        Button buttonQuit = root.Q<Button>("Quit");

        buttonHowToPlay.clickable.clicked += () => HowToPlay();
        buttonPlay.clickable.clicked += () => OnOnlineClick();
        buttonSettings.clickable.clicked += () => Settings();
        buttonSound.clickable.clicked += () => Sound();
        buttonQuit.clickable.clicked += () =>  Quit();
    }

     public void OnOnlineClick()
    {
        // Handle Online button click
        Debug.Log("Online button clicked");
        this.quickStartLobbyController.QuickStart();
    }
    

    public void HowToPlay()
    {
        // Handle HowToPlay button click
        Debug.Log("HowToPlay button clicked");
        SceneManager.LoadScene("HowToPlay");
    }

    public void Settings()
    {
        // Handle Settings button click
        Debug.Log("Settings button clicked");
        SceneManager.LoadScene("Settings");
    }

    public void Sound()
    {
        // Handle Sound button click
        Debug.Log("Sound button clicked");
        SceneManager.LoadScene("Sound");
    }

    // public void Local(int sceneIndex)
    // {
    //     SceneManager.LoadScene(sceneIndex);
    // }
    // public void Online(int sceneIndex)
    // {
    //     SceneManager.LoadScene(sceneIndex);
    // }
    // public void HowToPlayint(int sceneIndex)
    // {
    //     SceneManager.LoadScene(sceneIndex);
    // }
    public void Quit()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }

    // public void SoundEnable()
    // {
    //     AudioListener.pause = false;
    // }
    // public void SoundDisable()
    // {
    //     AudioListener.pause = true;
    // }
    // public void Settings(int sceneIndex)
    // {
    //     SceneManager.LoadScene(sceneIndex);
    // }
}