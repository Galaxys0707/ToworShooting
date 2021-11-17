using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public List<Text> RoomNames;
    public List<Text> RoomNumber;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //ロビー参加時と他のプレイヤーが更新したときに呼び出される
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room状況が更新されました");
        base.OnRoomListUpdate(roomList);
        foreach(var info in roomList)
        {
            for(int number=0;number<RoomNames.Count;number++)
            {
                //参加ボタン消すか消さないか

                //人数更新
                if(info.Name==RoomNames[number].text)
                {
                    RoomNumber[number].text = "人数:" + info.PlayerCount + "/" + info.MaxPlayers;
                    Debug.Log(info.PlayerCount);
                    break;
                }
            }
        }
    }
}
