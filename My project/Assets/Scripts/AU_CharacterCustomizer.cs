using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AU_CharacterCustomizer : MonoBehaviour
{
    [SerializeField] Color[] allColors;

    private int colorIndexNickname = 9;

    public void SetColor(int colorIndex)
    {
        colorIndexNickname = colorIndex;
        AU_PlayerController.localPlayer.SetColor(allColors[colorIndex]);
        SetColorAsNickname();
    }

    public void SetColorAsNickname()
    {
        string colorName = "";
        switch (colorIndexNickname)
        {
            case 0:
                colorName = "Roos";
                break;
            case 1:
                colorName = "Donker blauw";
                break;
            case 2:
                colorName = "Rood";
                break;
            case 3:
                colorName = "Groen";
                break;
            case 4:
                colorName = "Geel";
                break;
            case 5:
                colorName = "Zwart";
                break;
            case 6:
                colorName = "Licht blauw";
                break;
            case 7:
                colorName = "Bruin";
                break;
            case 8:
                colorName = "Oranje";
                break;
            default:
                colorName = "Wit";
                break;
        }
        AU_PlayerController.localPlayer.SetColorAsNickname(colorName);
    }

    public void NextScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}