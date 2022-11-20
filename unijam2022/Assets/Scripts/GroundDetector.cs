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
        if (collision.gameObject.CompareTag("Grass") || collision.gameObject.CompareTag("Rock"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grass") || collision.gameObject.CompareTag("Rock"))
        {
            isGrounded = false;
        }
    }
}
