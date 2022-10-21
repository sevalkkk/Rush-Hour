using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{


    public Text CurrentHealthText;

    public CarController carController;

    float healthness;

    void Start()
    {
        healthness = 0;

    }


    void Update()
    {
        healthness = carController.GetComponent<CarController>().firstLife;
        CurrentHealthText.text = healthness.ToString();

    }


}
