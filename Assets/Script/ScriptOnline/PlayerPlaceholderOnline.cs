using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PlayerPlaceholderOnline : MonoBehaviour
{
    public GameObject Player;
    public GameObject Planet;
    public static PlayerPlaceholderOnline Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Player.transform.position, 1f);

        Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

        //ROTATION
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1f);

    }


    public void NewPlanet(GameObject newPlanet)
    {

        Planet = newPlanet;
    }

}
