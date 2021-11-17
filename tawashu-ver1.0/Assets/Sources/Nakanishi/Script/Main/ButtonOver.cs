using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //ButtonPanel
    public float num = 7;
    public GameObject panel;
    public float panelPos_y = 45;
    bool flag = false;

    //ButtonMana
    public GameObject Mana_Object = null;
    private UIManager manager;
    private EnemySpawn enemy;
    //static public int Mana = 10;
    //    public int Level = 1;
    //    public int Level_Max = 5;

    //    public int[] Mana= new int[] { 10, 20, 30, 40, 50 };

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy==null)
        {
            enemy = GameObject.FindGameObjectWithTag("AttackPlayer").GetComponent<EnemySpawn>();
        }
        MouseOver();

        Text test_Text = Mana_Object.GetComponent<Text>();
    }

    public void OnClick1()
    {
        if(enemy.NowCreate)
        {
            enemy.SelectedEnemy = 0;
        }
        else
        {
            if(enemy.EnemyDatas[0,enemy.EnemyLevels[0]].LevelupCost<=enemy.Mana)
            {
                enemy.EnemyLevelup(0);
            }
        }
        //manager.EnemySelectNumber = 0;
        Debug.Log("1が押された!");  // ログを出力
    }
    public void OnClick2()
    {
        if (enemy.NowCreate)
        {
            enemy.SelectedEnemy = 1;
        }
        else
        {
            if (enemy.EnemyDatas[1, enemy.EnemyLevels[1]].LevelupCost <= enemy.Mana)
            {
                enemy.EnemyLevelup(1);
            }
        }
            Debug.Log("2が押された!");  // ログを出力
    }
    public void OnClick3()
    {
        if (enemy.NowCreate)
        {
            enemy.SelectedEnemy = 2;
        }
        else
        {
            if (enemy.EnemyDatas[2, enemy.EnemyLevels[2]].LevelupCost <= enemy.Mana)
            {
                enemy.EnemyLevelup(2);
            }
        }
            Debug.Log("3が押された!");  // ログを出力
    }
    public void OnClick4()
    {
        //enemy.SelectedEnemy = 3;
        //manager.EnemySelectNumber = 3;
        enemy.ManaLevelup();
        Debug.Log("4が押された!");  // ログを出力
    }

    public void OnClick5()
    {
        //enemy.SelectedEnemy = 4;
        //manager.EnemySelectNumber = 4;
        enemy.NowCreate = !enemy.NowCreate;
        Debug.Log("5が押された！");
    }

    public void OnClick()
    {
        //Debug.Log("5が押された!");  // ログを出力
        //CheckSpawnOrEnhance.ChengeSpawnOrEnhance();
    }
    //関数
    void MouseOver()
    {
        if (flag)
        {
            Vector3 pos = panel.transform.localPosition;
            Vector3 scale = panel.transform.localScale;
            if (pos.y < panelPos_y)
            {
                pos.y += panelPos_y / num;
            }
            else pos.y = panelPos_y;

            if (scale.x < 1.0f)
            {
                scale.x += 1.0f / num;
                scale.y += 1.0f / num;
            }

            panel.transform.localPosition = pos;
            panel.transform.localScale = scale;
        }
        else

        {
            Vector3 pos = panel.transform.localPosition;
            Vector3 scale = panel.transform.localScale;
            if (pos.y > 0)
            {
                pos.y -= panelPos_y / num;
            }
            else pos.y = 0;

            if (scale.x > 0.0f)
            {
                scale.x -= 1.0f / num;
                scale.y -= 1.0f / num;
            }
            panel.transform.localPosition = pos;
            panel.transform.localScale = scale;
        }
    }

    // オブジェクトの範囲内にマウスポインタが入った際に呼び出されます。
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (enemy == null)
        {
            enemy = GameObject.FindGameObjectWithTag("AttackPlayer").GetComponent<EnemySpawn>();
        }
        Debug.Log("Enter");
        enemy.IsMouseOver = true;
        flag = true;

    }

    // オブジェクトの範囲内からマウスポインタが出た際に呼び出されます。
    public void OnPointerExit(PointerEventData eventData)
    {
        if (enemy == null)
        {
            enemy = GameObject.FindGameObjectWithTag("AttackPlayer").GetComponent<EnemySpawn>();
        }
        Debug.Log("Exit");
        flag = false;
        enemy.IsMouseOver = false;
    }
}
