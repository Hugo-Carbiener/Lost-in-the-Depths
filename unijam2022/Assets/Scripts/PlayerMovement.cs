using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravityScale = 5f;
    [SerializeField] private LineRenderer line;
    private int facingDirection = 1;
    private float jumpForce;

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
        if (Input.GetAxis("Horizontal") > 0)
        {
            facingDirection = 1;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            facingDirection = -1;
        }
        transform.Translate(speed * Time.deltaTime * movement);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        // Laser
        if (Input.GetKey(KeyCode.E))
        {
            if (Physics.Raycast(transform.position, Vector3.right * facingDirection, out RaycastHit hit))
            {
                line.SetPosition(1, hit.point - transform.position);
                hit.transform.SendMessage("HitByRay");
            }
        }
        else
        {
            line.SetPosition(1, new Vector3(0, 0, 0));
        }
    }
}
