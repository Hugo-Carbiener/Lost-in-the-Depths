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

    [SerializeField] private GameObject mesh;
    private Animator animator;

    private void Start()
    {
        animator = mesh.GetComponent<Animator>();
        oxygenNetwork = GameObject.FindGameObjectWithTag("OxygenNetwork").GetComponent<OxygenNetwork>();
        Assert.IsNotNull(oxygenPylone);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isInPlacementMode) //if the player is in normal mode, he will enter placement mode by pressing Mouse0
        {
            if (oxygenNetwork.GetCurPylone() && PlayerManager._instance.resourcesInventory[ResourcesType.Pylons] > 0) //if the player is not in range or if he does not have enough pylons in his inventory, he cannot place an oxygen pylone
            {
                isInPlacementMode = true;
                StartCoroutine(PlacePylone());
            }
            else
            {
                // Play sound
            }
        }
    }

    /**
     * Function that will place the pylone accordingly on the ground
     */
    private IEnumerator PlacePylone()
    {
        PlayerManager._instance.resourcesInventory[ResourcesType.Pylons]--;
        animator.SetBool("IsKneeling", true);
        GetComponent<PlayerMovement>().enabled = false;
        new_pylone = Instantiate(oxygenPylone);
        new_pylone.transform.parent = gameObject.transform;
        oxygenNetwork.AddNewPylone(new_pylone);
        new_pylone.transform.localPosition = new Vector3(0, 0.7f, 1f);
        new_pylone.transform.parent = null;
        yield return new WaitForSeconds(2.51f);
        GetComponent<PlayerMovement>().enabled = true;
        animator.SetBool("IsKneeling", false);
        isInPlacementMode = false;
    }
}
