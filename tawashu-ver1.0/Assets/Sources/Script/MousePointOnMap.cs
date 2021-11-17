
using System.Linq;
using UnityEngine;

public class MousePointOnMap : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 currentPosition = Vector3.zero;


    public GameObject ArmyPrefab1;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) //マウス左ボタンが押されている間
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition); //レイの方向を定義
            var raycastHitList = Physics.RaycastAll(ray).ToList(); //レイのヒット先を探索
            if (raycastHitList.Any())
            {
                var distance = Vector3.Distance(mainCamera.transform.position, raycastHitList.First().point); //レイの原点とヒット先の距離を計測
                var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance); //ヒット先の位置を定義

                currentPosition = mainCamera.ScreenToWorldPoint(mousePosition); //ヒット先の位置をワールド座標に直す
                Debug.Log(currentPosition);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (currentPosition != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(currentPosition, 0.5f);
        }
    }
}
