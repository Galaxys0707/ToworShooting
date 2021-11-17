
using System.Linq;
using UnityEngine;
using UnityEditor;

public class ClickToCreateArmy : MonoBehaviour
{
    //private Vector3 currentPosition = Vector3.zero;
    private Vector3 mousePosition_offset = Vector3.zero;

    public Camera SubCamera;
    public GameObject ArmyPrefab1;

    void Start()
    {
       // SubCamera = Camera.Find("SubCamera");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int layerMask = 1 << LayerMask.NameToLayer("Ground");

            var ray = SubCamera.ScreenPointToRay(Input.mousePosition);
            var raycastHitList = Physics.RaycastAll(ray).ToList();
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                var distance = Vector3.Distance(SubCamera.transform.position, raycastHitList.First().point);
                float height_A = SubCamera.transform.position.y - raycastHitList.First().point.y;
                float height_B = 0 - raycastHitList.First().point.y;
                float offset = height_B / height_A * distance;
                Vector3 vec = (SubCamera.transform.position - raycastHitList.First().point).normalized;


                mousePosition_offset = raycastHitList.First().point + (vec * offset);
            }

            //for (var it = raycastHitList.First(); it != raycastHitList.Last(); it++)
            //{
            //    if(it.collider.CompareTag("Ground"))
            //    {
            //        var distance = Vector3.Distance(SubCamera.transform.position, raycastHitList.First().point);
            //        float height_A = SubCamera.transform.position.y - raycastHitList.First().point.y;
            //        float height_B = 0 - raycastHitList.First().point.y;
            //        float offset = height_B / height_A * distance;
            //        Vector3 vec = (SubCamera.transform.position - raycastHitList.First().point).normalized;


            //        mousePosition_offset = raycastHitList.First().point + (vec * offset);
            //    }
            //}


 //           if (raycastHitList.Any())
 //           {
 //               var distance = Vector3.Distance(SubCamera.transform.position, raycastHitList.First().point);
 //               float height_A = SubCamera.transform.position.y - raycastHitList.First().point.y;
 //               float height_B = 0 - raycastHitList.First().point.y;
 //               float offset = height_B / height_A * distance;
 //               Vector3 vec = (SubCamera.transform.position - raycastHitList.First().point).normalized;


 //               mousePosition_offset = raycastHitList.First().point + (vec * offset);
 //               //var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mousePosition_offset.z);
                
 //               //currentPosition = mainCamera.ScreenToWorldPoint(mousePosition);
 ////               currentPosition.y = 0.3f;
 //           }

            Instantiate(ArmyPrefab1, mousePosition_offset, Quaternion.identity);
        }
    }

}
