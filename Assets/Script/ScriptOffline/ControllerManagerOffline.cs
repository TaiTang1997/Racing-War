using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

public class ControllerManagerOffline : MonoBehaviour
{
    public List<GameObject> playerCar = new List<GameObject>();
    public static ControllerManagerOffline Instance;
    public bool gameSates = false;
    public Text timeText;
    private float timeStart = 3f;
    public int numberEnemyActive;
    public GameObject UIWin;
    public GameObject UILose;
    public GameObject UIPause;
    public GameObject UIWinOver;
    public GameObject[] Planet;
    public GameObject[] EnemyCar;
    public int i = 0;

    private void Awake()
    {
        Instance = this;
        playerCar[Random.Range(0, 6)].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!gameSates)
        {
            timeText.gameObject.SetActive(true);
            timeStart -= Time.deltaTime;
            timeText.text = "" + Mathf.Round(timeStart);
            if (timeStart < 0)
            {
                UIManager.Instance.OnSoundCarDriving();
                gameSates = true;
                timeText.gameObject.SetActive(false);
            }
        }
    }
    public void OnRotationRight()
    {
        PlayerController.Instance.OnRotationRight();
    }
    public void OffRotationRight()
    {
        PlayerController.Instance.OffRotationRight();
    }
    public void OnRotationLeft()
    {
        PlayerController.Instance.OnRotationLeft();
    }
    public void OffRotationLeft()
    {
        PlayerController.Instance.OffRotationLeft();
    }
    public void Jump()
    {
        PlayerController.Instance.Jump();
    }
    public void Attack()
    {
        if (gameSates)
        {
            PlayerController.Instance.AtkPlayer();
        }
       
    }
    #region Win of Player
    //GAME WIN
    public void ActiveUIWin()
    {
        UIManager.Instance.OffSoundCarDriving();
        UIManager.Instance.OnSoundWin();
        if (i <= 4)
        {
            numberEnemyActive--;
            if (numberEnemyActive <= 0)
            {
                PlayerController.Instance.x = 0;
                PlayerController.Instance.z = 0;
                UIWin.SetActive(true);
                numberEnemyActive = 1;
            }
        }
        else
        {
            numberEnemyActive--;
            if (numberEnemyActive <= 0)
            {
                PlayerController.Instance.x = 0;
                PlayerController.Instance.z = 0;
                UIWinOver.SetActive(true);
                numberEnemyActive = 1;
            }
        }
    
    }
    public void UnActiveUIWin()
    {
        UIWin.SetActive(false);
    }

    public void HomeWin()
    {
        SceneManager.LoadScene(0);
        UIManager.Instance.OnSoundBackGround();
    }
   
    public void ContinueWin()
    {
        if (i <= 4)
        {
            UIManager.Instance.OnSoundCarDriving();
            PlayerController.Instance.x = 0.1f;
            PlayerController.Instance.z = 0.2f;
            Planet[i].gameObject.SetActive(true);
            EnemyCar[i].gameObject.SetActive(true);
            i++;
            UnActiveUIWin();
        }
    }
    public void RestartWin()
    {
        SceneManager.LoadScene(1);
    }
    #endregion



    #region Lose of Player
    //GAME LOSE
    public void ActiveUILose()
    {
        UIManager.Instance.OffSoundCarDriving();
        UIManager.Instance.OnSoundLose();
        UILose.SetActive(true);
    }
    public void UnActiveUILose()
    {
        UILose.SetActive(false);
    }
    public void HomeLose()
    {
        SceneManager.LoadScene(0);
        UIManager.Instance.OnSoundBackGround();
    }
    public void RestartLose()
    {
        SceneManager.LoadScene(1);
    }
    #endregion



    #region Pause Game
    //Pause
    public void ActivePause()
    {
        UIManager.Instance.OffSoundCarDriving();
        Time.timeScale = 0;
        UIPause.SetActive(true);
    }
    public void UnActivePause()
    {
        UIPause.SetActive(false);
    }
    public void Resume()
    {
        UIManager.Instance.OnSoundCarDriving();
        Time.timeScale = 1;
        UnActivePause();
    }
    public void HomePause()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        UIManager.Instance.OnSoundBackGround();
    }
    #endregion
}