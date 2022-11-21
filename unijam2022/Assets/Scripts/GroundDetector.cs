using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private GameObject feet;

    public bool isGrounded { get; private set; }

    private void Awake()
    {
        isGrounded = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("JUMP FIX : GDECTOR : STAY called");
        RaycastHit hit;
        if (collision.gameObject.CompareTag("Grass") || collision.gameObject.CompareTag("Rock"))
        {
            if (Physics.Raycast(feet.transform.position, new Vector3(0, -1, 0), out hit, 1f) && (hit.transform.tag=="Grass" || hit.transform.tag == "Rock"))
            {
                isGrounded = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("JUMP FIX : GDECTOR : EXIT called");
        if (collision.gameObject.CompareTag("Grass") || collision.gameObject.CompareTag("Rock"))
        {
            isGrounded = false;
        }
    }
}
