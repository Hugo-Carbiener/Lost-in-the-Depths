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
    private float closeDistance, mediumDistance, farDistance;

    private bool isInPlacementMode;
    private GameObject new_pylone;

    private void Start()
    {
        oxygenNetwork = GameObject.FindGameObjectWithTag("OxygenNetwork").GetComponent<OxygenNetwork>();
        Assert.IsNotNull(oxygenPylone);
        isInPlacementMode = false;
        closeDistance = oxygenPylone.GetComponent<OxygenPyloneController>().maxPyloneDistance / 3.5f;
        mediumDistance = oxygenPylone.GetComponent<OxygenPyloneController>().maxPyloneDistance / 2;
        farDistance = oxygenPylone.GetComponent<OxygenPyloneController>().maxPyloneDistance / 1.5f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isInPlacementMode) //if the player is in normal mode, he will enter placement mode by pressing Mouse0
        {
            StartPlacementPylone();
        }
        if (isInPlacementMode) //in placement mode, we test wether the pylone is connected to the network and change its color accordingly
        {
            if ((oxygenNetwork.GetLastPylone().transform.position - gameObject.transform.position).magnitude <= oxygenNetwork.GetLastPylone().GetComponent<OxygenPyloneController>().maxPyloneDistance)
            {
                Debug.Log("PLACEMENT : connected");
                if((oxygenNetwork.GetLastPylone().transform.position - gameObject.transform.position).magnitude <= closeDistance)
                {
                    new_pylone.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else if ((oxygenNetwork.GetLastPylone().transform.position - gameObject.transform.position).magnitude <= mediumDistance)
                {
                    new_pylone.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
                else if ((oxygenNetwork.GetLastPylone().transform.position - gameObject.transform.position).magnitude <= farDistance)
                {
                    new_pylone.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }
            else
            {
                Debug.Log("PLACEMENT : NOT connected");
                new_pylone.GetComponent<MeshRenderer>().material.color = Color.black;
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                PlacePylone();
            }
        }
    }

    /**
     *  Function that will begin the placement mode
     */
    private void StartPlacementPylone()
    {
        isInPlacementMode=true;
        new_pylone = Instantiate(oxygenPylone);
        new_pylone.transform.parent = gameObject.transform;
        new_pylone.transform.localPosition = new Vector3(0, 1.5f, 0);
        
    }

    /**
     * Function that will place the pylone accordingly on the ground
     */
    private void PlacePylone()
    {
        oxygenNetwork.AddNewPylone(new_pylone);
        new_pylone.transform.localPosition = new Vector3(0, 0.7f, 1f);
        new_pylone.transform.parent = null;
        isInPlacementMode = false;
    }
}
