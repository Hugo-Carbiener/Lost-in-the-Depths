using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private GameObject mesh;
    private Animator animator;
    private AudioSource laserSound;

    [SerializeField] private float speed = 3.5f;
    private bool isFacingRight = true;

    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravityScale = 5f;
    private float jumpForce;
    public bool isGrounded;

    [SerializeField] private LineRenderer line;
    private Vector3 forwardVector;

    private void Start()
    {
        animator = mesh.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        laserSound = gameObject.GetComponent<AudioSource>();
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
        if(Input.GetAxis("Horizontal") < 0)
        {
            if (isFacingRight) mesh.transform.Rotate(0, 180, 0);
            isFacingRight = false;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            if (!isFacingRight) mesh.transform.Rotate(0, 180, 0);
            isFacingRight = true;
        }
        if(Input.GetAxis("Horizontal") != 0)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
        transform.Translate(speed * Time.deltaTime * movement);

        // Jump
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z)) && isGrounded)
        {
            if (!animator.GetBool("IsJumping"))
            {
                animator.SetBool("IsJumping", true);
            }
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        // Laser
        if (Input.GetMouseButton(0) && Time.timeScale == 1)
        {
            if (!animator.GetBool("IsShooting"))
            {
                animator.SetBool("IsShooting", true);
            }            
            if (!laserSound.isPlaying) laserSound.Play();
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
            animator.SetBool("IsShooting", false);
            laserSound.Stop();
            line.SetPosition(1, new Vector3(0, 0, 0));
        }
    }

    private void LateUpdate()
    {
        if (rb.velocity.y < 0)
        {
            if (!animator.GetBool("IsFalling"))
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", true);
            }
        }
        else if(rb.velocity.y >= 0)
        {
            animator.SetBool("IsFalling", false);
        }     
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.layer == 6)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.layer==6)
        {
            isGrounded = false;
        }
    }
}
