using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public BulletScript01 bulletprefab1;

    private List<BulletScript01> activeList = new List<BulletScript01>();
    private Stack<BulletScript01> inactivePool = new Stack<BulletScript01>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=activeList.Count-1;i>=0;i--)
        {
            var bullet = activeList[i];
            if(bullet.IsActive)
            {
                bullet.OnUpdate();
            }
            else
            {
                Remove(bullet);
            }
        }
    }

    public void Fire(int id, int ownerid, Vector3 origin, Vector3 angle, int timestamp)
    {
        var bullet = (inactivePool.Count > 0) ? inactivePool.Pop() : Instantiate(bulletprefab1, transform);
        bullet.Active(id, ownerid, origin, angle, timestamp);
        activeList.Add(bullet);
    }

    public void Remove(BulletScript01 bullet)
    {
        activeList.Remove(bullet);
        bullet.DeActivate();
        inactivePool.Push(bullet);
    }

    public void Remove(int id,int ownerid)
    {
        foreach(var bullet_ in activeList)
        {
            if(bullet_.Equals(id,ownerid))
            {
                Remove(bullet_);
                break;
            }
        }
    }
}
