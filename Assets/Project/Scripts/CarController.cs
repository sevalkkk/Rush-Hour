using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    #region Wheel Collider 
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider rearLeft;
    [SerializeField] private WheelCollider rearRight;

    #endregion
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private float speed;
    [SerializeField] private float rotationDuration;
    [SerializeField] private float laneChangeDuration;
    [SerializeField] private LineRenderer[] wheelTrails;

    private bool isTouched = false;
   
    private bool isTrailsActive = false;
    private bool isCrashed = false;
    private Rigidbody rb;
    private Sequence moveSequence;
    private Tween changeSpeedTween;
    private Ray ray;

    //private float gameSpeed;

    public Transform rot;
    public float currentSpeed;
    public float angularSpeed;
    public Slider lifeBarSlider;
    public bool isRestart = false;
    public bool isInRightLane = true;
    public float currentLife;
    public float firstLife;
    //create a limit to not touch the corners (not falldown)
    private float limitX = 4;
    private float gameSpeed;





    private void Start()
    {
        gameSpeed = Time.timeScale;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.position;
        //create sequence.
        moveSequence = DOTween.Sequence();

        SetTrailsActive(false);
        firstLife = lifeBarSlider.value + 99 ;
        //add listener to gamestarted event.
        LevelController.Instance.gameStartedEvent.AddListener(HandleGameStartedEvent);

       
    }

    private void Update()
    {
        float newX = transform.position.x;
        newX = 3f;
        Mathf.Clamp(newX, -limitX, limitX);
        if (gameSpeed!=0)
        {
            //set the motor torgue of rear wheels.
            rearLeft.motorTorque  = currentSpeed ;
            rearRight.motorTorque = currentSpeed;
            //set the steer angle of front wheels . Actually we don't affect the horizontol movement , i dont know , is it necessary ...
            frontLeft.steerAngle = angularSpeed;
            frontRight.steerAngle = angularSpeed;

        }


        // if game is not active , return.
        if (!LevelController.Instance.isGameActive)
        {
            return;
        }
        //move forward.
       
            transform.position += currentSpeed * transform.forward * Time.deltaTime;



        //stay in the left lane as long as you press hold the button or touch the screen;
        if (Input.GetMouseButtonDown(0) && !isTouched)
        {

            MoveToLeftLane();
            isTouched = true;
        }
        
        //when u quite the touch the screen , come back to right lane.
        if(Input.GetMouseButtonUp(0) && isTouched)
        {
            MoveToRightLane();
            isTouched = false;
            
            
        }
        if(isCrashed)
        {
            isCrashed = false;
            if(currentSpeed <=20)
            {
                firstLife -= 10f; ;
            }
            else if(currentSpeed > 20 && currentSpeed < 35)
            {
                firstLife -= 25f;
            }
            else
            {
                firstLife -= 45f;
            }
            lifeBarSlider.value = firstLife /100;

            if(firstLife <=0)
            {
                firstLife = 0;
                isRestart = true;
                
                



            }
        }


        // Draw wheel trails
        if (isTrailsActive)
        {
            for (int i = 0; i < wheelTrails.Length; i++)
            {
                //create a trails.
                wheelTrails[i].positionCount++;
                wheelTrails[i].SetPosition(wheelTrails[i].positionCount - 1, wheelTrails[i].transform.position);
            }
        }

        ray = new Ray(transform.position + Vector3.up * 0.75f, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            //if your raycast hitting something and its tag is traffic car , and distance of between player car and traffic car , is closer than 12.5f
            if (hit.transform != null && hit.collider.CompareTag("TrafficCar") && hit.distance < 12.5f)
            {
                ChangeSpeedSmoothly(speed / 2, 0.25f);
            }
            else
            { 
                if (isInRightLane)
                {
                    ChangeSpeedSmoothly(speed, 1.5f);
                }
                else
                {
                    ChangeSpeedSmoothly(speed * 2f, 3f);
                }
            }
        }
    }

    
    private void HandleGameStartedEvent()
    {
        currentSpeed = speed;
    }

    public void ChangeSpeedSmoothly(float newSpeed, float duration = 0.25f)
    {
        changeSpeedTween.Kill();
        changeSpeedTween = DOTween.To(() => currentSpeed, val => currentSpeed = val, newSpeed, duration);
    }

  

    public void MoveToLeftLane()
    {
        if (isInRightLane)
        {
            SetTrailsActive(true);
            isInRightLane = false;

            moveSequence.Kill();
            moveSequence = DOTween.Sequence();

            moveSequence.Append(transform.DOMoveX(-3f, laneChangeDuration).SetEase(Ease.OutQuint));
            moveSequence.Join(transform.DORotate(Vector3.up * -10f, rotationDuration).OnComplete(() => transform.DORotate(Vector3.zero, rotationDuration)));
            moveSequence.OnComplete(() => SetTrailsActive(false));
            moveSequence.Play();
        }
    }

    public void MoveToRightLane()
    {
        if (!isInRightLane)
        {
            if (!isTrailsActive)
            {
                SetTrailsActive(true);
            }
            isInRightLane = true;
            
            moveSequence.Kill();
            moveSequence = DOTween.Sequence();
            moveSequence.Append(transform.DOMoveX(3f, laneChangeDuration).SetEase(Ease.OutQuint));
            moveSequence.Join(transform.DORotate(Vector3.up * 10f, rotationDuration).OnComplete(() => transform.DORotate(Vector3.zero, rotationDuration)));
            moveSequence.OnComplete(() => SetTrailsActive(false));
            moveSequence.Play();

            



        }
    }

 

    public void SetTrailsActive(bool isActive)
    {
        if (isActive)
        {
            for (int i = 0; i < wheelTrails.Length; i++)
            {
                wheelTrails[i].positionCount = 0;
            }
        }

        isTrailsActive = isActive;
    }

  
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("TrafficCar"))
        {
            isCrashed = true;
            Destroy(other.gameObject);
        }
    }

    // for see the raycast 
    private void OnDrawGizmos()
    {
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
    }


}
