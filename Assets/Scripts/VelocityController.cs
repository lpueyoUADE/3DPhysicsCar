using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VelocityController : MonoBehaviour
{
    public Rigidbody rb;
    Text texto;
    void Start()
    {
        texto = GetComponentInChildren<Text>();
    }
    void Update()
    {
        texto.text = ((float)System.Math.Round(rb.velocity.magnitude * 3.6f, 0)).ToString() + " km/h";
    }
}
