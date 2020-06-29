using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private GameObject buttonOffSound;
    [SerializeField] private GameObject buttonOnSound;
    [SerializeField] private GameObject panelHelp;

    private void Start()
    {
        if (UIManager.Instance.offAndOnBackGroundSound.activeInHierarchy)
        {
            buttonOffSound.SetActive(true);
            buttonOnSound.SetActive(false);
        }
        else
        {
            buttonOnSound.SetActive(true);
            buttonOffSound.SetActive(false);
        }
    }

    /*
    * Off Sound
    */
    public void OffSoundGame()
    {
        buttonOffSound.SetActive(false);
        buttonOnSound.SetActive(true);
        UIManager.Instance.OffBackGroundSound();
    }
    /*
     * On Sound 
     */
    public void OnSoundGame()
    {
        buttonOffSound.SetActive(true);
        buttonOnSound.SetActive(false);
        UIManager.Instance.OnBackGroundSound();
    }
    /*
     * Help Game
     */
    public void OnHelpGame()
    {
        panelHelp.SetActive(true);
    }
    public void OffHelpGame()
    {
        panelHelp.SetActive(false);
    }

}
