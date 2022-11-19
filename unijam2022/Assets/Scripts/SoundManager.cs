using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource natureSource;
    [SerializeField] private AudioSource depthSource;

    private void Update()
    {
        if (PlayerManager._instance.gameObject.transform.position.y < -4f)
        {
            natureSource.volume -= Time.deltaTime * 0.1f;
            depthSource.volume += Time.deltaTime * 0.1f;
        } 
        else if (PlayerManager._instance.gameObject.transform.position.y > -4f)
        {
            depthSource.volume -= Time.deltaTime * 0.1f;
            natureSource.volume += Time.deltaTime * 0.1f;
        }
    }
}
