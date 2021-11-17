using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;

public class ChooseUI : MonoBehaviourPunCallbacks,IPunObservable
{
    bool Push;

    bool Recieve_Push;
    bool Recieve_ButtonA;
    bool Recieve_ButtonB;

    public GameObject g_sword;
    public GameObject g_shield;
    public Canvas canvas;
    public Text wait_text;
    //public Text time_Text;
    private GameObject TimeTex_obj;

    public bool ButtonA;
    public bool ButtonB;

    private const float Count = 4.0f;               //表示カウント    予定カウント＋１
    private float SceneChangeTime = 3.0f;           //SceneChangeするまでの時間
    private float timer;                            //時間保存用
    private string SendTextMassage;                 //送信テキスト

    //private float RecieveTimer;                     //受け取り用時間変数

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            GameObject sw = Instantiate(g_sword);
            sw.transform.SetParent(canvas.transform, false);
            GameObject sword = sw.transform.GetChild(0).gameObject;
            Button sword_button = sword.GetComponent<Button>();
            sword_button.onClick.AddListener(ClickA);

            GameObject sh = Instantiate(g_shield);
            sh.transform.SetParent(canvas.transform, false);
            GameObject shield = sh.transform.GetChild(0).gameObject;
            Button shield_button = shield.GetComponent<Button>();
            shield_button.onClick.AddListener(ClickB);

            wait_text.text = "どちらかを選んでください";
        }
        else
        {
            GameObject sw = Instantiate(g_sword);
            sw.transform.SetParent(canvas.transform, false);
            GameObject sh = Instantiate(g_shield);
            sh.transform.SetParent(canvas.transform, false);
            wait_text.text = "他のプレイヤーが選択中です、お待ちください";

            TimeTex_obj = PhotonNetwork.Instantiate("TimeTex", Vector3.zero, Quaternion.identity);
            TimeTex_obj.transform.SetParent(canvas.transform, false);
            TimeTex_obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "llllllllllllll";
        }

        Push = false;
        ButtonA = false;
        ButtonB = false;

        Recieve_Push = false;
        Recieve_ButtonA = false;
        Recieve_ButtonB = false;

        SendTextMassage = "";
    }

    // Update is called once per frame
    void Update()
    {
        //部屋の人数がマックス？
        if(PhotonNetwork.CurrentRoom.PlayerCount==2)
        {
            if(Push)
            {
                wait_text.text = "ゲームが始まります";
            }
        }

        //wait_text.text = "" + PhotonNetwork.IsMasterClient;
        //別案
        if (Recieve_Push)
        {
            if (Recieve_ButtonB)
            {
                //マスター交代
                if (PhotonNetwork.LocalPlayer != PhotonNetwork.MasterClient)
                {
                    PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                }
            }

            if (PhotonNetwork.IsMasterClient)
            {
                wait_text.text = "あなたはAttack側になりました。";
            }
            else
            {
                wait_text.text = "あなたはDefence側になりました。";
            }

            //いったん変数作成
            float t = SceneChangeTime / Count;
            //時間加算
            timer += Time.deltaTime;


            if (timer >= t) { TimeTex_obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "3"; }
            if (timer >= t * 2.0f) { TimeTex_obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "2"; }
            if (timer >= t * 3.0f) { TimeTex_obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "1"; }

            //時間たったのでシーン変更
            if (SceneChangeTime <= timer)
            {
                photonView.RPC(nameof(This_SceneChange), RpcTarget.AllViaServer);
            }
        }
    }

    public void ClickA()
    {
        if (!Push)
        {
            Button b = GameObject.FindWithTag("AttackButton").GetComponent<Button>();
            b.interactable = false;
            //SelectA = true;
            Push = true;
            //MasterChange();
            ButtonA = true;
            Debug.Log("Push ButtonA");
            wait_text.text = "他のプレイヤーの参加を待っています";
        }
    }

    public void ClickB()
    {
        if (!Push)
        {
            Button b = GameObject.FindWithTag("DefenceButton").GetComponent<Button>();
            b.interactable = false;
            Push = true;
            //SelectA = false;
            ButtonB = true;
            Debug.Log("Push ButtonB");
            wait_text.text = "他のプレイヤーの参加を待っています";
        }
    }

    //マスター変更
    private void MasterChange()
    {
        //自分がマスターじゃなかったらチェンジ
        if (!PhotonNetwork.IsMasterClient)
        {
           PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);  
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(Push);
            stream.SendNext(ButtonA);
            stream.SendNext(ButtonB);
        }
        else
        {
            Recieve_Push = (bool)stream.ReceiveNext();
            Recieve_ButtonA = (bool)stream.ReceiveNext();
            Recieve_ButtonB = (bool)stream.ReceiveNext();
        }
    }



    [PunRPC]
    public void This_SceneChange()
    {
        SceneManager.LoadScene("SceneMain");
    }
}
