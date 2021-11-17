using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class EnemyScript01 : MonoBehaviourPunCallbacks
{
    enum EnemyState
    {
        Move,
        Attack,
        SkillEnter,     // スキル仕様の瞬間
        SkillStay,      // スキル仕様待機
    }

    public enum EnemyType
    {
        Avant,          // 前衛
        Middle,         // 中衛
        Rear,           // 後衛
    }

    // ステータス
    public int HP = 100;                   // ヒットポイント
    public int HP_MAX = 100;               // 最大ヒットポイント
    public float ATK = 1;                    // 攻撃力
    public float ATK_Buff = 100;               // 攻撃力（バフ時）
    public float TP = 0;                     // テクニカルポイント（100溜まったらスキル撃つ）
    public EnemyType Type;                       // タイプ（前衛、中衛、後衛）

    // 移動
    private Player player;                     // プレイヤー
    public GameObject Tower;                      // これに向かって行く
    public float Speed = 3;                  // 移動速度
    public float AttackRange = 10;           // 塔を攻撃し始める射程

    // ショット
    public BulletManager bulletmanager;          // マネージャー
    public GameObject Bullet;                     // 弾のプレハブ入れる
    public float ShotInterval_M = 3.0f;      // ショット間隔（移動中）
    public float ShotInterval_T = 2.0f;      // ショット間隔（射程内）
    private float ShotTimer;                  // ショットタイマー
    private BulletScript01 BulletScript;               // 弾のスクリプト

    // 行動
    private EnemyState CurrentState;               // どういう行動をするか
    private EnemyState PreviousState;              // 前の行動
    private float Distance_T;                 // 塔との距離

    // スキル
    public GameObject SkillHealObject;            // スキル（回復）
    public GameObject SkillBuffObject;            // スキル（バフ）
    public GameObject SkillAttackObject;          // スキル（攻撃）
    public float SkillStayTime = 2.0f;       // スキル待機時間
    public float SkillChargeTime = 10.0f;    // スキルが使えるようになるまでの時間
    public float SkillPower;                 // スキルの威力
    private float SkillTimer = 1.0f;          // スキル用タイマー

    public float BuffTime = 10.0f;           // バフ効果時間
    private float BuffTimer;                  // バフ効果時間タイマー

    public GameObject Mana;                       // 死んだら発生するマナ
    public int ManaVolume = 5;             // マナの量

    private PhotonView my_photonview;
    //----------ネット用-------------//
    private int TimeStamp;
    private Vector3 origin;
    public bool IsActive => gameObject.activeSelf;       //  オブジェクトのアクティブ管理
    public int ID { get; private set; }
    public int OwnerID { get; private set; }
    public bool Equals(int id, int ownerid) => id == ID && OwnerID == ownerid;
    //-------------------------------//

    public Vector3 velocity;

    void Awake()
    {
        my_photonview = GetComponent<PhotonView>();
        //object[] data = photonView.InstantiationData;
        HP_MAX = (int)my_photonview.InstantiationData[0];
        ATK = (float)my_photonview.InstantiationData[1];
        SkillPower = (float)my_photonview.InstantiationData[2];
        ManaVolume = (int)my_photonview.InstantiationData[3];
        Tower.transform.position = (Vector3)my_photonview.InstantiationData[4];
        //Player.transform.position = (Vector3)my_photonview.InstantiationData[5];
    }

    // Start is called before the first frame update
    void Start()
    {
        // 変数初期化
        //Tower.transform.position = new Vector3(Tower.transform.position.x, 0, Tower.transform.position.z);
        transform.LookAt(Tower.transform);
        Distance_T = (transform.position - Tower.transform.position).magnitude;
        PreviousState = CurrentState = EnemyState.Move;
        TP = 0;
        SkillStayTime = 2.0f;
        ShotTimer = ShotInterval_M;
        HP = HP_MAX;
        switch(Type)
        {
            case EnemyType.Avant:
                AttackRange = 10;
                break;
            case EnemyType.Middle:
                AttackRange = 20;
                break;
            case EnemyType.Rear:
                AttackRange = 30;
                break;
        }


        // デバッグ用（後で消す）

    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log("EnemyHP" + HP);
        //gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
        //Debug.Log(gameObject.GetComponent<PhotonView>().Owner);
        // 死亡判定
        //if (HP <= 0)
        //{
        //    for (int i = 0; i < ManaVolume; i++)
        //    {
        //        GameObject mana = Instantiate(Mana, transform.position, transform.rotation);
        //        mana.GetComponent<ParticleScript01>().SetObject(transform.position, Tower);
        //    }

        //    //gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
        //    //Debug.Log(gameObject.GetComponent<PhotonView>().Owner);
        //    //PhotonNetwork.Destroy(gameObject);
        //}
        if (photonView.IsMine)
        {
            if (photonView.CreatorActorNr != photonView.OwnerActorNr)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                PhotonNetwork.Destroy(gameObject);
                player.Mana += ManaVolume;
            }
            //Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
            switch (CurrentState)
            {
                // 移動
                case EnemyState.Move:
                    // 移動
                    Vector3 ToTower = (Tower.transform.position - transform.position).normalized;
                    transform.position += ToTower * (Speed * Time.deltaTime);

                    // TP上昇
                    TP += Time.deltaTime / SkillChargeTime;
                    // スキル発動
                    if (TP >= 1.0f)
                    {
                        PreviousState = CurrentState;
                        CurrentState = EnemyState.SkillEnter;
                        break;
                    }

                    // バフ
                    if (BuffTimer > 0) BuffTimer -= Time.deltaTime;

                    // 攻撃
                    ShotTimer -= Time.deltaTime;
                    if (ShotTimer < 0)
                    {
                        photonView.RPC(nameof(create), RpcTarget.All);
                        ShotTimer = ShotInterval_T + ShotTimer;
                        //GameObject childGameObject = transform.Find("BulletPos").gameObject;
                        //GameObject b = Instantiate(Bullet, childGameObject.transform.position, transform.rotation);
                        //b.GetComponent<BulletScript01>().SetTarget(Player);
                        //ShotTimer = ShotInterval_M + ShotTimer;
                    }

                    // 距離計算
                    Distance_T = (transform.position - Tower.transform.position).magnitude;
                    // 塔との距離が射程以内なら塔への攻撃開始
                    if (Distance_T < AttackRange)
                    {
                        PreviousState = CurrentState;
                        CurrentState = EnemyState.Attack;
                        ShotTimer = ShotInterval_T;
                        break;
                    }

                    break;

                // 攻撃
                case EnemyState.Attack:

                    // TP上昇
                    TP += Time.deltaTime / SkillChargeTime;
                    // スキル発動
                    if (TP >= 1.0f)
                    {
                        PreviousState = CurrentState;
                        CurrentState = EnemyState.SkillEnter;
                        break;
                    }

                    // バフ
                    if (BuffTimer > 0) BuffTimer -= Time.deltaTime;

                    // 攻撃
                    ShotTimer -= Time.deltaTime;
                    if (ShotTimer < 0)
                    {
                        /*
                        GameObject childGameObject = transform.Find("BulletPos").gameObject;
                        GameObject b = Instantiate(Bullet, childGameObject.transform.position, transform.rotation);
                        b.GetComponent<BulletScript01>().SetTarget(Tower);
                        if (BuffTimer > 0)
                            b.GetComponent<BulletScript01>().SetDmageValue((int)ATK_Buff);      // バフ時
                        else
                            b.GetComponent<BulletScript01>().SetDmageValue((int)ATK);           // 通常
                        ShotTimer = ShotInterval_T + ShotTimer;
                        */
                        photonView.RPC(nameof(create), RpcTarget.All);
                        ShotTimer = ShotInterval_T + ShotTimer;
                    }
                    break;

                // スキル
                case EnemyState.SkillEnter:

                    TP -= 1.0f;
                    SkillTimer = SkillStayTime;     // スキル待機時間設定

                    switch (Type)
                    {
                        // 前衛
                        case EnemyType.Avant:
                            {
                                GameObject childGameObject = transform.Find("BulletPos").gameObject;
                                GameObject b = Instantiate(SkillAttackObject, childGameObject.transform.position, transform.rotation);
                                b.GetComponent<BulletScript01>().SetTarget(Tower);
                                if (BuffTimer > 0)
                                    b.GetComponent<BulletScript01>().SetDmageValue((int)(ATK_Buff * SkillPower));   // バフ時
                                else
                                    b.GetComponent<BulletScript01>().SetDmageValue((int)(ATK * SkillPower));        // 通常
                                break;
                            }
                        // 中衛
                        case EnemyType.Middle:
                            {
                                Vector3 BuffPos = transform.position + transform.forward * 2.5f;        // 前方にバフ
                                GameObject b = Instantiate(SkillBuffObject, BuffPos, transform.rotation);
                                EnemySkillBuff bs = b.gameObject.GetComponent<EnemySkillBuff>();
                                bs.SetBuffMag(SkillPower);        // バフ倍率設定
                                break;
                            }
                        // 後衛
                        case EnemyType.Rear:
                            {
                                Vector3 HealPos = transform.position + transform.forward * 2.5f;        // 自分も回復に当たる（はず）
                                GameObject h = Instantiate(SkillHealObject, HealPos, transform.rotation);
                                EnemySkillHeal hs = h.gameObject.GetComponent<EnemySkillHeal>();
                                hs.SetHealMag(SkillPower);        // 回復倍率設定
                                break;
                            }
                    }


                    CurrentState = EnemyState.SkillStay;
                    break;

                // スキル待機
                case EnemyState.SkillStay:

                    SkillTimer -= Time.deltaTime;

                    // スキル終わったら前の行動に戻る
                    if (SkillTimer <= 0)
                    {
                        EnemyState temp = CurrentState;
                        CurrentState = PreviousState;
                        PreviousState = temp;
                        break;
                    }
                    break;

            }
        }
    }

    public void OnUpdate()
    {
        float elapsedTime = Mathf.Max(0f, unchecked(PhotonNetwork.ServerTimestamp - TimeStamp) / 1000f);

        Vector3 ToTower = (Tower.transform.position - transform.position).normalized;
        transform.position = origin + ToTower * elapsedTime;
    }

    public void Active(int id, int ownerId, Vector3 origin, Vector3 angle, int timestamp)
    {
        ID = id;
        OwnerID = ownerId;
        this.origin = origin;
        velocity = (Tower.transform.position - origin).normalized;
        this.TimeStamp = timestamp;

        gameObject.SetActive(true);
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
    }

    public void SetTarget(GameObject t, GameObject p)
    {
        Tower = t;
        //Player = p;
    }

    public void SetData(int hp, float atk, float sp, int mv)
    {
        HP_MAX = hp;
        HP = HP_MAX;
        ATK = atk;
        SkillPower = sp;
        ManaVolume = mv;
    }

    // 当たった瞬間
    void OnTriggerEnter(Collider other)
    {
        // 回復（仮）
        if (other.gameObject.tag == "E_Heal")
        {
            EnemySkillHeal h = other.gameObject.GetComponent<EnemySkillHeal>();
            HP += (int)(h.HealMag * (float)HP_MAX);
            if (HP > HP_MAX) HP = HP_MAX;
            SkillTimer = h.DestroyTimer;
        }

        // バフ（仮）
        if (other.gameObject.tag == "E_Buff")
        {
            EnemySkillBuff b = other.gameObject.GetComponent<EnemySkillBuff>();
            ATK_Buff = b.BuffMag * ATK;
            BuffTimer = BuffTime;
            if (HP > HP_MAX) HP = HP_MAX;
            SkillTimer = b.DestroyTimer;
        }

        // プレイヤー弾
        if (other.gameObject.tag == "P_Bullet")
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("DefencePlayer").GetComponent<Player>();
            }

            int damage = player.GetState(true);
            HP -= damage;
            Debug.Log("Damege : " + damage + " EnemyHP : " + HP);
            if (HP <= 0)
            {
                if (!my_photonview.IsMine)
                {
                    Debug.Log("このオブジェクトの持ち主" + photonView.OwnerActorNr);
                    Debug.Log("ID:" + PhotonNetwork.LocalPlayer.ActorNumber);
                    if (photonView.OwnerActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        //my_photonview.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
                        photonView.RequestOwnership();
                        Debug.Log("現在の持ち主" + photonView.OwnerActorNr);
                    }
                }

                //my_photonview.SetOwnerInternal(PhotonNetwork.LocalPlayer, PhotonNetwork.LocalPlayer.ActorNumber);
                //my_photonview.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
                //Debug.Log("持ち主" + my_photonview.OwnerActorNr + ":自分" + PhotonNetwork.LocalPlayer.ActorNumber);
                //if (PhotonNetwork.LocalPlayer.ActorNumber == photonView.OwnerActorNr)
                //{
                //    PhotonNetwork.Destroy(gameObject);
                //    Debug.Log("Network Object [Enemy] is Destroyed");
                //    player.Mana += ManaVolume;
                //}
            }
            //Debug.Log("mana get");
            //if (!other.GetComponent<PhotonView>().IsMine)
            //{
            //other.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            //}
            //PhotonNetwork.Destroy(other.gameObject);
            //photonView.RPC(nameof(remove), RpcTarget.All, other.gameObject);

        }
    }

    // 当たっている間
    void OnTriggerStay(Collider other)
    {


    }

    [PunRPC]
    private void FireB(Vector3 origin, GameObject target_)
    {
        var b = Instantiate(Bullet);
        b.GetComponent<BulletScript01>().SetTarget(target_);
    }

    //オンライン用　まあネット上に作成
    [PunRPC]
    private void FireBullet(Vector3 origin, Vector3 target, PhotonMessageInfo info)
    {
        int time = info.SentServerTimestamp;
        bulletmanager.Fire(time, photonView.OwnerActorNr, origin, target, time);
    }

    [PunRPC]
    private void HitByBullet(int bulletid, int ownerid)
    {
        bulletmanager.Remove(bulletid, ownerid);
    }

    [PunRPC]
    void create()
    {
        GameObject childGameObject = transform.Find("BulletPos").gameObject;
        GameObject b = Instantiate(Bullet, childGameObject.transform.position, transform.rotation);
        b.GetComponent<BulletScript01>().SetTarget(Tower);
        if (BuffTimer > 0)
            b.GetComponent<BulletScript01>().SetDmageValue((int)ATK_Buff);      // バフ時
        else
            b.GetComponent<BulletScript01>().SetDmageValue((int)ATK);           // 通常
        //ShotTimer = ShotInterval_T + ShotTimer;
    }

    [PunRPC]
    void damage(int d)
    {
        HP -= d;
    }
}
