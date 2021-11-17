using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillBuff : MonoBehaviour
{

    public float DestroyTimer = 2.0f;       // 消えるまでの時間
    public float BuffMag = 1.2f;            // 攻撃力増加倍率

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DestroyTimer -= Time.deltaTime;
        if (DestroyTimer <= 0) Destroy(gameObject);
    }

    public void SetBuffMag(float bm) { BuffMag = bm; }


}
