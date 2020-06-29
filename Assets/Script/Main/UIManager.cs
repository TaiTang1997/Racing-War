using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Music
{
    public string name;
    public AudioClip audioClip;
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private Music[] musics;

    [SerializeField] private AudioSource audiSourceBackground;
    [SerializeField] private AudioSource audiSourceWin;
    [SerializeField] private AudioSource audiSourceLose;
    [SerializeField] private AudioSource audiSourceAttack;
    [SerializeField] private AudioSource audiSourceCarDriving;

    [SerializeField] private GameObject effectPlayerEnemyDied;
    [SerializeField] private GameObject effectBulletPlayer;
    [SerializeField] private GameObject effectBulletEnemy;
    [SerializeField] private GameObject effectPlayerEnemyDiedAR;
    [SerializeField] private GameObject effectBulletPlayerAR;
    [SerializeField] private GameObject effectBulletEnemyAR;
    public GameObject offAndOnBackGroundSound;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialization();
        OnSoundBackGround();
    }
    public void Initialization()
    {
        audiSourceBackground.clip = musics[0].audioClip;
        audiSourceWin.clip = musics[1].audioClip;
        audiSourceLose.clip = musics[2].audioClip;
        audiSourceAttack.clip = musics[3].audioClip;
        audiSourceCarDriving.clip = musics[4].audioClip;
    }
    /*
     * Background
     */
    public void OnSoundBackGround()
    {
        audiSourceBackground.Play();
    }
    public void OffSoundBackGround()
    {
        audiSourceBackground.Stop();
    }

    /*
   * CarDriving 
   */
    public void OnSoundCarDriving()
    {
        audiSourceCarDriving.Play();
    }
    public void OffSoundCarDriving()
    {
        audiSourceCarDriving.Stop();
    }
    /*
     * Game Win
     */
    public void OnSoundWin()
    {
        audiSourceWin.Play();
    }
    /*
     * Game Lose
     */
    public void OnSoundLose()
    {
        audiSourceLose.Play();
    }
    /*
     * Attack
     */
    public void OnAttack()
    {
        audiSourceAttack.Play();
    }

    /*
     * Effect
     */
    public void EffectDied(GameObject obj)
    {
        GameObject effect = GameObject.Instantiate(effectPlayerEnemyDied, obj.transform.position, Quaternion.identity) as GameObject;
        DestroyGameObjectPool(effect, 1f);
    }
    public void EffectBulletPlayerDestroy(GameObject obj)
    {
        GameObject effect = GameObject.Instantiate(effectBulletPlayer, obj.transform.position, Quaternion.identity) as GameObject;
        DestroyGameObjectPool(effect, 1f);
    }
    public void EffectBulletEnemyDestroy(GameObject obj)
    {
        GameObject effect = GameObject.Instantiate(effectBulletEnemy, obj.transform.position, Quaternion.identity) as GameObject;
        DestroyGameObjectPool(effect, 1f);
    }

    public void EffectDiedAR(GameObject obj)
    {
        GameObject effect = GameObject.Instantiate(effectPlayerEnemyDiedAR, obj.transform.position, Quaternion.identity) as GameObject;
        DestroyGameObjectPool(effect, 1f);
    }
    public void EffectBulletPlayerDestroyAR(GameObject obj)
    {
        GameObject effect = GameObject.Instantiate(effectBulletPlayerAR, obj.transform.position, Quaternion.identity) as GameObject;
        DestroyGameObjectPool(effect, 1f);
    }
    public void EffectBulletEnemyDestroyAR(GameObject obj)
    {
        GameObject effect = GameObject.Instantiate(effectBulletEnemyAR, obj.transform.position, Quaternion.identity) as GameObject;
        DestroyGameObjectPool(effect, 1f);
    }

    public void DestroyGameObjectPool(GameObject obj, float time)
    {
        if (obj != null)
        {
            StartCoroutine(DestroyGameObject(obj, time));
        }
    }

    IEnumerator DestroyGameObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        if (obj != null)
        {
            Destroy(obj);
        }
    }

    public void OffBackGroundSound()
    {
        offAndOnBackGroundSound.SetActive(false);
    }
    public void OnBackGroundSound()
    {
        offAndOnBackGroundSound.SetActive(true);
    }


}
