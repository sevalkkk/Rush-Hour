using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speed : MonoBehaviour
{


    public Text CurrentSpeedText;

    public CarController carController;

    float currentSpeed;

    void Start()
    {
        currentSpeed = 0;

    }


    void Update()
    {
        currentSpeed = carController.GetComponent<CarController>().currentSpeed;
        CurrentSpeedText.text = currentSpeed.ToString();

    }


}
