using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class CallPlayerScript : MonoBehaviourPunCallbacks
{

    //ファイルパス
    //public bool Now_AttackPlayer;
    public GameObject Camera_Attack;
    public GameObject Camera_Defence;
    //public GameObject Core;
    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        //DontDestroyOnLoad(gameObject);
        Vector3 p = new Vector3(0.0f, 0.0f, 0.0f);
        //マスターかどうか
        if (PhotonNetwork.IsMasterClient)
        {
            //必要なもの呼び出し
            //床面
            GameObject floor = PhotonNetwork.Instantiate("Stage/Ground", Vector3.zero, Quaternion.identity);
            //DontDestroyOnLoad(floor);
            //SceneManager.MoveGameObjectToScene(floor, SceneManager.GetActiveScene());
            Debug.Log("Create_Floor");
            //タワー
            GameObject tower = PhotonNetwork.Instantiate("Stage/Tower", Vector3.zero, Quaternion.identity);
            //DontDestroyOnLoad(tower);
            //SceneManager.MoveGameObjectToScene(tower, SceneManager.GetActiveScene());
            Debug.Log("Create_Tower");

            Vector3 cpos = new Vector3(0.0f, 0.0f, 0.0f);

            PhotonNetwork.Instantiate("Player/Player_Attack", p, Quaternion.identity);
            Debug.Log("CreatePlayerA");

            Instantiate(Camera_Attack);
            //PhotonNetwork.Instantiate(CameraPlayerA, cpos, Quaternion.identity);
            Debug.Log("CreatePlayerA_Camera");
        }
        else
        {

            PhotonNetwork.Instantiate("Player/Player_Defence", p, Quaternion.identity);
            Debug.Log("CreatePlayerB");

            Instantiate(Camera_Defence);
            Debug.Log("CreatePlayerB_Camera");

            PhotonNetwork.Instantiate("Player/BossCore", p, Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
