using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    public GameObject HPBar = null;//フレーム
    public GameObject HPbar = null;//本体
    static public float Percentage = 1;//０から１まで

    //HPbarの座標は、Percentageが１なら０。０なら‐５になります。

    //デバッグ用
    float iaia = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector = HPbar.transform.position;
        Vector3 scale = HPbar.transform.localScale;

        vector.x = (7.5f * Percentage)-7.5f;
        scale.x = Percentage;
        HPbar.transform.position = vector;
        HPbar.transform.localScale = scale;

        if(Input.GetKey(KeyCode.X))
        {
            iaia = iaia-0.01f;

            SetPercentage(iaia);
            Debug.Log(iaia);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            iaia = iaia + 0.01f;

            SetPercentage(iaia);
            Debug.Log(iaia);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            iaia = 1;

            SetPercentage(iaia);
            Debug.Log(iaia);
        }
    }

    //他所で使う関数
    static void SetPercentage(float P)//０から１まで
    {
        if (P > 1) Percentage = 1;
        else if (P < 0) Percentage = 0;
        else Percentage = P;
    }
}