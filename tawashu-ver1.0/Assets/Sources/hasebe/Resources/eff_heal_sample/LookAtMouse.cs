using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LookAtMouse : MonoBehaviour
{

    private Camera mainCamera;                         // カメラ

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition); //レイの方向を定義
        var raycastHitList = Physics.RaycastAll(ray).ToList();      //レイのヒット先を探索

        if (raycastHitList.Any())//レイが何かに当たったかを取る
        {
            //当たった物との距離
            var distance = Vector3.Distance(mainCamera.transform.position, raycastHitList.First().point);

            float height_A = mainCamera.transform.position.y - raycastHitList.First().point.y;
            float height_B = transform.position.y - raycastHitList.First().point.y;
            float offset = height_B / height_A * distance;
            //カメラから当たった場所までの正規化済みベクトルを取る
            Vector3 vec = (mainCamera.transform.position - raycastHitList.First().point).normalized;

            Vector3 SpawnPosition = raycastHitList.First().point + (vec * offset);

            transform.LookAt(SpawnPosition);
            foreach(var obj in raycastHitList)//raycastHitList
            {
                if(obj.collider.gameObject.tag=="Trump")
                {

                }
            }
        }
    }
}
