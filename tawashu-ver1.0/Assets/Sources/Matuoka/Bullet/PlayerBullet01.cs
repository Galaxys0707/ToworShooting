using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PlayerBullet01 : MonoBehaviourPunCallbacks
{

    //private EnemySpawn enemy;
    public GameObject Target;
    public float Speed = 50;
    public float Timer = 5;
    public int EnemyDmageValue = 200;
    public int BossCoreDamage = 20;

    private Vector3 ToTarget;

    // Start is called before the first frame update
    void Start()
    {
        //enemy = GameObject.FindWithTag("AttackPlayer").GetComponent<EnemySpawn>();
        ToTarget = Target ? (Target.transform.position - transform.position).normalized : transform.forward;
        // ToTarget.y = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // 移動
        transform.position += ToTarget * (Speed * Time.deltaTime);

        // 消滅
        Timer -= Time.deltaTime;
        if (Timer < 0) Destroy(gameObject);
    }

    // 当たり判定
    void OnTriggerEnter(Collider other)
    {
        //// 敵
        if (other.gameObject.tag == "Enemy")
        {
            //Destroy(other.gameObject);
        }

        // 塔
        if (other.gameObject.tag == "Tower")
        {
            
            Destroy(gameObject);
        }
    }

   public void SetTarget(GameObject target) { Target = target; }
   public void SetDmageValue(int damage) { EnemyDmageValue = damage; }
   public int GetDmageValue() { return EnemyDmageValue; }
}
