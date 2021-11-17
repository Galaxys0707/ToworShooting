using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class UI_Change : MonoBehaviourPunCallbacks
{
    public GameObject Attack;
    public GameObject Defence;

    private GameObject obj;
    private EnemySpawn enemy;
    // Start is called before the first frame update
    void Start()
    {
        Attack.SetActive(false);
        Defence.SetActive(false);

        obj = GameObject.FindWithTag("AttackPlayer");
        //enemy = obj.GetComponent<EnemySpawn>();
        DontDestroyOnLoad(gameObject);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Attack.SetActive(true);
            Defence.SetActive(false);
        }
        else
        {
            Attack.SetActive(false);
            Defence.SetActive(true);
        }
//        Debug.Log(enemy.SelectedEnemy);
    }

    public void OnClickButton1()
    {
        //UI.OnClickButton1();
        enemy.SelectedEnemy = 0;
        Debug.Log("Push 1");
        Debug.Log(enemy.SelectedEnemy+"が押された");
    }

    public void OnClickButton2()
    {
        //UI.OnClickButton2();
        enemy.SelectedEnemy = 1;
        Debug.Log("Push 2");
        Debug.Log(enemy.SelectedEnemy);
    }

    public void OnClickButton3()
    {
        //UI.OnClickButton3();
        enemy.SelectedEnemy = 2;
        Debug.Log(enemy.SelectedEnemy);
    }
}
