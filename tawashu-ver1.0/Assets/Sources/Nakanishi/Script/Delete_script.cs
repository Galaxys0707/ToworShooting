using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Delete_script : MonoBehaviourPunCallbacks
{

    private float timer;
    private PhotonView my_photon;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        my_photon = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer>=2.0f)
        {
            if(!my_photon.IsMine)
            {
                my_photon.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
