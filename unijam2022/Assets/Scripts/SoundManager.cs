using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource natureSource;
    [SerializeField] private AudioSource depthSource;
    [SerializeField] private AudioSource depthMusic;
    [SerializeField] private float depthTransition = -4f;

    private void Update()
    {
        if (PlayerManager._instance.gameObject.transform.position.y < depthTransition)
        {
            natureSource.volume -= Time.deltaTime * 0.1f;
            depthSource.volume += Time.deltaTime * 0.1f;
            depthMusic.volume += Time.deltaTime * 0.1f;
        } 
        else if (PlayerManager._instance.gameObject.transform.position.y > depthTransition)
        {
            depthSource.volume -= Time.deltaTime * 0.1f;
            depthMusic.volume -= Time.deltaTime * 0.1f;
            natureSource.volume += Time.deltaTime * 0.1f;
        }
        else if (PlayerManager._instance.gameObject.transform.position.y < -58f)
        {
            depthMusic.volume -= Time.deltaTime * 0.1f;
        }
        else if (PlayerManager._instance.gameObject.transform.position.y < depthTransition && PlayerManager._instance.gameObject.transform.position.y > -58f)
        {
            depthMusic.volume += Time.deltaTime * 0.1f;
        }
    }
}
