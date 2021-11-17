using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class ARoom : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions roomoption = new RoomOptions();
        roomoption.MaxPlayers = 2;      //一部屋の最大プレイヤー数
        roomoption.IsOpen = true;       //入室許可をする
        roomoption.IsVisible = true;    //ロビーから見えるようにする

        //部屋の作成　ないなら作り、あるならそのまま接続
        PhotonNetwork.JoinOrCreateRoom("RoomA", roomoption, TypedLobby.Default);
    }

    //参加した時の処理
    public override void OnJoinedRoom()
    {
        var v = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0.0f);
        //ResourceファイルにあるPrefabを呼び出す
        //PhotonNetwork.Instantiate("PlayerB", v, Quaternion.identity);
        PhotonNetwork.NickName = "sb";

        Debug.Log("プレイヤーB作成完了");
    }
}
