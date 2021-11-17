using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceUIButton_Only : MonoBehaviour
{
    private Player player;
    private bool NowCreatePanel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("DefencePlayer").GetComponent<Player>();
        }
    }

    public void OnClick()
    {
        player.NowCreate = !player.NowCreate;
    }
}
