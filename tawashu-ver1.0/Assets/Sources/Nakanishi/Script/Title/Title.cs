using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    //透明度変更用　変数たち
    public Text tex;
    bool alpha_flag;
    float alpha_color;

    public string LoadSceneName = "Lobby";

    // Start is called before the first frame update
    void Start()
    {
        alpha_color = 1;
        tex.color = new Color(255, 0, 0, alpha_color);
        alpha_flag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (alpha_flag)
            alpha_color -= Time.deltaTime;
        else
            alpha_color += Time.deltaTime;

        if (alpha_color < 0)
        {
            alpha_color = 0;
            alpha_flag = false;
        }
        else if (alpha_color > 1)
        {
            alpha_color = 1;
            alpha_flag = true;
        }

        tex.color = new Color(255, 0, 0, alpha_color);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeScene();
        }
    }

    //雑に関数化
    private void ChangeScene()
    {
        SceneManager.LoadScene(LoadSceneName);
    }
}
