using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RoomListingMenu : MonoBehaviour
{
    public static RoomListingMenu Instance { get; private set; }
    public Text txtStateRoom;

    private void Awake()
    {
        Instance = this;
    }
}
