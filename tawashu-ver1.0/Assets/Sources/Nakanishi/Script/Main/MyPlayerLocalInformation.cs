using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerLocalInformation : MonoBehaviour
{

    //これをもとにどっち側か判別
    private bool SelectAttack;
    private bool Push;
    public Text Wait;

    public Button Sword;
    public Button Shield;
    // Start is called before the first frame update
    void Start()
    {
        Push = false;
        Wait.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(Push)
        {
            Wait.text = "他のプレイヤーの選択またはほかのプレイヤーの参加を待っています";
        }
        if(Push)
        {
            //用済み消去
            Text.Destroy(Wait);
            Destroy(this.gameObject);
            Button.Destroy(Shield);
            Button.Destroy(Sword);
        }

    }

    public void OnClickAttack()
    {
        SelectAttack = true;
        
        Push = true;
    }

    public void OnClickDefence()
    {
        SelectAttack = false;
        Push = true;
    }
}
