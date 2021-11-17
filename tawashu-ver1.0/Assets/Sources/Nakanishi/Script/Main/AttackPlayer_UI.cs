using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AttackPlayer_UI : MonoBehaviour
{
    public Slider HPBar;
    public Slider ManaBar;
    private EnemySpawn obj;
    private Player player;
    public Text HPText;
    public Text ManaText;
    public Text OverHP;
    public Slider OverSlider;

    public GameObject PowerupButton;
    public GameObject CreateButton;

    //必要なUI　追加してください　１個ずつのほうが使いやすい　けどめんどくさい
    public Text Text1;
    public Text Text2;
    public Text Text3;
    public Text Text4;
    public Text Text5;

    public Text[] LvText;

    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.FindWithTag("AttackPlayer").GetComponent<EnemySpawn>();
        player = GameObject.FindWithTag("DefencePlayer").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(obj==null)
        {
            obj = GameObject.FindWithTag("AttackPlayer").GetComponent<EnemySpawn>();
        }
        if (player == null)
        {
            player = GameObject.FindWithTag("DefencePlayer").GetComponent<Player>();
        }

        HPBar.value = obj.HP / obj.HP_MAX;
        ManaBar.value = obj.Mana / obj.Mana_MAX;

        HPText.text = "" + obj.HP;
        ManaText.text = "" + obj.Mana;

        //生成モードかそうじゃないかでUIの数字変更
        Text1.text = "" + obj.GetCost(0, obj.NowCreate);
        Text2.text = "" + obj.GetCost(1, obj.NowCreate);
        Text3.text = "" + obj.GetCost(2, obj.NowCreate);
        Text4.text = "" + obj.ManaLevelUpCost[obj.ManaLevel];

        OverHP.text = "" + player.HP + "/" + player.HP_MAX;
        OverSlider.value = player.HP / player.HP_MAX;
        //Panel
        if(obj.NowCreate)
        {
            CreateButton.SetActive(true);
            PowerupButton.SetActive(false);
        }
        else
        {
            CreateButton.SetActive(false);
            PowerupButton.SetActive(true);
        }

        for (int i = 0; i < LvText.Length - 1; i++)
        {
            LvText[i].text = "Lv." + obj.GetLevel(i);
        }

        LvText[LvText.Length - 1].text = "Lv." + obj.GetManaLevel();
    }
}
