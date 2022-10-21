using UnityEngine;
using DG.Tweening;


public class TrafficCar : MonoBehaviour
{
    [SerializeField] private float forwardSpeed ;

    private bool move = false;
    
    private bool isCrashed = false;
    private Tween changePosTween;

    private float pos;
    
    
    //create a limit to not touch the corners (not falldown)

    private void Start()
    {
       
        
        //add listener to event to moving cars .
        LevelController.Instance.gameStartedEvent.AddListener(() =>
        {
            move = true;
            
        }
        );
    }

    private void Update()
    {
        pos = transform.position.x;

        if (move)
        {
            transform.position += forwardSpeed * transform.forward * Time.deltaTime;

            if (isCrashed)
            {

                ChangePosSmoothly(10f, 0.25f);
               
              

            }
        }

       
      
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCrashed = true;
        }
    }

    public void ChangePosSmoothly(float newPos , float duration)
    {
        
        changePosTween.Kill();
        changePosTween = DOTween.To(() => pos, val => pos = val, newPos, duration);
    }
}
