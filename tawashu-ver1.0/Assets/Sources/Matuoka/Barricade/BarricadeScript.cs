using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class BarricadeScript : MonoBehaviourPunCallbacks
{
    public GameObject Target;
    public GameObject Tower;
    private GameObject LookTarget;
    private Vector3 defPosition;        //defPositionをVector3で定義する。

    public float Speed = 50;
    public float Timer = 5;

    public int BarricadeHp = 0;


    private Vector3 ToTarget;



    // Start is called before the first frame update
    void Start()
    {
        ToTarget = Target ? (Target.transform.position - transform.position).normalized : transform.forward;

        //  常にタワーに向いて一定の回転
        defPosition = Tower.transform.position;    //タワーから円運動を始める

        GameObject object1 = GameObject.FindWithTag("Tower");
        LookTarget = object1;
        LookTarget.transform.position = defPosition;

    }

    // Update is called once per frame
    void Update()
    {
        // 消滅
        //Timer -= Time.deltaTime;
        //if (Timer < 0) Destroy(gameObject);

        //  常にタワーに向いて一定の回転w
        if (LookTarget)
        {
            var direction = LookTarget.transform.position - transform.position;
            direction.y = 0;   // x、z軸が回転するのを防ぐ

            var lookRotation = Quaternion.LookRotation(-direction, Vector3.up);              //  directionを-にしてZ軸が前方に向くように調整
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.3f);    //  第3引数で振り向く速さの調整
        }

        //  耐久地が0になったら消す
        if (BarricadeHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    //  当たり判定 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "E_Bullet")
        {
            BarricadeHp -= 50;
            //GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
            //foreach (var obj in e)
            // {
            //     obj.GetComponent<BulletScript01>().GetDmageValue();
            // }
            Destroy(other.gameObject);
        }
    }

    public void SetTarget(GameObject target) { Target = target; }
    public void SetHpValue(int hp) { BarricadeHp = hp; }
    public int GetHpValue() { return BarricadeHp; }

}
