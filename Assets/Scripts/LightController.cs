using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    public float velocity;
    public float limit;
    void Update()
    {
        transform.position += velocity * Vector3.right / 2.5f;
        if(transform.position.x > limit)
        {
            transform.position = new Vector3(-limit, transform.position.y, transform.position.z);
        }
    }
}
