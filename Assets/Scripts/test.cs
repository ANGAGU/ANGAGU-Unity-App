using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // this가 가리키는 놈은 현재 이 스크립트가 연결되어있는 게임 오브젝트이다.
            this.transform.position = this.transform.position + new Vector3(0, 0, 1);
        }
    }
}
