using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Tower : MonoBehaviourPunCallbacks
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Defence_Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //エネミーの弾
        if (other.gameObject.tag == "E_Bullet")
        {
            if(player==null)
            {
                player = GameObject.FindWithTag("DefencePlayer").GetComponent<Player>();
            }
            int d = other.GetComponent<BulletScript01>().DmageValue;
            player.HP -= d;
            //Debug.Log("HP : " + player.HP + "| Damage :" + d);
        }
    }
}
