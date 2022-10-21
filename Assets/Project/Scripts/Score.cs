using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    
    public Text CurrentScore;

    public OverTake overTake;
    

   
    void Start()
    {
        overTake = GameObject.FindGameObjectWithTag("OverTakePlayer").GetComponent<OverTake>();
     
    }

    
    void Update()
    {
      
        CurrentScore.text = overTake.score.ToString();

    }


}
