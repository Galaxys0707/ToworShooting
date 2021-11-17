using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Player_Bullet_Nakanishi : MonoBehaviourPunCallbacks
{

    Vector3 origin;
    Vector3 velocity;
    float timestamp;
    public float Speed = 20f;
    private Player player;

    //public float Speed = 50;
    public float Timer = 5;
    private int ATK;
    public int EnemyDmageValue = 100;
    public int BossCoreDamage = 10;

    private const string Tag1 = "Enemy1";
    private const string Tag2 = "Enemy2";
    private const string Tag3 = "Enemy3";
    // Start is called before the first frame update
    void Start()
    {
        //BossCoreDamage=
        //Speed = 3.0f;
        EnemyDmageValue = 50;
        //BossCoreDamage = 5;
        player = GameObject.FindWithTag("DefencePlayer").GetComponent<Player>();
        BossCoreDamage = player.GetState(true);
        //Debug.Log(BossCoreDamage);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * (Speed * Time.deltaTime);

        // 消滅
        Timer -= Time.deltaTime;
        if (Timer < 0) Destroy(gameObject);
        //transform.position = new Vector3(transform.position.x + 0.1f, 0.0f, 0.0f);
        //float elapsedtime = Mathf.Max(0f, unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f);
        //transform.position = origin + velocity * Speed * elapsedtime;
        //transform.Translate(1f, 0, 0);
    }

    public void Init(Vector3 velocity)
    {
        //this.origin = origin;
        this.velocity = velocity;
        //this.timestamp = timestamp;

        //Debug.Log(velocity);
        //transform.position = origin;
    }

    void OnTriggerEnter(Collider other)
    {
        if(player==null)
        {
            player = GameObject.FindWithTag("DefencePlayer").GetComponent<Player>();
        }

        //// 敵
        if (other.gameObject.tag == Tag1)
        {
            Destroy(gameObject);
        }
        if (other.gameObject.tag == Tag2)
        {
            Destroy(gameObject);
        }
        if (other.gameObject.tag == Tag3)
        {
            Destroy(gameObject);
        }
        if(other.gameObject.tag=="BossCore")
        {
            Destroy(gameObject);
        }
    }

    public void ATK_Set(int atk)
    {
        ATK = atk;
    }

    public int Get_ATK()
    {
        return ATK;
    }
}
