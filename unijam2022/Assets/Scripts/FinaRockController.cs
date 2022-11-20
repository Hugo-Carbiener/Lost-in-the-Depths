using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinaRockController : MonoBehaviour
{
    private AudioSource audioLaser;
    private void Start()
    {
        audioLaser = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        StartCoroutine(Victory());
    }

    private IEnumerator Victory()
    {
        GetComponent<PlayerMovement>().enabled = false;
        audioLaser.Stop();
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>().TriggerVictory();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenu");
    }
}
