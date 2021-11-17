using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Stage : MonoBehaviourPunCallbacks
{
    public string GroundName;
    public string TowerName;

    public GameObject Ground;
    public GameObject Tower;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        //部屋作成者が作る
        //if (PhotonNetwork.IsMasterClient)
        //{
            //床面
        //    PhotonNetwork.Instantiate(GroundName, Vector3.zero, Quaternion.identity);
        //    Debug.Log("Create_Floor");
        //    //タワー
        //    PhotonNetwork.Instantiate(TowerName, Vector3.zero, Quaternion.identity);
        //    Debug.Log("Create_Tower");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
