using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript01 : MonoBehaviour
{
    private Vector3 StartPos;
    private Vector3 RelayPos;       // 中継座標
    private Vector3 RelayPos2;       // 中継座標
    private GameObject EndObject;

    public float AntiForce;         // 反対方向への力
    public float AntiRange;         // 反対方向へ行く範囲

    public float Counter;
    private float Timer;

    
    // Start is called before the first frame update
    void Start()
    {
        Timer = 0;
        transform.LookAt(EndObject.transform.position);
        float dist = Vector3.Distance(StartPos, EndObject.transform.position);
        //AntiForce *= dist;
        RelayPos = StartPos
            + (-transform.forward * Random.Range(AntiForce / 10.0f, AntiForce))
            + (transform.up * Random.Range(-AntiRange, AntiRange))
            + (transform.right * Random.Range(-AntiRange, AntiRange));

        RelayPos2 = StartPos
            + (transform.forward * Random.Range(0, dist))
            + (transform.up * Random.Range(RelayPos.y * 0.2f, RelayPos.y * 0.8f))
            + (transform.right * Random.Range(-AntiRange, AntiRange));

    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime / Counter;
        if (Timer > 1.0f) Timer = 1.0f;
        transform.position = GetPoint(transform.position, RelayPos, RelayPos2, EndObject.transform.position, Timer);
    }

    public void SetObject(Vector3 s, GameObject obj) { StartPos = s; EndObject = obj; }

    Vector3 GetPoint(Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = Vector3.Lerp(p1, p2, t);
        Vector3 b = Vector3.Lerp(p2, p3, t);
        return Vector3.Lerp(a, b, t);
    }

    Vector3 GetPoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        Vector3 a = Vector3.Lerp(p1, p2, t);
        Vector3 b = Vector3.Lerp(p2, p3, t);
        Vector3 c = Vector3.Lerp(p3, p4, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(d, e, t);
    }
}
