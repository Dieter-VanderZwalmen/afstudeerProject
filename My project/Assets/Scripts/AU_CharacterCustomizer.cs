using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AU_CharacterCustomizer : MonoBehaviour
{
    [SerializeField] Color[] allColors;

    private int colorIndexNickname = 10;

    public void SetColor(int colorIndex)
    {
        colorIndexNickname = colorIndex;
        AU_PlayerController.localPlayer.SetColor(allColors[colorIndex]);
        SetColorAsNickname();
    }

    public void SetColorAsNickname()
    {   
        if (colorIndexNickname == 0)
        {
            AU_PlayerController.localPlayer.SetColorAsNickname("Roos");
        }
        else if (colorIndexNickname == 1)
        {
            AU_PlayerController.localPlayer.SetColorAsNickname("Donker blauw");
        }
        else if (colorIndexNickname == 2)
        {
            AU_PlayerController.localPlayer.SetColorAsNickname("Rood");
        }
        else if (colorIndexNickname == 3)
        {
            AU_PlayerController.localPlayer.SetColorAsNickname("Groen");
        }
        else if (colorIndexNickname == 4)
        {
            AU_PlayerController.localPlayer.SetColorAsNickname("Geel");
        }
        else if (colorIndexNickname == 5)
        {
            AU_PlayerController.localPlayer.SetColorAsNickname("Zwart");
        }
        else if (colorIndexNickname == 6)
        {
            AU_PlayerController.localPlayer.SetColorAsNickname("Licht blauw");
        }
        else if (colorIndexNickname == 7)
        {
            AU_PlayerController.localPlayer.SetColorAsNickname("Bruin");
        }
        else if (colorIndexNickname == 8)
        {
            AU_PlayerController.localPlayer.SetColorAsNickname("Oranje");
        }
        else {
            AU_PlayerController.localPlayer.SetColorAsNickname("Wit");
        }
    }

    public void NextScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}