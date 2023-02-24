using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Photon.Pun;

public class StartGame : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonGarderobe = root.Q<Button>("Garderobe");
        Button buttonPlay = root.Q<Button>("Play");
        Label hostLabel = root.Q<Label>("Host");

        buttonGarderobe.clickable.clicked += () => CharacterCustomization();
        hostLabel.text = "Welkom " + PhotonNetwork.NickName;

        if (!PhotonNetwork.IsMasterClient)
            buttonPlay.SetEnabled(false);
            //dont show host label
        
        buttonPlay.clickable.clicked += () => OnPlayClick();
    }

     public void OnPlayClick()
    {
        // Handle Online button click
        Debug.Log("Online button clicked");
        if (PhotonNetwork.IsMasterClient)
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