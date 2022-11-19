using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnPlay()
    {
        SceneManager.LoadScene("MVP");
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
