using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class StartGame : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonGarderobe = root.Q<Button>("Garderobe");
        Button buttonPlay = root.Q<Button>("Play");

        buttonGarderobe.clickable.clicked += () => CharacterCustomization();
        buttonPlay.clickable.clicked += () => OnPlayClick();
    }

     public void OnPlayClick()
    {
        // Handle Online button click
        Debug.Log("Online button clicked");
        SceneManager.LoadScene("Game");
    }

    public void CharacterCustomization()
    {
        // Handle HowToPlay button click
        Debug.Log("CharacterCostumization button clicked");
        SceneManager.LoadScene("CharacterCustomization");
    }

    public void Quit()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }
}