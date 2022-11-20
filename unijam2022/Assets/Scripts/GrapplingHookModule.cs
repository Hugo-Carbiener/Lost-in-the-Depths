using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Component used for the grappling
 */
public class GrapplingHookModule : MonoBehaviour
{
    public LayerMask grappeableMask;
    [SerializeField] private LineRenderer line;
    private Vector3 forwardVector;
    private Vector3 grapplePoint;
    [SerializeField] private float maxDistance;

    //joint variables :
    private SpringJoint joint;
    [SerializeField] private float spring, damper, massScale, jointMinDistance, jointMaxDistance;
    
    //mesh objects :
    [SerializeField] private GameObject grapplingHook;
    private GameObject grappling;

    [SerializeField] private AudioSource audio;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartGrapple();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopGrapple();
        }
    }
    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartGrapple()
    {
        Debug.Log("Start grapple");
        audio.Play();
        RaycastHit hit;
        line.positionCount = 2;
        forwardVector = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);
        forwardVector.z = 0;
        if (Physics.Raycast(gameObject.transform.position, forwardVector, out hit, maxDistance))
        {
            grapplePoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(gameObject.transform.position,grapplePoint);
            joint.maxDistance = distanceFromPoint * jointMaxDistance;
            joint.minDistance = distanceFromPoint * jointMinDistance;
            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = massScale;

            grappling = Instantiate(grapplingHook);
            grappling.transform.parent = null;
            grappling.transform.position=grapplePoint;
            grappling.transform.right = (-transform.position+grapplePoint).normalized;
            GetComponent<Rigidbody>().AddForce(forwardVector.normalized*10f, ForceMode.Impulse);
        }
    }

    private void DrawRope()
    {
        if(line.positionCount > 0)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, grapplePoint);
        }

    }

    private void StopGrapple()
    {
        Debug.Log("Stop grapple");
        Destroy(joint);
        if(grappling != null)
        {
            Destroy(grappling);
        }
        line.positionCount = 0;
    }
}
