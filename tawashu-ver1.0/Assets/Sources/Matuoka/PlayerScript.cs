using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviourPunCallbacks
{

    //public BulletManager bulletmanager;
    //public GameObject prefab;

    //public float angle;
    //public float AngleChangeSpeed;

    //---------松岡ソース------------------------//

    public GameObject Tower;
    private GameObject LookTarget;
    public GameObject Bullet;
    public GameObject Barricade;
    private EnemySpawn enemy;


    private float speed;              //オブジェクトのスピード
    private float radius;             //円を描く半径
    private Vector3 defPosition;        //defPositionをVector3で定義する。

    float x;
    float z;
    float Angle;
    PlayerState State;                    //プレイヤーの状態
    public int P_Atk = 30;               //攻撃力
    public int B_Hp = 100;               //バリケードの耐久地
    public int P_Energy = 0;            //強化に使用するエナジー
    public int EnhancementPhase = 0;    //強化段階
    public int EnhancedFees = 20;       //強化費用(今は20)

    // 連射時設定
    private float time = 0f;
    public float Interval = 0.5f;

    //  敵撃破数
    public int EnemyDefeatNum = 0;

    private Camera camera;
    public float CameraRadius;


    //中西　追加
    //マジックナンバー防止
    private const int AttackSpeed = 0;
    private const int PowerUp = 1;
    private const int RotateSpeedUp = 2;
    private const int BarricadeCreate = 3;
    private const int State_Finish = 4;

    //左から1レべのステータス　ステータスいじるならここで
    private static readonly float[] Read_AttackSpeed = { 0.5f, 0.4f, 0.3f, 0.2f, 0.1f };                 //弾の速さ
    private static readonly int[] Read_PowerUP = { 100, 150, 200, 250, 300 };           //攻撃力
    private static readonly float[] Read_RotateSpeed = { 15f, 20f, 25f, 30f, 35f };     //旋回速度
    private static readonly int[] Read_BarricadeHP = { 150, 200, 250, 300, 350 };       //バリケード耐久値
    //レベルアップする際に必要なコスト      左から１→２になる際に必要なコスト
    private static readonly int[] Read_AttackSpeed_LevelUpCost = { 10, 40, 60, 80 };
    private static readonly int[] Read_PowerUp_LevelUpCost = { 20, 40, 60, 80 };
    private static readonly int[] Read_RotateSpeed_LevelUpCost = { 30, 40, 60, 80 };
    private static readonly int[] Read_BarricadeHP_LevelUpCost = { 40, 40, 60, 80 };
    //

    private const int MaxLevel = 4;         //強化限界レベル

    private int[] NowLevel;                 //それぞれの現在のレベル取得     上の定数も使う
    public float HP;                        //HP
    public float Mana;                      //Mana
    public float HP_MAX = 4000.0f;           //HP最大値
    public float Mana_MAX = 100.0f;         //Mana最大値   
    public bool NowCreate;                  //生成モードか強化モードか
    public int[,] NowState;                 //現在のステータス取得

    private float Mana_AppSpeed = 2;        //秒間アップ数
    private float time_Left;                //秒間処理用

    public enum PlayerState
    {
        Move = 0,
        InTower = 1
    }
    //-------------------------------------------//

    void Awake()
    {
        Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = 15.0f;
        radius = 3.0f;
        Angle = 0.0f;
        State = PlayerState.Move;

        defPosition = Tower.transform.position;    //タワーから円運動を始める

        camera = GameObject.FindWithTag("DefenceCamera").GetComponent<Camera>();
        enemy = GameObject.FindWithTag("AttackPlayer").GetComponent<EnemySpawn>();
        AddInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        //エラー防止
        if (camera == null)
        {
            camera = GameObject.FindWithTag("DefenceCamera").GetComponent<Camera>();
        }
        if(enemy==null)
        {
            enemy = GameObject.FindWithTag("AttackPlayer").GetComponent<EnemySpawn>();
        }

        Nakanishi_Update();
        time += Time.deltaTime;

        if (photonView.IsMine)
        {
            //  弾を撃つ処理
            // 左クリック押下かつ設定した連射間隔より経過時間が大きければ発射可能
            if (Input.GetButton("Fire1") && time >= Interval)
            {
                //photonView.RPC(nameof(CreateBullet), RpcTarget.All);
                //Vector3 v = transform.forward;
                //photonView.RPC(nameof(CreateBullet), RpcTarget.All, transform.position, transform.forward);
                GameObject b = Instantiate(Bullet, transform.position, transform.rotation);
                b.GetComponent<PlayerBullet01>().SetDmageValue(P_Atk);      // バフ時
                time -= Interval;
            }

            //  Zキーを押して尚且つエナジーがEnhancedFees以上で更に強化が10段階未満であれば
            if (Input.GetKeyDown(KeyCode.Z) && P_Energy >= EnhancedFees && EnhancementPhase < 10)
            {
                P_Atk += 20;            //  攻撃力up
                P_Energy -= 20;         //  エナジーを消費
                EnhancementPhase++;     //  強化段階を上げる
                EnhancedFees += 0;      //  強化費用を上げる(今の仕様では強化費用は上げなくていいので0)
            }
            //  Xキーを押してバリケードを設置
            if (Input.GetKeyDown(KeyCode.X) && time >= Interval)
            {
                photonView.RPC(nameof(CreateBarricade), RpcTarget.All);
                //GameObject b = Instantiate(Barricade, transform.position, Barricade.transform.rotation);
                //b.GetComponent<BarricadeScript>().SetHpValue(B_Hp);
                //time = 0f;
            }

            //  エナジーが0以下にならないようにする
            if (P_Energy <= 0) P_Energy = 0;


            switch (State)
            {
                case PlayerState.Move:        //   プレイヤーがタワーの周辺で円運動
                    if (Input.GetKey(KeyCode.D))
                    {
                        Angle += Time.deltaTime * (Mathf.PI * 2.0f / speed);
                        camera.transform.RotateAround(camera.transform.position, camera.transform.forward, -Time.deltaTime * (360.0f / speed));
                    }
                    if (Input.GetKey(KeyCode.A))
                    {
                        Angle -= Time.deltaTime * (Mathf.PI * 2.0f / speed);
                        camera.transform.RotateAround(camera.transform.position, camera.transform.forward, Time.deltaTime * (360.0f / speed));
                    }

                    x = radius * Mathf.Sin(Angle);      //X軸の設定
                    z = radius * Mathf.Cos(Angle);      //Z軸の設定 

                    //  プレイヤー後進処理
                    //if (Input.GetKeyDown(KeyCode.S))
                    //{
                    //    transform.position -= transform.forward * speed * Time.deltaTime;
                    //}

                    //Debug.Log(Angle);
                    break;

                case PlayerState.InTower:     //  プレイヤーがタワーの中に入ったとき

                    break;
            }
            transform.position = new Vector3(x + defPosition.x, transform.position.y, z + defPosition.z);  //自分のいる位置から座標を動かす。

            Vector3 TowerPosOffset = new Vector3(Tower.transform.position.x, transform.position.y, Tower.transform.position.z);
            Vector3 TtoP = (transform.position - TowerPosOffset).normalized;
            TowerPosOffset = TowerPosOffset + TtoP * CameraRadius;

            camera.transform.position = new Vector3(TowerPosOffset.x, camera.transform.position.y, TowerPosOffset.z);


            //  仮で弾を出している
            //if (Input.GetMouseButtonDown(0))
            //{
            //    Vector3 v = camera.transform.forward;
            //    //photonView.RPC(nameof(Fire), RpcTarget.All, transform.position, v);
            //    photonView.RPC(nameof(CreateBullet), RpcTarget.All, transform.position, transform.forward);
            //    //PhotonNetwork.Instantiate()
            //    //photonView.RPC(nameof(Fire), RpcTarget.All, transform.position);
            //    //Fire(transform.position);
            //}

            //死亡判定
            //死亡判定
            if (IsDeath())
            {
                photonView.RPC(nameof(SendChangeScene_Win), RpcTarget.Others);
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene("Lose");
            }
            //他のプレイヤー消滅　＝　特殊勝利
            if (Alone())
            {
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene("Win");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mana")
        {
            P_Energy += 5;                   //  エナジーを増やす
            Destroy(other.gameObject);
        }
    }


    [PunRPC]
    private void CreateBullet()
    {
        GameObject b = Instantiate(Bullet, transform.position, transform.rotation);
        b.GetComponent<PlayerBullet01>().SetDmageValue(P_Atk);      // バフ時
        //time = 0f;
        //var b = Instantiate(this.Bullet, Vector3.zero, Quaternion.identity);
        //float timestamp = info.SentServerTimestamp;
        //b.GetComponent<MOve>().Init(origin, velocity, timestamp);
        //Debug.Log("Create Shot");
        //Debug.Log("photonView.OwnerActorNr:" + photonView.OwnerActorNr);
    }

    [PunRPC]
    private void CreateBarricade()
    {
        GameObject b = Instantiate(Barricade, transform.position, transform.rotation);
        b.GetComponent<BarricadeScript02>().SetHP(B_Hp);
        time -= Interval;
    }

    private void AddInitialize()
    {
        //初期は生成モードにする
        NowCreate = true;
        NowLevel = new int[State_Finish];
        //レベル初期化
        for (var i = 0; i < State_Finish; i++)
        {
            NowLevel[i] = 0;
        }
        //使うからとりあえずnew
        NowState = new int[State_Finish, MaxLevel];

        //ステータスセット
        for (var i = 0; i < MaxLevel; i++)
        {
            NowState[AttackSpeed, i] = (int)Read_AttackSpeed[i];
            NowState[PowerUp, i] = Read_PowerUP[i];
            NowState[RotateSpeedUp, i] = (int)Read_RotateSpeed[i];
            NowState[BarricadeCreate, i] = Read_BarricadeHP[i];
        }
    } 

    private int GetNowLevel(int SelectNumber)
    {
        return NowLevel[SelectNumber];
    }

    private bool LevelUp(int Target)
    {
        int level_ = NowLevel[Target];
        //もう最大値なので処理しない
        if (level_ >= MaxLevel) return false;

        switch(Target)
        {
            case AttackSpeed:
                if(Mana >= Read_AttackSpeed_LevelUpCost[level_])
                {
                    Mana -= Read_AttackSpeed_LevelUpCost[level_];
                    NowLevel[Target]++;
                    Interval = Read_AttackSpeed[NowLevel[Target]];
                    Debug.Log("AttackSpeed is Level up");
                    return true;
                }
                break;
            case PowerUp:
                if(Mana>=Read_PowerUp_LevelUpCost[level_])
                {
                    Mana -= Read_PowerUp_LevelUpCost[level_];
                    NowLevel[Target]++;
                    P_Atk = Read_PowerUP[NowLevel[Target]];
                    Debug.Log("Powerup is Level up");
                    return true;
                }
                break;
            case RotateSpeedUp:
                if (Mana >= Read_RotateSpeed_LevelUpCost[level_])
                {
                    Mana -= Read_RotateSpeed_LevelUpCost[level_];
                    NowLevel[Target]++;
                    speed = Read_RotateSpeed[NowLevel[Target]];
                    Debug.Log("RotateSpeed is Level up");
                    return true;
                }
                break;
            case BarricadeCreate:
                if(Mana>=Read_BarricadeHP_LevelUpCost[level_])
                {
                    Mana -= Read_BarricadeHP_LevelUpCost[level_];
                    NowLevel[Target]++;
                    B_Hp = Read_BarricadeHP[NowLevel[Target]];
                    Debug.Log("BarricadeHP is Level up");
                    return true;
                }
                break;
        }
        return false;
    }            

    private void Initialize()
    {
        HP = HP_MAX;
        Mana = 0;
        time_Left = 0.0f;
    }

    public bool IsDeath()
    {
        if (HP <= 0)
        {
            return true;
        }
        return false;
    }

    private bool Alone()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            return true;
        }
        return false;
    }

    //送るよう
    [PunRPC]
    public void SendChangeScene_Win()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Win");
    }

    [PunRPC]
    public void SendChangeScene_Lose()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Win");
    }

    private void Nakanishi_Update()
    {
        //一度取得

        //１秒ごとに呼ぶ
        time_Left += Time.deltaTime;
        if (time_Left >= 1.0f)
        {
            time_Left = 0.0f;
            Mana += Mana_AppSpeed;
            Debug.Log(Mana);
            if (Mana > Mana_MAX)
            {
                Mana = Mana_MAX;
            }
        }
    }
}