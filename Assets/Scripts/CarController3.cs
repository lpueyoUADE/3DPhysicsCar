using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController3 : MonoBehaviour
{
    public float motorPower;
    public float steeringPower;
    public float jumpForce;
    public float handBrakeForce;

    public float wheelRotation;

    public float linearDrag;
    public float angularDrag;

    public GameObject centerOfMass;

    public GameObject[] wheels;
    public TrailRenderer backLeftTR;
    public TrailRenderer backRightTR;

    public Light BackLightLeft;
    public Light BackLightRight;

    public LayerMask layerMask;
    Rigidbody rb;
    public AudioSource audioSource;
    public AudioSource audioSourceGas;
    public AudioSource audioSourceVelocity;

    public AudioClip gearSound;
    public AudioClip jumpSound;

    private float gas;
    private float steering;

    public int gear;

    private float[] gears = {
        -0.5f,
        0,
        0.3f,
        0.75f,
        1f
    };

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.position;

        gear = 1;
    }
    public bool isBreaking()
    {
        return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.DownArrow);
    }

    private void Update()
    {
        BackLightLeft.enabled = gas < 0 || isBreaking();
        BackLightRight.enabled = gas < 0 || isBreaking();

        if(Input.GetKeyDown(KeyCode.Z) && gear > 0)
        {
            gear--;
            audioSource.PlayOneShot(gearSound);
        }

        if (Input.GetKeyDown(KeyCode.X) && gear < gears.Length - 1)
        {
            gear++;
            audioSource.PlayOneShot(gearSound);
        }
    }

    private void FixedUpdate()
    {        
        gas = Input.GetAxis("Vertical");
        steering = Input.GetAxis("Horizontal");

        RaycastHit hit;

        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        float direction = localVelocity.z < 0 ? -1 : 1;
        
        // Trails
        backLeftTR.emitting = false;
        backRightTR.emitting = false;

        if (Physics.Raycast(transform.position + transform.up * 2f, Vector3.down, out hit, 2f, layerMask))
        {
            // Acelerate
            rb.AddForce(Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized * gas * motorPower * gears[gear]);

            // HandBrake
            if(isBreaking())
            {
                rb.AddForce(new Vector3(rb.velocity.x, 0, rb.velocity.z) * -1 * handBrakeForce);
            }

            // Steer
            if (rb.velocity.magnitude > 0.03)
            {
                int reverse = gear == 0 ? -1 : 1;
                float steerWithVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized.magnitude;
                rb.AddTorque(steerWithVelocity * reverse * steering * steeringPower * transform.up);
            }

            // Drag
            rb.AddForce(new Vector3(rb.velocity.x, 0, rb.velocity.z) * -1 * linearDrag);

            // Angular Drag
            Vector3 sidewaysForce = Vector3.Project(rb.velocity, transform.right);
            sidewaysForce.y = 0;

            rb.AddForce(sidewaysForce * -1 *  angularDrag);

            // Jump
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                audioSource.PlayOneShot(jumpSound);
            }

            // Trails
            if(sidewaysForce.magnitude > 0.7f && gas != 0)
            {
                backLeftTR.emitting = true;
                backRightTR.emitting = true;
            }
        }

        SpinAndSteerWheels(direction);

        EngineSound();
    }
    private void SpinAndSteerWheels(float direction)
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            if(i < 2)
            {
                wheels[i].transform.parent.localRotation = Quaternion.Euler(new Vector3(0, steering * wheelRotation, 0));
            }

            float radius = wheels[i].GetComponentInParent<SphereCollider>().radius;
            wheels[i].transform.Rotate((rb.velocity.magnitude / radius) * direction, 0, 0);
        }
    }
    private void EngineSound()
    {
        audioSourceGas.pitch = 1 + (0.5f * gas);
        float magnitude = 0.25f * new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude / 10;
        audioSourceVelocity.pitch = 0.85f + magnitude;
        audioSourceVelocity.volume = Clamp(magnitude, 0.2f, 1f);
    }

    private float Clamp(float value, float min, float max)
    {
        return (value <= min) ? min : (value >= max) ? max : value;
    }
}
