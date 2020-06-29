using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MPManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public PlayFabAuth auth;
    public Text connectSates;
    public Text userNameText;
    public GameObject[] DisableConnect;
    public GameObject[] EnableConnect;
    public GameObject[] DisableJoin;
    public GameObject[] EnableJoin;
    public List<GameObject> listSpawn = new List<GameObject>();
    public int curentPlayer = 0;
    public int numSpawnPlayer = 0;
    public string username;
    public GameObject[] disOnPlayerCountMax;
    public bool gameSates = false;
    public bool aPlayerHasWin1001 = false;
    public bool aPlayerHasWin2001 = false;
    public Text timeText;
    public float time = 3f;
    public GameObject LosePanel;
    public Text textLose;
    public bool over1001 = false;
    public bool over2001 = false;
    public bool jumpPlayer = false;
    public bool rotaRight = false;
    public bool rotaLeft = false;
    public bool atkPlayer = false;
    public InputField txtRoomName;

    public Transform _content;

    public RoomListing _roomListing;
    public List<RoomListing> _listing = new List<RoomListing>();
    public List<string> _listNameCar = new List<string>();
    public int typeCar;
    public Image healthImgFill2;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(curentPlayer);
            stream.SendNext(numSpawnPlayer);
            stream.SendNext(aPlayerHasWin1001);
            stream.SendNext(aPlayerHasWin2001);
            stream.SendNext(over1001);
            stream.SendNext(over2001);
        }
        else if (stream.IsReading)
        {
            curentPlayer = (int)stream.ReceiveNext();
            numSpawnPlayer = (int)stream.ReceiveNext();
            aPlayerHasWin1001 = (bool)stream.ReceiveNext();
            aPlayerHasWin2001 = (bool)stream.ReceiveNext();
            over1001 = (bool)stream.ReceiveNext();
            over2001 = (bool)stream.ReceiveNext();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in gameObject.transform)
        {
            listSpawn.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameSates)
        {
            if (curentPlayer == 2)
            {
                timeText.gameObject.SetActive(true);
                foreach (GameObject disPlayer in disOnPlayerCountMax)
                {
                    disPlayer.SetActive(false);
                }
                time -= Time.deltaTime;
                timeText.text = "Time :" + Mathf.Round(time);
                if (time < 0)
                {
                    UIManager.Instance.OnSoundCarDriving();
                    gameSates = true;
                    timeText.gameObject.SetActive(false);
                }
            }
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        connectSates.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public override void OnConnectedToMaster()
    {
        foreach (GameObject dis in DisableConnect)
        {
            dis.SetActive(false);
        }
        foreach (GameObject en in EnableConnect)
        {
            en.SetActive(true);
        }
        userNameText.text = "Player: " + username;
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

    }




    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        RoomOptions room = new RoomOptions();
        room.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(txtRoomName.text, room, TypedLobby.Default);     
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Tao thanh cong");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Khong tim thay phong");
    }
    public override void OnJoinedRoom()
    {
        UIManager.Instance.OffSoundBackGround();
        photonView.RPC("AddPlayer", RpcTarget.All);
        photonView.RPC("SpawnPlayer", RpcTarget.All);

        foreach (GameObject disJoin in DisableJoin)
        {
            disJoin.SetActive(false);
        }
        foreach (GameObject enJoin in EnableJoin)
        {
            enJoin.SetActive(true);
        }
        RoomListingMenu.Instance.txtStateRoom.gameObject.SetActive(false);


        typeCar = Random.Range(0, 6);

        Vector3 pos = listSpawn[numSpawnPlayer - 1].transform.position;
        GameObject player = PhotonNetwork.Instantiate(_listNameCar[typeCar], pos, Quaternion.identity, 0);
        player.GetComponent<PlayerControllerOnline>().username = username;
        player.GetComponent<PlayerControllerOnline>().mp = this;

    }
    [PunRPC]
    void AddPlayer()
    {
        curentPlayer++;
    }
    [PunRPC]
    void SpawnPlayer()
    {
        numSpawnPlayer = PhotonNetwork.PlayerList.Length;
        if (numSpawnPlayer == 2)
        {
            healthImgFill2.gameObject.SetActive(true);
        }
    }
    public void Disconnect()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
        UIManager.Instance.OnSoundBackGround();
    }

    /*
    * Win 1001
    */
    public void SetWin1001()
    {
        LosePanel.SetActive(true);
        textLose.text = "You Win";
        UIManager.Instance.OffSoundCarDriving();
        auth.SetWins(1);
        photonView.RPC("HasWin1001", RpcTarget.All);
    }
    [PunRPC]
    void HasWin1001()
    {
        aPlayerHasWin1001 = true;
    }

    /*
     * Lose 1001
     */
    public void SetLose1001()
    {
        LosePanel.SetActive(true);
        textLose.text = "You Lose";
        UIManager.Instance.OffSoundCarDriving();
        photonView.RPC("HasLose1001", RpcTarget.All);
    }
    [PunRPC]
    void HasLose1001()
    {
        over1001 = true;
    }

    /*
     * Win 2001
     */
    public void SetWin2001()
    {
        LosePanel.SetActive(true);
        textLose.text = "You Win";
        UIManager.Instance.OffSoundCarDriving();
        auth.SetWins(1);
        photonView.RPC("HasWin2001", RpcTarget.All);
    }
    [PunRPC]
    void HasWin2001()
    {
        aPlayerHasWin2001 = true;
    }

    /*
     * Lose 2001
     */
    public void SetLose2001()
    {
        LosePanel.SetActive(true);
        textLose.text = "You Lose";
        UIManager.Instance.OffSoundCarDriving();
        photonView.RPC("HasLose2001", RpcTarget.All);
    }
    [PunRPC]
    void HasLose2001()
    {
        over2001 = true;
    }

    public void LeaveRoom()
    {
        LosePanel.gameObject.SetActive(false);
        aPlayerHasWin1001 = false;
        aPlayerHasWin2001 = false;
        gameSates = false;
        over1001 = false;
        over2001 = false;
        curentPlayer = 0;
        numSpawnPlayer = 0;
        time = 3f;
        healthImgFill2.gameObject.SetActive(false);
        PhotonNetwork.LeaveRoom();
        UIManager.Instance.OnSoundBackGround();
    }

    public override void OnLeftRoom()
    {
        txtRoomName.text = "";
        
        foreach (GameObject enJoin in EnableJoin)
        {
            enJoin.SetActive(false);
        }
        foreach (GameObject disJoin in DisableJoin)
        {
            disJoin.SetActive(true);
        }
    }



    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = _listing.FindIndex(x => x._RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(_listing[index].gameObject);
                    _listing.RemoveAt(index);
                }
            }
            else
            {
                int index = _listing.FindIndex(x => x._RoomInfo.Name == info.Name);
                if (index == -1)
                {
                    RoomListing listing = Instantiate(_roomListing, _content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        _listing.Add(listing);
                    }
                }
                else
                {

                }

            }

        }
    }

    public void JumpPlayer()
    {
        jumpPlayer = true;
    }

    public void OnRotationRight()
    {
        rotaRight = true;
    }
    public void OffRotationRight()
    {
        rotaRight = false;
    }
    public void OnRotationLeft()
    {
        rotaLeft = true;
    }
    public void OffRotationLeft()
    {
        rotaLeft = false;
    }

    public void AtkPlayer()
    {
        atkPlayer = true;
    }

    public void ExitLogin()
    {
        SceneManager.LoadScene(0);
    }
}
