using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject craftCanvas;

    private void OnTriggerStay(Collider other)
    {
        canvas.gameObject.SetActive(true);
        if (Input.GetKey(KeyCode.A))
        {
            Time.timeScale = 0;
            craftCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canvas.gameObject.SetActive(false);
    }
}