using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class EnemySpawn : MonoBehaviourPunCallbacks,IPunObservable
{
    public EnemyManager     Enemymanager;
    public GameObject[]     Enemies;                            // 敵
    public int              SelectedEnemy = 0;                  // 選ばれている敵    public GameObject       Tower;  
    public int[]            EnemyLevels;                        // 敵のレベル
    public GameObject       Tower;                              // 塔
    public GameObject       Player;                             // プレイヤー
    public float            SpawnInterval = 2.0f;               // 生成の間隔
    private Camera          camera;                             // カメラ
    public GameObject       CameraObj;
    public GameObject Core;

    private Vector3         currentPosition = Vector3.zero;     // 刺さった位置
    private Vector3         SpawnPosition;                      // 敵を生成した位置
    public float            SpawnRadius = 20;                        // スポーン補正する半径

    //add nakanishi
    public float HP;      //HP
    public float Mana;    //Mana
    public float HP_MAX = 2000.0f;       //最大値
    public float Mana_MAX = 200.0f;     //最大値
    public int   ManaLevel = 0;         //マナ増加レベル
    //今が生成モードかどうかか判別
    public bool NowCreate;       
    //内部使用
    private float Mana_AppSpeed = 2;    //秒間アップ数

    private float time_Left;                //秒間処理用
    private const int Mana_MaxLevel = 4;    //マナ加速する際にマックスレベル       現状5レべがマックスなので　(最大値ー１で設定)
    public float GetMana = 4.0f;            //倒された際にゲットする

    //Mana用配列？
    int[] Mana_Heal_Number = { 2, 3, 4, 5, 6 };
    public int[] ManaLevelUpCost = { 50, 100, 150, 200, 0 };

    public bool IsMouseOver;

    public AudioClip Appear_SE;
    AudioSource audioSource;


    public struct EnemyData
    {
        public int HP;
        public float ATK;
        public float SkillPower;
        public int ManaVolume;

        public int SpawnCost;       // 生成コスト
        public int LevelupCost;     // 強化コスト
    }

    public enum EnemyType
    {
        Avant,          // 前衛
        Middle,         // 中衛
        Rear,           // 後衛
    }

    public EnemyData[,] EnemyDatas;         // 敵のステータスデータ


    void Awake()
    {
        Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        //photonView.RPC(nameof(CoreCreate), RpcTarget.All);
        //Camera = Camera.main;
        //CameraObj = GameObject.FindWithTag("AttackCamera");
        ManaLevel = 0;
        camera = GameObject.FindWithTag("AttackCamera").GetComponent<Camera>();
        SpawnPosition = transform.position;
        SelectedEnemy = 0;
        NowCreate = true;
        EnemyDatas = new EnemyData[3,5];                    // 敵データ初期化
        EnemyLevels = new int[3];                           // レベル初期化
        for (int i = 0; i < 3; i++) EnemyLevels[i] = 0;

        ////初期設定
        //*************** 後衛 ***************//
        // Lv1
        EnemyDatas[0, 0].HP = 100;
        EnemyDatas[0, 0].ATK = 10;
        EnemyDatas[0, 0].SkillPower = 0.3f;    // 回復の割合
        EnemyDatas[0, 0].ManaVolume = 1;       // 落とすマナ量
        EnemyDatas[0, 0].SpawnCost = 2;        // 生成コスト
        EnemyDatas[0, 0].LevelupCost = 20;     // 強化コスト

        // Lv2
        EnemyDatas[0, 1].HP = 150;
        EnemyDatas[0, 1].ATK = 20;
        EnemyDatas[0, 1].SkillPower = 0.4f;    // 回復の割合
        EnemyDatas[0, 1].ManaVolume = 2;       // 落とすマナ量
        EnemyDatas[0, 1].SpawnCost = 3;        // 生成コスト
        EnemyDatas[0, 1].LevelupCost = 50;     // 強化コスト

        // Lv3
        EnemyDatas[0, 2].HP = 200;
        EnemyDatas[0, 2].ATK = 30;
        EnemyDatas[0, 2].SkillPower = 0.5f;    // 回復の割合
        EnemyDatas[0, 2].ManaVolume = 3;       // 落とすマナ量
        EnemyDatas[0, 2].SpawnCost = 4;        // 生成コスト
        EnemyDatas[0, 2].LevelupCost = 100;      // 強化コスト

        // Lv4
        EnemyDatas[0, 3].HP = 250;
        EnemyDatas[0, 3].ATK = 40;
        EnemyDatas[0, 3].SkillPower = 0.6f;    // 回復の割合
        EnemyDatas[0, 3].ManaVolume = 4;       // 落とすマナ量
        EnemyDatas[0, 3].SpawnCost = 5;        // 生成コスト
        EnemyDatas[0, 3].LevelupCost = 200;      // 強化コスト

        // Lv5
        EnemyDatas[0, 4].HP = 350;
        EnemyDatas[0, 4].ATK = 60;
        EnemyDatas[0, 4].SkillPower = 0.8f;    // 回復の割合
        EnemyDatas[0, 4].ManaVolume = 6;       // 落とすマナ量
        EnemyDatas[0, 4].SpawnCost = 6;        // 生成コスト
        EnemyDatas[0, 4].LevelupCost = 0;      // 強化コスト(Lv最大なので無視)

        //*************** 中衛 ***************//
        // Lv1
        EnemyDatas[1, 0].HP = 150;
        EnemyDatas[1, 0].ATK = 20;
        EnemyDatas[1, 0].SkillPower = 1.2f;    // 攻撃力増加倍率
        EnemyDatas[1, 0].ManaVolume = 2;       // 落とすマナ量
        EnemyDatas[1, 0].SpawnCost = 3;        // 生成コスト
        EnemyDatas[1, 0].LevelupCost = 40;      // 強化コスト

        // Lv2
        EnemyDatas[1, 1].HP = 200;
        EnemyDatas[1, 1].ATK = 30;
        EnemyDatas[1, 1].SkillPower = 1.25f;    // 攻撃力増加倍率
        EnemyDatas[1, 1].ManaVolume = 3;       // 落とすマナ量
        EnemyDatas[1, 1].SpawnCost = 4;        // 生成コスト
        EnemyDatas[1, 1].LevelupCost = 80;      // 強化コスト

        // Lv3
        EnemyDatas[1, 2].HP = 250;
        EnemyDatas[1, 2].ATK = 40;
        EnemyDatas[1, 2].SkillPower = 1.3f;    // 攻撃力増加倍率
        EnemyDatas[1, 2].ManaVolume = 4;       // 落とすマナ量
        EnemyDatas[1, 2].SpawnCost = 5;        // 生成コスト
        EnemyDatas[1, 2].LevelupCost = 150;    // 強化コスト

        // Lv4
        EnemyDatas[1, 3].HP = 300;
        EnemyDatas[1, 3].ATK = 50;
        EnemyDatas[1, 3].SkillPower = 1.35f;    // 攻撃力増加倍率
        EnemyDatas[1, 3].ManaVolume = 5;       // 落とすマナ量
        EnemyDatas[1, 3].SpawnCost = 6;        // 生成コスト
        EnemyDatas[1, 3].LevelupCost = 300;      // 強化コスト

        // Lv5
        EnemyDatas[1, 4].HP = 400;
        EnemyDatas[1, 4].ATK = 70;
        EnemyDatas[1, 4].SkillPower = 1.5f;    // 攻撃力増加倍率
        EnemyDatas[1, 4].ManaVolume = 7;       // 落とすマナ量
        EnemyDatas[1, 4].SpawnCost = 7;        // 生成コスト
        EnemyDatas[1, 4].LevelupCost = 1;      // 強化コスト(Lv最大なので無視)


        //*************** 前衛 ***************//
        // Lv1
        EnemyDatas[2, 0].HP = 300;
        EnemyDatas[2, 0].ATK = 40;
        EnemyDatas[2, 0].SkillPower = 1.5f;    // 攻撃力 * この値のダメージを与える弾発射
        EnemyDatas[2, 0].ManaVolume = 3;       // 落とすマナ量
        EnemyDatas[2, 0].SpawnCost = 4;        // 生成コスト
        EnemyDatas[2, 0].LevelupCost = 50;      // 強化コスト

        // Lv2     
        EnemyDatas[2, 1].HP = 350;
        EnemyDatas[2, 1].ATK = 50;
        EnemyDatas[2, 1].SkillPower = 1.6f;    // 攻撃力 * この値のダメージを与える弾発射
        EnemyDatas[2, 1].ManaVolume = 4;       // 落とすマナ量
        EnemyDatas[2, 1].SpawnCost = 5;        // 生成コスト
        EnemyDatas[2, 1].LevelupCost = 100;      // 強化コスト

        // Lv3     
        EnemyDatas[2, 2].HP = 400;
        EnemyDatas[2, 2].ATK = 60;
        EnemyDatas[2, 2].SkillPower = 1.7f;    // 攻撃力 * この値のダメージを与える弾発射
        EnemyDatas[2, 2].ManaVolume = 5;       // 落とすマナ量
        EnemyDatas[2, 2].SpawnCost = 6;        // 生成コスト
        EnemyDatas[2, 2].LevelupCost = 200;      // 強化コスト

        // Lv4    
        EnemyDatas[2, 3].HP = 450;
        EnemyDatas[2, 3].ATK = 70;
        EnemyDatas[2, 3].SkillPower = 1.8f;    // 攻撃力 * この値のダメージを与える弾発射
        EnemyDatas[2, 3].ManaVolume = 6;       // 落とすマナ量
        EnemyDatas[2, 3].SpawnCost = 7;        // 生成コスト
        EnemyDatas[2, 3].LevelupCost = 400;      // 強化コスト

        // Lv5     
        EnemyDatas[2, 4].HP = 600;
        EnemyDatas[2, 4].ATK = 90;
        EnemyDatas[2, 4].SkillPower = 2.0f;    // 攻撃力 * この値のダメージを与える弾発射
        EnemyDatas[2, 4].ManaVolume = 8;       // 落とすマナ量
        EnemyDatas[2, 4].SpawnCost = 8;        // 生成コスト
        EnemyDatas[2, 4].LevelupCost = 1;      // 強化コスト(Lv最大なので無視)
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) SelectedEnemy = 0;
        if (Input.GetKeyDown("2")) SelectedEnemy = 1;
        if (Input.GetKeyDown("3")) SelectedEnemy = 2;
        //if (Input.GetKeyDown(KeyCode.H)) HP -= 20;

        Nakanishi_Update();

        if (photonView.IsMine)
        {
            #region
            /*
            if (Input.GetKeyDown(KeyCode.G))
            {
                object[] initdata = {
            EnemyDatas[SelectedEnemy, EnemyLevels[SelectedEnemy]].HP,
            EnemyDatas[SelectedEnemy, EnemyLevels[SelectedEnemy]].ATK,
            EnemyDatas[SelectedEnemy, EnemyLevels[SelectedEnemy]].SkillPower,
            EnemyDatas[SelectedEnemy, EnemyLevels[SelectedEnemy]].ManaVolume,
            Tower.transform.position,
            };
                Vector3 p = new Vector3(0.0f, 0.0f, 10.0f);
                PhotonNetwork.Instantiate("Enemy1", p, Quaternion.identity, 1, initdata);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                Vector3 p = new Vector3(0.0f, 0.0f, 10.0f);
                PhotonNetwork.Instantiate("Sample", p, Quaternion.identity);
            }
            */
            #endregion
            //Debug.Log(manager.EnemySelectNumber);
            int cost = GetEnemyData(SelectedEnemy, EnemyLevels[SelectedEnemy]).SpawnCost;   // 生成コスト

            if (Input.GetMouseButtonDown(0) && Mana >= cost&& !IsMouseOver) // マウス左ボタンが押された瞬間、マナが足りていれば
            {

                var ray = camera.ScreenPointToRay(Input.mousePosition); //レイの方向を定義
                var raycastHitList = Physics.RaycastAll(ray).ToList();      //レイのヒット先を探索

                foreach (RaycastHit it in raycastHitList)
                {
                    if (it.collider.gameObject.tag == "Ground")
                    {
                        Mana -= cost;   // コスト分マナ減らす

                        var distance = Vector3.Distance(camera.transform.position, raycastHitList.First().point); //距離計測

                        float height_A = camera.transform.position.y - raycastHitList.First().point.y;
                        float height_B = Player.transform.position.y - raycastHitList.First().point.y;
                        float offset = height_B / height_A * distance; //レイのヒット先からカメラへのベクトルを取得、

                        Vector3 vec = (camera.transform.position - raycastHitList.First().point).normalized;

                        SpawnPosition = raycastHitList.First().point + (vec * offset);
                        // 塔に近すぎたら補正
                        Vector3 T_pos = new Vector3(Tower.transform.position.x, SpawnPosition.y, Tower.transform.position.z);
                        float T_distance = Vector3.Distance(T_pos, SpawnPosition);
                        if (T_distance < SpawnRadius)
                        {
                            Vector3 T_vec = (SpawnPosition - T_pos).normalized;
                            SpawnPosition = T_pos + T_vec * SpawnRadius;
                        }
                        Debug.Log(SpawnPosition);
                        EnemyCreate(SpawnPosition,SelectedEnemy, EnemyLevels[SelectedEnemy]);
                        //photonView.RPC(nameof(Create), RpcTarget.All, SpawnPosition, transform.rotation,
                        //    SelectedEnemy, EnemyLevels[SelectedEnemy]);
                        //photonView.RPC(nameof(Create), RpcTarget.All, SpawnPosition, transform.rotation, SelectedEnemy);
                        //audioSource.PlayOneShot(Appear_SE);

                        //Debug.Log(raycastHitList.First().collider.gameObject.tag);
                    }
                }
            }

            if (Input.GetMouseButton(0) && Mana >= cost&& !IsMouseOver) //マウス左ボタンが押されている間、マナが足りていれば
            {

                var ray = camera.ScreenPointToRay(Input.mousePosition); //レイの方向を定義
                var raycastHitList = Physics.RaycastAll(ray).ToList();      //レイのヒット先を探索

                foreach (RaycastHit it in raycastHitList)
                {
                    if (it.collider.gameObject.tag == "Ground")
                    {
                        var distance = Vector3.Distance(camera.transform.position, raycastHitList.First().point);
                        float height_A = camera.transform.position.y - raycastHitList.First().point.y;
                        float height_B = Player.transform.position.y - raycastHitList.First().point.y;
                        float offset = height_B / height_A * distance;
                        Vector3 vec = (camera.transform.position - raycastHitList.First().point).normalized;

                        currentPosition = raycastHitList.First().point + (vec * offset);
                        // 塔に近すぎたら補正
                        Vector3 T_pos = new Vector3(Tower.transform.position.x, currentPosition.y, Tower.transform.position.z);
                        float T_distance = Vector3.Distance(T_pos, currentPosition);
                        if (T_distance < SpawnRadius)
                        {
                            Vector3 T_vec = (currentPosition - T_pos).normalized;
                            currentPosition = T_pos + T_vec * SpawnRadius;
                        }

                        float dist = Vector3.Distance(SpawnPosition, currentPosition);
                        if (dist > SpawnInterval)
                        {
                            Mana -= cost;   // コスト分マナ減らす
                            SpawnPosition = currentPosition;

                            EnemyCreate(SpawnPosition,SelectedEnemy, EnemyLevels[SelectedEnemy]);
                            //object[] initdata = { SelectedEnemy, EnemyLevels[SelectedEnemy] };
                            //GameObject e = PhotonNetwork.InstantiateRoomObject("Enemy1", SpawnPosition, transform.rotation, 0, initdata);
                            //e.GetComponent<EnemyScript01>().SetTarget(Tower, Player);
                            //e.GetComponent<EnemyScript01>().SetData(
                            //            EnemyDatas[SelectedEnemy, EnemyLevels[SelectedEnemy]].HP,
                             //           EnemyDatas[SelectedEnemy, EnemyLevels[SelectedEnemy]].ATK,
                             //           EnemyDatas[SelectedEnemy, EnemyLevels[SelectedEnemy]].SkillPower,
                             //           EnemyDatas[SelectedEnemy, EnemyLevels[SelectedEnemy]].ManaVolume);

                            //photonView.RPC(nameof(Create), RpcTarget.All, SpawnPosition, transform.rotation,
                            //    SelectedEnemy, EnemyLevels[SelectedEnemy]);
                            //photonView.RPC(nameof(Create), RpcTarget.All, SpawnPosition, transform.rotation, SelectedEnemy);
                            //audioSource.PlayOneShot(Appear_SE);
                        }
                    }
                }
            }

            //死亡判定
            if (IsDeath())
            {
                photonView.RPC(nameof(SendChange), RpcTarget.Others);
                //
                SceneManager.LoadScene("Lose");
                //PhotonNetwork.Disconnect();
            }

            //特殊勝利
            if (Alone())
            {
                //PhotonNetwork.Disconnect();
                SceneManager.LoadScene("Win");
            }
        }
    }

    // レベルアップ
    public bool EnemyLevelup(int e_num)
    {
        if (EnemyLevels[e_num] >= 4) return false;      // 最大レベルならレベルアップできない（一応）

        int cost = GetEnemyData(e_num, EnemyLevels[e_num]).LevelupCost; // 強化コスト
        if (Mana >= cost)
        {
            Mana -= cost;
            EnemyLevels[e_num]++;
            return true;    // レベルアップできた
        }

        return false;   // レベルアップできない
    }

    public bool ManaLevelup()
    {
        if (ManaLevel >= Mana_MaxLevel) return false;       // マナレベルが最大ならレベルアップできない

        if(Mana >= ManaLevelUpCost[ManaLevel])
        {
            Mana -= ManaLevelUpCost[ManaLevel];
            ManaLevel++;
            Mana_AppSpeed = Mana_Heal_Number[ManaLevel];
            return true;
        }

        return false;   // レベルアップできない
    }

    //--------------------UIのボタンに対応したものを出すよう----------------------//
    public void OnclickButton1()
    {
        SelectedEnemy = 0;
        //Debug.Log("NowSelect Is Number 0");
    }

    public void OnClickButton2()
    {
        SelectedEnemy = 1;
    }

    public void OnClickButton3()
    {
        SelectedEnemy = 2;
    }

    [PunRPC]
    private void Create(Vector3 SpawnPoint,Quaternion Rotation, int e_num, int lv)
    {
        GameObject e = Instantiate(Enemies[e_num], SpawnPoint, Rotation);
        e.GetComponent<EnemyScript01>().SetTarget(Tower, Player);
        e.GetComponent<EnemyScript01>(). SetData(
                    EnemyDatas[e_num, lv].HP,
                    EnemyDatas[e_num, lv].ATK,
                    EnemyDatas[e_num, lv].SkillPower,
                    EnemyDatas[e_num, lv].ManaVolume);
        e.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
        //e.GetComponent<PhotonView>().SetOwnerInternal(PhotonNetwork.LocalPlayer, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    //----------------------------------------------------------------------------//

    public ref EnemyData GetEnemyData(int num, int lv)  // 参照渡し・返し
    {
        return ref EnemyDatas[num, lv];
    }

    private void Initialize()
    {
        HP = HP_MAX;
        Mana = 0;
        time_Left = 0.0f;
    }

    //死亡判定
    private bool IsDeath()
    {
        if(HP <= 0)
        {
            return true;
        }
        return false;
    }
    private void Nakanishi_Update()
    {
        //１秒ごとに呼ぶ
        time_Left += Time.deltaTime;
        if (time_Left >= 1.0f)
        {
            time_Left = 0.0f;
            Mana += Mana_AppSpeed;
            //Debug.Log(Mana);
            if (Mana > Mana_MAX)
            {
                Mana = Mana_MAX;
            }
        }
    }

    public int GetCost(int EnemyNumber,bool NowCreate_)
    {
        if (EnemyNumber > 2) return 0;
        if (NowCreate_) { return EnemyDatas[EnemyNumber, EnemyLevels[EnemyNumber]].SpawnCost; }
        return EnemyDatas[EnemyNumber, EnemyLevels[EnemyNumber]].LevelupCost;
    }

    public int GetManaCost()
    {
        return ManaLevelUpCost[ManaLevel];
    }

    private bool Alone()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            return true;
        }
        return false;
    }

    [PunRPC]
    public void SendChange()
    {
        SceneManager.LoadScene("Win");
        //PhotonNetwork.Disconnect();
    } 

    private void EnemyCreate(Vector3 p,int enemy_type, int enemy_level)
    {
        object[] initdata = {
            EnemyDatas[enemy_type, enemy_level].HP,
            EnemyDatas[enemy_type, enemy_level].ATK,
            EnemyDatas[enemy_type, enemy_level].SkillPower,
            EnemyDatas[enemy_type, enemy_level].ManaVolume,
            Tower.transform.position,
            };

        //PhotonNetwork.Instantiate("Enemy1", p,Quaternion.identity, 0, initdata);
        //GameObject e;
        switch (enemy_type)
        {
            case 0:
                PhotonNetwork.Instantiate("Enemy1", SpawnPosition, transform.rotation, 0, initdata);
                break;
            case 1:
                PhotonNetwork.Instantiate("Enemy2", SpawnPosition, transform.rotation, 0, initdata);
                break;
            case 2:
                PhotonNetwork.Instantiate("Enemy3", SpawnPosition, transform.rotation, 0, initdata);
                break;
        }
        //e.GetComponent<EnemyScript01>().SetTarget(Tower, Player);
        //e.GetComponent<EnemyScript01>().SetData(
        //            EnemyDatas[enemy_type, enemy_level].HP,
        //            EnemyDatas[enemy_type, enemy_level].ATK,
        //            EnemyDatas[enemy_type, enemy_level].SkillPower,
        //            EnemyDatas[enemy_type, enemy_level].ManaVolume);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(HP);
        }
        else
        {
            HP = (float)stream.ReceiveNext();
        }
    }

    //三宅追加
    public int GetLevel(int Enemy)
    {
        return EnemyLevels[Enemy]+1;
    }

    public int GetManaLevel()
    {
        return ManaLevel + 1;
    }
}
