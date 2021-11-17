using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Photon.Pun;
using Photon.Realtime;

public class LookAtMouse2 : MonoBehaviourPunCallbacks
{
    // private Camera mainCamera;                         // カメラ
    private GameObject CameraObj;
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        CameraObj = GameObject.FindWithTag("DefenceCamera");
        camera = CameraObj.GetComponent<Camera>();
        //camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (camera == null)
            {
                camera = GameObject.FindWithTag("DefenceCamera").GetComponent<Camera>();
            }
            var ray = camera.ScreenPointToRay(Input.mousePosition);      //レイの方向を定義
            var raycastHitList = Physics.RaycastAll(ray).ToList();           //レイのヒット先を探索

            foreach (RaycastHit it in raycastHitList)
            {
                if (it.collider.gameObject.tag == "Ground")
                {
                    var distance = Vector3.Distance(camera.transform.position, raycastHitList.First().point);
                    float height_A = camera.transform.position.y - raycastHitList.First().point.y;
                    float height_B = transform.position.y - raycastHitList.First().point.y;
                    float offset = height_B / height_A * distance;
                    Vector3 vec = (camera.transform.position - raycastHitList.First().point).normalized;

                    Vector3 SpawnPosition = raycastHitList.First().point + (vec * offset);

                    transform.LookAt(SpawnPosition);
                }
            }
        }

    }
}
