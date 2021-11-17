using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class BarricadeScript02 : MonoBehaviourPunCallbacks
{
    public int      HP;       // 体力
    public Vector3 TowerPosition;   // タワーの場所
    public float CreatePos;         //タワーからのきょりで生成します
    private PhotonView my_photonview;

    void Awake()
    {
        my_photonview = GetComponent<PhotonView>();

        HP = (int)my_photonview.InstantiationData[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        //Vector3 direction = (this.transform.position - TowerPosition).normalized;
        transform.position = transform.forward * CreatePos;
    }

    public void SetTower(Vector3 pos)
    {
        TowerPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(HP);
        //// HP0以下なら消滅
        //if (HP <= 0)
        //{
        //    if (photonView.IsMine)
        //    {
        //        PhotonNetwork.Destroy(this.gameObject);
        //    }
        //}
    }

    // 当たり判定
    void OnTriggerEnter(Collider other)
    {
        // バリケード
        if (other.gameObject.tag == "E_Bullet")
        {
            BulletScript01 bs = other.gameObject.GetComponent<BulletScript01>();
            HP -= bs.DmageValue;    // ダメージ受ける
            Debug.Log("BarricadeHP : " + HP + ": 受けたダメージ = " + bs.DmageValue);
            //Destroy(other.gameObject);
            if (HP <= 0)
            {
                my_photonview.RequestOwnership();
                //my_photonview.TransferOwnership(PhotonNetwork.LocalPlayer);
                Debug.Log(photonView.OwnerActorNr);
                PhotonNetwork.Destroy(gameObject);
                Debug.Log("NetworkObject [Barricade] is Destroyed");

            }
        }
    }

    public void SetHP(int hp) { HP = hp; }

    
    /*
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(HP);
        }
        else
        {
            HP = (int)stream.ReceiveNext();
        }
    }*/
}
