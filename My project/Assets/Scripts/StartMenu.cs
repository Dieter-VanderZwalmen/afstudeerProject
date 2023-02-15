using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// how to import stylesheet from another file
// https://stackoverflow.com/questions/1000000/how-to-import-stylesheet-from-another-file


public class StartMenu : MonoBehaviour
{
     public void OnOnlineClick()
    {
        // Handle Online button click
        Debug.Log("Online button clicked");
        SceneManager.LoadScene("MainMenu");
    }

    public void HowToPlay()
    {
        // Handle HowToPlay button click
        Debug.Log("HowToPlay button clicked");
        SceneManager.LoadScene("HowToPlay");
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