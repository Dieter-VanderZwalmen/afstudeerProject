using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class Setting : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonBack = root.Q<Button>("Back");

        buttonBack.clickable.clicked += () => Back();
    }

     public void Back()
    {
        // Handle Online button click
        Debug.Log("Back button clicked");
        SceneManager.LoadScene("StartMenu");
    }
}