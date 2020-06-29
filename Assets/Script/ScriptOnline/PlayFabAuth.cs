using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
public class PlayFabAuth : MonoBehaviour
{
    public MPManager mpManager;
    public InputField user;
    public InputField pass;
    public InputField email;
    public Text txtMessage;
    public Button btnLogin;
    public Button btnRegister1;
    public Button btnRegister2;
    public Button btnBackLogin;
    public Text txtNumberWin;
    public Text txtNumberLose;
    public bool IsAuthenticated = false;
    public GetPlayerCombinedInfoRequestParams InfoRequest;
    LoginWithPlayFabRequest loginRequest;
    public int Blocks = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Login()
    {
        loginRequest = new LoginWithPlayFabRequest();
        loginRequest.Username = user.text;
        loginRequest.Password = pass.text;
        loginRequest.InfoRequestParameters = InfoRequest;//Cờ cho những phần thông tin để trả lại cho người dùng.
        PlayFabClientAPI.LoginWithPlayFab(loginRequest, resultCallback =>
        {
            IsAuthenticated = true;
            txtMessage.text = "Login Successfully";
            mpManager.ConnectToMaster();
            mpManager.username = user.text;

            Blocks = resultCallback.InfoResultPayload.UserVirtualCurrency["BL"];//Kết quả cho thông tin được yêu cầu.->Từ điển số dư tiền ảo thuộc về người dùng.
            GetValueLose();
            GetValueWin();

        }, errorCallback =>
        {
            IsAuthenticated = false;
            txtMessage.text = "Login Failed";
        }, null);
    }

    public void Register1()
    {
        email.gameObject.SetActive(true);
        btnLogin.gameObject.SetActive(false);
        btnRegister2.gameObject.SetActive(true);
        btnRegister1.gameObject.SetActive(false);
        btnBackLogin.gameObject.SetActive(true);
        txtMessage.text = "";
        user.text = "";
        pass.text = "";
    }
    public void BackLogin()
    {
        email.gameObject.SetActive(false);
        btnLogin.gameObject.SetActive(true);
        btnRegister2.gameObject.SetActive(false);
        btnRegister1.gameObject.SetActive(true);
        btnBackLogin.gameObject.SetActive(false);
        txtMessage.text = "";
        user.text = "";
        pass.text = "";
        email.text = "";
    }

    public void Register2()
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
        request.Email = email.text;
        request.Username = user.text;
        request.Password = pass.text;
        PlayFabClientAPI.RegisterPlayFabUser(request, resultCallback =>
        {
            txtMessage.text = "Create Account Success";
        }, errorCallback =>
        {
            txtMessage.text = "Create Account Failed";
            email.gameObject.SetActive(true);
        });
    }


    public void BuyItems(string ItemID)
    {
        PurchaseItemRequest pr = new PurchaseItemRequest();
        pr.CatalogVersion = "Player Abilitys";
        pr.ItemId = ItemID;
        pr.VirtualCurrency = "BL";
        pr.Price = 1000;

        GetUserInventoryRequest ir = new GetUserInventoryRequest();

        PlayFabClientAPI.GetUserInventory(ir, resultCallback =>
        {
            List<ItemInstance> ls = resultCallback.Inventory;
            bool hasItem = false;
            foreach (ItemInstance i in ls)
            {
                if (i.ItemId == ItemID)
                {
                    hasItem = true;
                    Debug.Log("Item aready : " + i.DisplayName);
                }
                else
                {
                    hasItem = false;
                }

            }
            if (hasItem == false)
            {
                PlayFabClientAPI.PurchaseItem(pr, resultCallback2 =>
                {
                    Blocks -= 1000;
                    Debug.Log("Iteam Purcha: " + resultCallback2.Items[0].DisplayName);
                }, errorCallback1 =>
                {

                });
            }
        }, errorCallback => { });

    }

    /* 
     * Set giá trị win cho Player
     */
    public void SetWins(int value)
    {
        GetPlayerStatisticsRequest request1 = new GetPlayerStatisticsRequest();
        List<string> names = new List<string>();
        names.Add("Wins");
        request1.StatisticNames = names;
        int tell = 0;
        PlayFabClientAPI.GetPlayerStatistics(request1, resultCallback =>
        {
            foreach (var eachStat in resultCallback.Statistics)
            {
                if (eachStat.StatisticName.Equals("Wins"))
                {
                    tell = eachStat.Value;
                }

            }
            UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest();

            List<StatisticUpdate> list = new List<StatisticUpdate>();
            value += tell;
            StatisticUpdate sts = new StatisticUpdate();
            sts.StatisticName = "Wins";
            sts.Value = value;
            list.Add(sts);

            request.Statistics = list;

            PlayFabClientAPI.UpdatePlayerStatistics(request, resultCallback2 =>
            {
                Debug.Log("Wins have been set");
            }, errorCallback2 =>
            {

            });

        }, errorCallback =>
        {

        });

    }
    /*
     * Get giá trị Win cho Player
     */
    public void GetValueWin()
    {
        GetPlayerStatisticsRequest request1 = new GetPlayerStatisticsRequest();
        List<string> names = new List<string>();
        names.Add("Wins");
        request1.StatisticNames = names;
        PlayFabClientAPI.GetPlayerStatistics(request1, resultCallback =>
        {
            foreach (var eachStat in resultCallback.Statistics)
            {
                if (eachStat.StatisticName.Equals("Wins"))
                {
                    txtNumberWin.text = "Win: " + eachStat.Value;
                }

            }

        }, errorCallback =>
        {

        });

    }

    /*
     * Set giá trị Lose cho Player
     */
    public void SetPlayerLose(int value)
    {
        GetPlayerStatisticsRequest request = new GetPlayerStatisticsRequest();
        List<string> namePlayer = new List<string>();
        namePlayer.Add("Loses");
        request.StatisticNames = namePlayer;
        int ab = 0;
        PlayFabClientAPI.GetPlayerStatistics(request, resultCallback =>
        {
            foreach (var eachStat in resultCallback.Statistics)
            {
                if (eachStat.StatisticName.Equals("Loses"))
                {
                    ab = eachStat.Value;
                }

            }
            UpdatePlayerStatisticsRequest request2 = new UpdatePlayerStatisticsRequest();

            List<StatisticUpdate> list1 = new List<StatisticUpdate>();
            value += ab;
            StatisticUpdate sts1 = new StatisticUpdate();
            sts1.StatisticName = "Loses";
            sts1.Value = value;
            list1.Add(sts1);

            request2.Statistics = list1;

            PlayFabClientAPI.UpdatePlayerStatistics(request2, resultCallback2 =>
            {
                Debug.Log("Loses have been set");
            }, errorCallback2 =>
            {

            });

        }, errorCallback =>
        {

        });
    }


    /*
     * Get giá trị Lose cho Player
     */
    public void GetValueLose()
    {
        GetPlayerStatisticsRequest request = new GetPlayerStatisticsRequest();
        List<string> namePlayer = new List<string>();
        namePlayer.Add("Loses");
        request.StatisticNames = namePlayer;

        PlayFabClientAPI.GetPlayerStatistics(request, resultCallback =>
        {
            foreach (var eachStat in resultCallback.Statistics)
            {
                if (eachStat.StatisticName.Equals("Loses"))
                {
                    txtNumberLose.text = "Lose: " + eachStat.Value;
                }

            }

        }, errorCallback =>
        {

        });
    }


}
