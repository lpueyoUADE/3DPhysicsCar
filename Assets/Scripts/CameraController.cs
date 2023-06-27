using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float smooting;
    public float rotationSmooting;
    public Transform target;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, smooting);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationSmooting);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
    }
}
