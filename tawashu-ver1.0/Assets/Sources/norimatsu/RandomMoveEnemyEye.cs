using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine.SceneManagement;

public class RandomMoveEnemyEye : MonoBehaviourPunCallbacks
{
    enum Direction{
        UP,
        LEFT,
        DOWN,
        RIGHT
    }
    
    private EnemySpawn enemy;
    private Player player;
           
    //逃走までの回数　（数ぶんあたれば移動
    public const int EscapeCount = 10;
    //ヒットした際のカウンター
    private int HitCounter = 0;
    //出現位置決定用半径
    public int Sphere_Radius = 4;
    //

    //public float HP;
    //private Direction Dir;
    //private Vector2 Pos;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        //SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        HitCounter = 0;
        teleport();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                teleport();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                //SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
                //SceneManager.LoadScene("Win");
            }
        }
        */
    }

    void teleport()
    {
        //-1.0f～1.0fの間で乱数とって方角決めて　正規化した値を上げる
        Vector3 d = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;

        Debug.Log(d);
        //向きに値かける
        d *= Sphere_Radius;
        //オブジェクトにセット
        gameObject.transform.position = d;

        /*
        Random.InitState(System.DateTime.Now.Millisecond);

        Dir = (Direction)Random.Range(1, 4 + 1);

        Pos.x = Random.Range(-19, 19 + 1);
        Pos.y = Random.Range(-19, 19 + 1);
        
        switch(Dir)
        {
            case (Direction)0:Pos.y = -19; break;
            case (Direction)1:Pos.x = -19; break;
            case (Direction)2:Pos.y = 19; break;
            case (Direction)3:Pos.x = 19; break;
        }

        this.gameObject.transform.position = new Vector3(Pos.x, 1, Pos.y);
        */
    }


    //未完成
    void OnTriggerEnter(Collider collision)
    {
        //当たった時ボスのHP減らす処理抜け
        if (collision.tag == "P_Bullet")
        {
            if (enemy == null)
            {
                enemy = GameObject.FindWithTag("AttackPlayer").GetComponent<EnemySpawn>();
            }
            //if (player== null)
            //{
            //    player = GameObject.FindWithTag("DefencePlayer").GetComponent<Player>();
            //}
            enemy.HP -= collision.GetComponent<Player_Bullet_Nakanishi>().Get_ATK();
            HitCounter++;
            if (HitCounter >= EscapeCount)
            {
                teleport();
                HitCounter = 0;
            }
            Debug.Log("EnemyHP" + enemy.HP);
        }
        //Debug.Log("HITTTTTTTTTTTTTTTTTTTTTTTTTTTT");
    }
}
