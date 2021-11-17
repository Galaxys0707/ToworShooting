using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefencePlayer_UI : MonoBehaviour
{
    private Player player;
    private EnemySpawn enemy;
    public Text HPText;
    public Text ManaText;
    public Slider HPBar;
    public Slider ManaBar;

    public Text OverHPText;
    public Slider OverHPSlider;

    public Text Text1;
    public Text Text2;
    public Text Text3;
    public Text Text4;
    public Text Text5;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("DefencePlayer").GetComponent<Player>();
        enemy = GameObject.FindWithTag("AttackPlayer").GetComponent<EnemySpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player==null)
        {
           player = GameObject.FindWithTag("DefencePlayer").GetComponent<Player>();
        }

        if(enemy== null)
        {
            enemy = GameObject.FindWithTag("AttackPlayer").GetComponent<EnemySpawn>();
        }
        HPBar.value = player.HP / player.HP_MAX;
        ManaBar.value = player.Mana / player.Mana_MAX;
        HPText.text = "" + player.HP;
        ManaText.text = "" + player.Mana;

        Debug.Log((player.GetStateLevelupCost_f(true)));

        //Debug.Log((player.GetState_f(true)));
        //Text1.text = "70";
        //Text1.text = "" + (player.GetState_f(true));
        //Text2.text = "" + (player.GetState(true));
        //Text3.text = "" + (player.GetState_f(false));
        //Text4.text = "" + (player.GetState(false));

        Text1.text = "" + (player.GetStateLevelupCost_f(true));
        Text2.text = "" + (player.GetStateLevelupCost(true));
        Text3.text = "" + (player.GetStateLevelupCost_f(false));
        Text4.text = "" + (player.GetStateLevelupCost(false));

        OverHPText.text = "" + enemy.HP + "/" + enemy.HP_MAX;
        OverHPSlider.value = enemy.HP / enemy.HP_MAX;
    }
}
