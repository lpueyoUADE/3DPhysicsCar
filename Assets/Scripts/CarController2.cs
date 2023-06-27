using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController2 : MonoBehaviour
{
    Rigidbody rb;
    public GameObject centerOfMass;
    
    public LayerMask layerMask;

    
    public float suspensionMaxDistance = 1;
    public float stiffness = 100;
    public float damping = 100;
    public float motorPower = 1000;
    public float turnPower = 500;
    
    public Transform[] suspension;
    /*private void calculateCorners()
    {
        Vector3 sizeMax = GetComponent<Renderer>().bounds.max;
        Vector3 sizeMin = GetComponent<Renderer>().bounds.min;

        Vector3 BottomLeft = new Vector3(sizeMin.x, 0, sizeMin.z);
        Vector3 BottomRight = new Vector3(sizeMin.x, 0, sizeMax.z);
        Vector3 TopLeft = new Vector3(sizeMax.x, 0, sizeMin.z);
        Vector3 TopRight = new Vector3(sizeMax.x, 0, sizeMax.z);

        suspension = new Vector3[] {
               BottomLeft,
               BottomRight,
               TopLeft,
               TopRight
        };
    }*/
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.localPosition;        
    }

    private void FixedUpdate()
    {
        //calculateCorners();

        foreach (var susPoint in suspension)
        {
            RaycastHit hit;

            Vector3 point = susPoint.position;

            Ray ray = new Ray(point, Vector3.down);

            if (Physics.Raycast(ray, out hit, suspensionMaxDistance, layerMask))
            {
                float compressionRatio = (suspensionMaxDistance - hit.distance) / suspensionMaxDistance;
                float force = stiffness * compressionRatio - (damping * rb.velocity.y);

                Debug.DrawRay(point, rb.transform.up * force, Color.red);

                rb.AddForceAtPosition(transform.up * force, point);
            }

            Debug.DrawRay(point, rb.transform.up * -suspensionMaxDistance);

        }

        Vector3 motorForce = rb.transform.forward * Input.GetAxis("Vertical") * motorPower;
        Vector3 motorForceX = Vector3.ProjectOnPlane(motorForce, Vector3.up);

        Vector3 motionTurn = rb.transform.up * Input.GetAxis("Horizontal") * turnPower;

        rb.AddForce(motorForceX);
        rb.AddTorque(motionTurn);
    }
}
