using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float speed = 3.5f;

    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravityScale = 5f;
    private float jumpForce;

    [SerializeField] private LineRenderer line;
    private Vector3 forwardVector;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y);
    }

    private void FixedUpdate()
    {
        rb.AddForce((gravityScale - 1) * rb.mass * Physics.gravity);
    }

    private void Update()
    {
        // Horizontal movement
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        transform.Translate(speed * Time.deltaTime * movement);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        // Laser
        if (Input.GetKey(KeyCode.E) || Input.GetMouseButton(0))
        {
            forwardVector = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);
            forwardVector.z = 0;
            int minableLayerIndex = LayerMask.NameToLayer("Minable");
            if (minableLayerIndex != -1)
            {
                int layerMask = (1 << minableLayerIndex);

                if (Physics.Raycast(transform.position, forwardVector, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    line.SetPosition(1, hit.point - transform.position);
                    hit.transform.root.SendMessage("HitByRay");
                }
                else
                {
                    line.SetPosition(1, forwardVector);
                }
            }
        }
        else
        {
            line.SetPosition(1, new Vector3(0, 0, 0));
        }
    }
}
