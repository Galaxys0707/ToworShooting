using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class MOve : MonoBehaviourPunCallbacks
{

    Vector3 origin;
    Vector3 velocity;
    float timestamp;
    public float Speed = 20f;

    //public float Speed = 50;
    public float Timer = 5;
    public int EnemyDmageValue = 200;
    public int BossCoreDamage = 20;
    // Start is called before the first frame update
    void Start()
    {
        //Speed = 3.0f;
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

    public void Init(Vector3 origin,Vector3 velocity,float timestamp)
    {
        this.origin = origin;
        this.velocity = velocity;
        this.timestamp = timestamp;

        Debug.Log(velocity);
        //transform.position = origin;
    }

    void OnTriggerEnter(Collider other)
    {
        //// 敵
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }

        // 塔
        if (other.gameObject.tag == "Tower")
        {

            //Destroy(gameObject);
        }
    }
}
