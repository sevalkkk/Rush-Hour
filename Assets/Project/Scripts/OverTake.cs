using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverTake : MonoBehaviour
{
    private bool isOverTake;
    public int score;

    public GameObject CarController;

    private bool isInRightLane;
    private bool triggered;
    void Start()
    {
        isOverTake=false;
        score = PlayerPrefs.GetInt(Constants.SCORE_DATA, 0);
    }

    // Update is called once per frame
    void Update()
    {

        isInRightLane = CarController.GetComponent<CarController>().isInRightLane;
        if (triggered && !isOverTake)
        {
            if(isInRightLane)
            {
                isOverTake = true;
                triggered = false;
            }

            if(isOverTake)
            {
                isOverTake = false;

                AddScore(25);
                
            }
           
            
            
        }
        
    }

    private void AddScore(int amount)
    {
        score += amount;
        PlayerPrefs.SetInt(Constants.SCORE_DATA, score);
        PlayerPrefs.Save();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OverTake"))
        {
            //isOverTake = true;
            triggered = true;
            

        }
    }
   
}
