using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public EnemyScript01 Enemy1;

    private List<EnemyScript01> Enemy1_ActiveList = new List<EnemyScript01>();
    private Stack<EnemyScript01> Enemy1_inactivePool = new Stack<EnemyScript01>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=Enemy1_ActiveList.Count-1;i>=0;i--)
        {
            var ene1 = Enemy1_ActiveList[i];
            {
                if(ene1.IsActive)
                {
                    ene1.OnUpdate();
                }
                else
                {
                    Remove(ene1);
                }
            }
        }
    }

    public void Spawn(int id,int ownerId,Vector3 origin,Vector3 angle,int timestamp)
    {
        var e = (Enemy1_inactivePool.Count > 0) ? Enemy1_inactivePool.Pop() : Instantiate(Enemy1, transform);
        e.Active(id, ownerId, origin, angle, timestamp);
        Enemy1_ActiveList.Add(e);
    }

    public void Remove(EnemyScript01 e)
    {
        Enemy1_ActiveList.Remove(e);
        e.DeActivate();
        Enemy1_inactivePool.Push(e);
    }
}
