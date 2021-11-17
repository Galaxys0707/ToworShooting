using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    public GameObject Mana_Object = null;
    static public int Mana=0;
    static public int Mana_Max = 300;
    // Start is called before the first frame update
    void Start()
    {
        Mana = 0;
        Mana_Max = 300;
    }

    // Update is called once per frame
    void Update()
    {
        Text test_Text = Mana_Object.GetComponent<Text>();
        if(Mana>999) test_Text.text = Mana.ToString("0000");
        else if(Mana>99) test_Text.text = Mana.ToString("000");

        //デバッグ用
        if(Mana< Mana_Max) Mana++;

    }

    //他所で使う関数

    static void SetMana(int NowMana)//マナが変わるたび呼び出し
    {
        Mana = NowMana;
    }

    static void SetManaMax(int Max)//マナの最大値が変わるたび呼び出し
    {
        Mana_Max = Max;
    }
}
