using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class RoomNowInfo : MonoBehaviourPunCallbacks
{
    public Text RoomName;
    public Text PlayerID;
    Photon.Realtime.Player[] player = PhotonNetwork.PlayerList;
    
    // Start is called before the first frame update
    void Start()
    {
        RoomName.text = "部屋の名前:" + PhotonNetwork.CurrentRoom.Name;
        // PlayerID.text = "ID:" + photonView.Owner.ActorNumber;
        for (int i = 0; i < player.Length; i++)
        {
            if ((i + 1) == player[i].ActorNumber)
            {
                PlayerID.text = "ID:" + PhotonNetwork.IsMasterClient;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
