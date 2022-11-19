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
    private bool pyloneIsConnected;
    private GameObject new_pylone;

    private void Start()
    {
        oxygenNetwork = GameObject.FindGameObjectWithTag("OxygenNetwork").GetComponent<OxygenNetwork>();
        Assert.IsNotNull(oxygenPylone);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isInPlacementMode) //if the player is in normal mode, he will enter placement mode by pressing Mouse0
        {
            if (oxygenNetwork.GetCurPylone())
            {
                isInPlacementMode = true;
                StartCoroutine(PlacePylone());
            }
        }
    }

    /**
     * Function that will place the pylone accordingly on the ground
     */
    private IEnumerator PlacePylone()
    {
        new_pylone = Instantiate(oxygenPylone);
        new_pylone.transform.parent = gameObject.transform;
        oxygenNetwork.AddNewPylone(new_pylone);
        new_pylone.transform.localPosition = new Vector3(0, 0.7f, 1f);
        new_pylone.transform.parent = null;
        yield return new WaitForSeconds(0.1f);
        isInPlacementMode = false;
    }
}
