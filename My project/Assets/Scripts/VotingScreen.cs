using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class VotingScreen : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonSkip = root.Q<Button>("Skip");

        buttonSkip.clickable.clicked += () => Skip();
    }

     public void Skip()
    {
        Debug.Log("Skip button clicked");
        SceneManager.LoadScene("Game");
    }
}