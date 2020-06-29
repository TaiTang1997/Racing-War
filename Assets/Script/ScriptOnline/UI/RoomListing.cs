using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    public RoomInfo _RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        _RoomInfo = roomInfo;
        _text.text = roomInfo.Name + " (" +roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers + ")";
        gameObject.name = roomInfo.Name;
    }

    public void OnClick_Button()
    {        
        if (PhotonNetwork.NetworkingClient.Server != ServerConnection.MasterServer || !PhotonNetwork.IsConnectedAndReady)
        {
            GameObject.Find(_RoomInfo.Name).SetActive(false);
            RoomListingMenu.Instance.txtStateRoom.gameObject.SetActive(true);
            RoomListingMenu.Instance.txtStateRoom.text = _RoomInfo.Name + " Does Not Exist";
        }
        else
        {
            PhotonNetwork.JoinRoom(_RoomInfo.Name);
        }

    }
}
