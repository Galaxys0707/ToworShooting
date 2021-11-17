using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defence_ChoosePanel : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("DefencePlayer").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
