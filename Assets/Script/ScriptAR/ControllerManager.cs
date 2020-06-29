using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ControllerManager : MonoBehaviour
{
    public Button btnSpeed;
    public Button btnLeft;
    public Button btnRight;
    public Button btnJump;
    public Button btnAttack;
    public Button btnPause;
    public Image fillHeal;
    public bool isUiActive;
    public List<GameObject> playerCarAR = new List<GameObject>();
    public static ControllerManager Instance;
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
        playerCarAR[Random.Range(0, 6)].gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        isUiActive = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if((PlayerControllerAR.Instance.meshPlayer.enabled == true) && !isUiActive)
        {
            UiActive();        
        }
        if(PlayerControllerAR.Instance.meshPlayer.enabled == true)
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

    }

    public void RotationRight()
    {
        PlayerControllerAR.Instance.RotationRight();
    }
    public void OffRotationRight()
    {
        PlayerControllerAR.Instance.OffRotationRight();
    }
    public void RotationLeft()
    {
        PlayerControllerAR.Instance.RotationLeft();
    }
    public void OffRotationLeft()
    {
        PlayerControllerAR.Instance.OffRotationLeft();
    }


    public void Jump()
    {
        PlayerControllerAR.Instance.Jump();
    }
    public void Attack()
    {
        if (gameSates)
        {
            PlayerControllerAR.Instance.Attack();
        }        
    }

    public void UiActive()
    {
        btnLeft.gameObject.SetActive(true);
        btnRight.gameObject.SetActive(true);
        btnJump.gameObject.SetActive(true);
        btnAttack.gameObject.SetActive(true);
        btnPause.gameObject.SetActive(true);
        fillHeal.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        isUiActive = true;
    }

    #region Win of Player
    //GAME WIN
    public void ActiveUIWinAR()
    {
        UIManager.Instance.OffSoundCarDriving();
        UIManager.Instance.OnSoundWin();
        if (i <= 4)
        {
            numberEnemyActive--;
            if (numberEnemyActive <= 0)
            {
                PlayerControllerAR.Instance.z = 0;
                UIWin.SetActive(true);
                numberEnemyActive = 1;
            }
        }
        else
        {
            numberEnemyActive--;
            if (numberEnemyActive <= 0)
            {
                PlayerControllerAR.Instance.z = 0;
                UIWinOver.SetActive(true);
                numberEnemyActive = 1;
            }
        }
       
    }
    public void UnActiveUIWinAR()
    {
        UIWin.SetActive(false);
    }

    public void HomeWinAR()
    {
        SceneManager.LoadScene(0);
        UIManager.Instance.OnSoundBackGround();
    }

    public void ContinueWinAR()
    {
        if (i <= 4)
        {
            UIManager.Instance.OnSoundCarDriving();
            PlayerControllerAR.Instance.z = 4.5f;
            Planet[i].gameObject.SetActive(true);
            EnemyCar[i].gameObject.SetActive(true);
            i++;
            UnActiveUIWinAR();
        }
    }
    public void RestartWinAR()
    {
        SceneManager.LoadScene(3);
    }
    #endregion


    #region Lose of Player
    //GAME LOSE
    public void ActiveUILoseAR()
    {
        UIManager.Instance.OffSoundCarDriving();
        UIManager.Instance.OnSoundLose();
        UILose.SetActive(true);
    }
    public void UnActiveUILoseAR()
    {
        UILose.SetActive(false);
    }
    public void HomeLoseAR()
    {
        SceneManager.LoadScene(0);
        UIManager.Instance.OnSoundBackGround();
    }
    public void RestartLoseAR()
    {
        SceneManager.LoadScene(3);
    }
    #endregion


    #region Pause Game
    //Pause
    public void ActivePauseAR()
    {
        UIManager.Instance.OffSoundCarDriving();
        Time.timeScale = 0;
        UIPause.SetActive(true);
    }
    public void UnActivePauseAR()
    {
        UIPause.SetActive(false);
    }
    public void ResumeAR()
    {
        UIManager.Instance.OnSoundCarDriving();
        Time.timeScale = 1;
        UnActivePauseAR();
    }
    #endregion

}
