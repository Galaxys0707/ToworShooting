using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public int EnemySelectNumber;

    public EnemySpawn enemyspawn;
    //簡易　見やすくするよう？
    public enum PushName
    {
        Default_Number = -1,
        UI_Push1,
        UI_Push2,
        UI_Push3,
        UI_Push4,
        UI_Push5,
    }

    //UIのボタンクリック用 今の状態チェック
    public PushName NowName;

    // Start is called before the first frame update
    void Start()
    {
        NowName = PushName.Default_Number;
    }

    // Update is called once per frame
    void Update()
    {
    
    }


    //UI　Pushed Button Call
    public void OnClickButton1()
    {
        this.SetNumber(0);
    }

    public void OnClickButton2()
    {
        this.SetNumber(1);
    }

    public void OnClickButton3()
    {
        this.SetNumber(2);
    }

    public void OnClickButton4()
    {
        this.SetNumber(3);
    }

    public void OnClickButton5()
    {
        this.SetNumber(4);
    }


    private void SetNumber(int Number)
    {
        switch(Number)
        {
            case 0:
                NowName = PushName.UI_Push1;
                EnemySelectNumber = (int)PushName.UI_Push1;
                break;
            case 1:
                NowName = PushName.UI_Push2;
                EnemySelectNumber = (int)PushName.UI_Push2;
                break;
            case 2:
                NowName = PushName.UI_Push3;
                EnemySelectNumber = (int)PushName.UI_Push3;
                break;
            case 3:
                NowName = PushName.UI_Push4;
                EnemySelectNumber = (int)PushName.UI_Push4;
                break;
            case 4:
                NowName = PushName.UI_Push5;
                EnemySelectNumber = (int)PushName.UI_Push5;
                break;
        }
    }
}
