﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlaceholder : MonoBehaviour
{
    public GameObject Player;
    public GameObject Planet;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        //Planet = GameObject.Find("Planet1");
    }
    // Update is called once per frame
    void Update()
    {
        //SMOOTH

        //POSITION
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
