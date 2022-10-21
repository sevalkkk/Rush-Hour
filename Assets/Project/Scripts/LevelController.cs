using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LevelController : MonoBehaviour
{
    public int levelCount;
    public bool isGameActive = false;
    public bool restart = false;
    public bool pauseGame = false;
    public GameObject tapToStart;
    public GameObject tapToRestart;
    public GameObject CarController;
    
    private bool isRestarted = false;
    private bool touched = false;

    private Tween changeSpeedTween;
   

   

    int currentLevel;

    #region Singleton Pattern
    public static LevelController Instance;

    [HideInInspector] public UnityEvent gameStartedEvent = new UnityEvent();
    


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }
    #endregion

    private void Start()
    {
        //we are getting current level, default =0
        currentLevel = PlayerPrefs.GetInt(Constants.CURRENT_LEVEL_DATA, 0);

        //for start game
        Time.timeScale = 1f;

       

    }


    private void Update()
    {
        isRestarted = CarController.GetComponent<CarController>().isRestart;
        
        if (isRestarted && !restart)
        {
            tapToRestart.SetActive(true);
            SlowMotion();
            restart = true;
            
        }


        if(touched && !pauseGame)
        {
            pauseGame = true;
            touched = false;     

        }

        else if(touched && pauseGame)
        {
            pauseGame = false;
            touched = false;
            Time.timeScale = 1;
        }
        if(pauseGame)
        {
            Time.timeScale = 0;
        }

        



    }
   

    public void StartGame()
    {
        isGameActive = true;
        tapToStart.SetActive(false);
        gameStartedEvent.Invoke();
    }

    public void RestartGame()
    {
       
        isRestarted = false;
        isGameActive = false;
        tapToRestart.SetActive(false);
        EndLevel(levelUp: false);
        PlayerPrefs.SetInt(Constants.SCORE_DATA, 0);
    }

    public void PauseGame()
    {
        
        touched = true;
        print("dur");
    }

    public void SlowMotion()
    {
        //before create tween , kill tween.
        changeSpeedTween.Kill();
        float slowMotionDuration = 1f;
        changeSpeedTween = DOTween.To(() => Time.timeScale, val => Time.timeScale = val, 0f, slowMotionDuration);


    }

    public void EndLevel(bool levelUp = true)
    {
        // If the level finished successfully, we call level up
        if (levelUp)
        {
            LevelUp();
        }

        isGameActive = false;

        DOTween.KillAll();
        SceneManager.LoadScene(currentLevel);
    }

    private void LevelUp()
    {
        if (currentLevel < levelCount - 1)
        {
            currentLevel++;
        }
        else
        {
            currentLevel = 0;
        }

        //set level to currentlevel.
        PlayerPrefs.SetInt(Constants.CURRENT_LEVEL_DATA, currentLevel);
        //save level.
        PlayerPrefs.Save();
    }
}
