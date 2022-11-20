using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Canvas loadingScreen;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        loadingScreen.gameObject.SetActive(false);
    }

    public void OnPlay()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        Time.timeScale = 1;
        loadingScreen.gameObject.SetActive(true);
        AsyncOperation loadingLevel = SceneManager.LoadSceneAsync("MVP");
        while (!loadingLevel.isDone)
        {
            yield return null;
        }
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
