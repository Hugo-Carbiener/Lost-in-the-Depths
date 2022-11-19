using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 *  Component used by the Player to place pylones
 */
public class PlayerPyloneController : MonoBehaviour
{
    private OxygenNetwork oxygenNetwork;
    [SerializeField] private GameObject oxygenPylone;

    private bool isInPlacementMode;
    private GameObject new_pylone;

    private void Start()
    {
        oxygenNetwork = GameObject.FindGameObjectWithTag("OxygenNetwork").GetComponent<OxygenNetwork>();
        Assert.IsNotNull(oxygenPylone);
        isInPlacementMode = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !isInPlacementMode)
        {
            StartPlacementPylone();
        }
        if (isInPlacementMode) 
        {
            if ((oxygenNetwork.GetLastPylone().transform.position - gameObject.transform.position).magnitude <= oxygenNetwork.GetLastPylone().GetComponent<OxygenPyloneController>().maxPyloneDistance)
            {
                Debug.Log("PLACEMENT : connected");
            }
            else
            {
                Debug.Log("PLACEMENT : NOT connected");
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                PlacePylone();
            }
        }
    }

    private void StartPlacementPylone()
    {
        isInPlacementMode=true;
        new_pylone = Instantiate(oxygenPylone);
        new_pylone.transform.parent = gameObject.transform;
        new_pylone.transform.localPosition = new Vector3(0, 1.5f, 0);
        
    }

    private void PlacePylone()
    {
        oxygenNetwork.AddNewPylone(new_pylone);
        new_pylone.transform.localPosition = new Vector3(0, 0, 0);
        new_pylone.transform.parent = null;
        isInPlacementMode = false;
    }
}
