using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class BulletScript01 : MonoBehaviourPunCallbacks
{
    private Player player;
    public GameObject Target;
    public float Speed = 5;
    public float Timer = 5;
    public int DmageValue = 100;
    private int PlayerDamage = 2;
    public Vector3 ToTarget;
    private Vector3 velocity;
    private Vector3 target;

    //----------ネット用-------------//
    private int TimeStamp;
    private Vector3 origin;
    public bool IsActive => gameObject.activeSelf;       //  オブジェクトのアクティブ管理
    public int ID { get; private set; }
    public int OwnerID { get; private set; }
    public bool Equals(int id, int ownerid) => id == ID && OwnerID == ownerid;
    //-------------------------------//

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("DefencePlayer").GetComponent<Player>();
        //velocity = (Target.transform.position - transform.position).normalized;
        ToTarget = Target ? (Target.transform.position - transform.position).normalized : transform.forward;
    }


    //      ↓こいつだと常に読んじゃうので下に別のアップデート用意しました
    //      ので別のほうに更新書いてね
    // Update is called once per frame
    void Update()
    {
        transform.position += ToTarget * (Speed * Time.deltaTime);
        //Debug.Log(ToTarget);
        //transform.position += velocity * (Speed * Time.deltaTime);
        // 消滅
        //Timer -= elapsedtime;
        Timer -= Time.deltaTime;
        if (Timer < 0) Destroy(gameObject);
    }

    // 当たり判定
    void OnTriggerEnter(Collider other)
    {
        // プレイヤー
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }

        // 塔
        if (other.gameObject.tag == "Tower")
        {
            if(player==null)
            {
                player = GameObject.FindWithTag("DefencePlayer").GetComponent<Player>();
            }
            //photonView.RPC(nameof(Damage), RpcTarget.All, DmageValue);
            //player.HP -= DmageValue;
            Destroy(gameObject);
            //PhotonNetwork.Destroy(gameObject);
        }

        // バリケード
        if (other.gameObject.tag == "barricade")
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
    }
    public void SetDmageValue(int damage) { DmageValue = damage; }

    //-----------ネット用　新規作成----------------//
    public void OnUpdate()
    {
        float elapsedtime = Mathf.Max(0f, unchecked(PhotonNetwork.ServerTimestamp - TimeStamp) / 1000f);
        // 移動          
        //transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);

        transform.position = origin + velocity * (Speed) * elapsedtime;
        //transform.position += ToTarget * (Speed * Time.deltaTime);

        // 消滅
        Timer -= elapsedtime;
        //Timer -= Time.deltaTime;
        if (Timer < 0) Destroy(gameObject);
    }

    public void Active(int id,int ownerId,Vector3 origin,Vector3 angle,int timestamp)
    {
        ID = id;
        OwnerID = ownerId;
        this.origin = origin;
        target = angle;
        velocity = angle;
        //velocity = (angle - origin).normalized;
        Debug.Log(velocity);
        this.TimeStamp = timestamp;
        OnUpdate();

        gameObject.SetActive(true);
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
    }


    //自動処理用

    //MonoBehaviour class の関数で画面外に行ったら呼ばれる
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    [PunRPC]
    void Damage(float d)
    {
        player.HP -= d;
    }

    //--------------------------------------------//

}
