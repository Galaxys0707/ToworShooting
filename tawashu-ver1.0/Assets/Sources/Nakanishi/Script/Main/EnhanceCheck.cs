using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceCheck : MonoBehaviour
{
    private EnemySpawn enemy;
    private bool NowCreatePanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy==null)
        {
            enemy = GameObject.FindWithTag("AttackPlayer").GetComponent<EnemySpawn>();
        }
    }

    public void OnClick()
    {
        enemy.NowCreate = !enemy.NowCreate;
    }
}
