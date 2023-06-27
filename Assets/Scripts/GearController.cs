using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearController : MonoBehaviour
{
    public CarController3 car;
    Text texto;
    void Start()
    {
        texto = GetComponentInChildren<Text>();
    }
    void Update()
    {

        switch (car.gear)
        {
            case 0:
                texto.text = "R";
                break;

            case 1:
                texto.text = "N";
                break;

            default:
                texto.text = (car.gear - 1).ToString();
                break;
        }
    }
}
