using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    Rigidbody rb;
    public Rigidbody[] wheels;
    public GameObject centerOfMass;
    public float motorPower = 1000;
    public float turnPower = 1000;
    public float dragAmount = 100;
    public float angularDragAmount = 100;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void FixedUpdate()
    {
        Vector3 motorTorque = rb.transform.forward * Input.GetAxis("Vertical") * motorPower;
        Vector3 motorTorqueX = Vector3.ProjectOnPlane(motorTorque, Vector3.up);

        Vector3 motionTurn = rb.transform.up * Input.GetAxis("Horizontal") * turnPower;

        rb.AddForce(motorTorque);
        rb.AddTorque(motionTurn);

        //Drag
        // rb.AddForce(Vector3.Project(rb.velocity, transform.forward).normalized * dragAmount * -1);

        Debug.DrawRay(rb.transform.position, rb.transform.forward * 3);
    }
}
