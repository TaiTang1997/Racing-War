using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] Button BtnSinglePlayer;
    [SerializeField] Button BtnMultiplePlayer;
    [SerializeField] Button BtnArPlayer;
    [SerializeField] Button BtnHelp;
    [SerializeField] Button BtnStore;
    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }

    void Initialization()
    {
        BtnSinglePlayer.onClick.AddListener(OnClickSinglePlayer);
        BtnMultiplePlayer.onClick.AddListener(OnClickMultiplePlayer);
        BtnArPlayer.onClick.AddListener(OnClickAR);
        BtnHelp.onClick.AddListener(OnClickHelp);
        BtnStore.onClick.AddListener(OnClickStore);
    }

    void OnClickSinglePlayer()
    {
        SceneManager.LoadScene(1);
        UIManager.Instance.OffSoundBackGround();
    }
    void OnClickMultiplePlayer()
    {
        SceneManager.LoadScene(2);
    }
    void OnClickAR()
    {
        SceneManager.LoadScene(3);
        UIManager.Instance.OffSoundBackGround();
    }


    void OnClickHelp()
    {

    }

    void OnClickStore()
    {

    }
}
