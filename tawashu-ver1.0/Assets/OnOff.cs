using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOff : MonoBehaviour
{

    public GameObject obj;
    public Text tex;
    private bool Check;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        Check = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Check)
        {
            tex.text = "しまう";
            obj.SetActive(true);
        }
        else
        {
            tex.text = "操作方法";
            obj.SetActive(false);
        }
    }

    public void OnClick()
    {
        Check = !Check;
    }
}
