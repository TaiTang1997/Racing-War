using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListPlayer : MonoBehaviour
{
    public static ListPlayer Instance { get; private set; }
    public int i;
    public string[] numberPlayer;
    private void Awake()
    {
        Instance = this;
    } 
}
