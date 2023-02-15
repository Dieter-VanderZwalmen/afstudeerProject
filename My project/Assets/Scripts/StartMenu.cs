using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
     public void OnOnlineClick()
    {
        // Handle Online button click
        SceneManager.LoadScene("MainMenu");
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
    // public void Quit()
    // {
    //     Application.Quit();
    // }

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