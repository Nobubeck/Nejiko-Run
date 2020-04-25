using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    Vector3 diff;
    //追従ターゲットプロパティ 
    public GameObject target;
    public float followSpeed;
    // Start is called before the first frame update
    void Start()
    {
        diff = target.transform.position - transform.position;
    }

    //updateの処理が終わった後に実行 
    void LateUpdate()
    {
        //距離が離れているほど、早くが近づく、遅いと逆 
        transform.position = Vector3.Lerp(
            transform.position,
            target.transform.position - diff,
            Time.deltaTime * followSpeed
        );
    }

    // Update is called once per frame
    void Update()
    {

    }
}
