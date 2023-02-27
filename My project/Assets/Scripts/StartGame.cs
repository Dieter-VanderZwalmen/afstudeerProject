using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Photon.Pun;

public class StartGame : MonoBehaviour
{
    [SerializeField] GameObject customizeMenu;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonGarderobe = root.Q<Button>("Garderobe");
        Label hostLabel = root.Q<Label>("Host");

        buttonGarderobe.clickable.clicked += () => CharacterCustomization();
        hostLabel.text = "Welkom " + PhotonNetwork.NickName;
    }

    public void CharacterCustomization()
    {
        // Handle HowToPlay button click
        Debug.Log("CharacterCostumization button clicked");
        customizeMenu.SetActive(true);
    }
    
    public void Quit()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }
}