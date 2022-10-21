using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCtrl : MonoBehaviour
{

    public bool pressed;
    public static ButtonCtrl current;
    public int basmaSayaci = 0;

    private void Start()
    {
        current = this;
        pressed = false;
    }

    public void ButtonDown()
    {
        pressed = true;
        basmaSayaci = 1;
    }

    public void ButtonUp()
    {
        pressed = false;
    }

}
