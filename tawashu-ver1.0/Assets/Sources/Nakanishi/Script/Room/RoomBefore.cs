using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;

public class RoomBefore : MonoBehaviourPunCallbacks
{
    public Image Loading;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.InLobby)
        {
            Loading.enabled = false;
        }
    }

    public override void OnConnectedToMaster()
    {
        Loading.enabled = false;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        
    }
}
