using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Canvas loadingScreen;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private string[] loadingTexts;

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
        loadingText.text = loadingTexts[Random.Range(0,loadingTexts.Length-1)];
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
