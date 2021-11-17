using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillHeal : MonoBehaviour
{

    public float DestroyTimer = 2.0f;       // 消えるまでの時間
    public float HealMag = 0.5f;            // 回復倍率

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

    public void SetHealMag(float hm) { HealMag = hm; }
}
