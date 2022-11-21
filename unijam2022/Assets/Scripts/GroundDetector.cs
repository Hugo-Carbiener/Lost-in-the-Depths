using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool isGrounded { get; private set; }

    private void Awake()
    {
        isGrounded = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("JUMP FIX : GDECTOR : STAY called");
        if (collision.gameObject.CompareTag("Grass") || collision.gameObject.CompareTag("Rock"))
        {
            isGrounded = true;
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
