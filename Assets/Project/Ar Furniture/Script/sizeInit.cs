using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sizeInit : MonoBehaviour
{
    [SerializeField]
    private GameObject originModel;

    // Start is called before the first frame update
    void Start()
    {
        //*** 사이징 체크 ***//
        resizing(new Vector3(0.5f, 0.3f, 0.3f));
    }

    // Update is called once per frame
    void Update()
    {

    }
    //void OnDrawGizmosSelected()
    //{
    //    Bounds totalBounds = new Bounds();
    //    foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
    //    {
    //        totalBounds.Encapsulate(meshRenderer.bounds);
    //    }
    //    Color temp = Color.red;
    //    temp.a = 0.3f;
    //    Gizmos.color = temp;
    //    Gizmos.DrawCube(totalBounds.center, totalBounds.size);
    //}
    void resizing(Vector3 realSize)
    {
        originModel = GameObject.FindWithTag("OriginModel");
        Bounds totalBounds = new Bounds();
        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            totalBounds.Encapsulate(meshRenderer.bounds);
        }
        Debug.Log(totalBounds.size);

        Vector3 boundSize = totalBounds.size;

        float resizeRate = realSize.x / boundSize.x;
        transform.localScale = new Vector3(resizeRate, resizeRate, resizeRate);
        /*** 그림자 크기 및 모델 실제 사이즈 체크 ***/
        originModel.transform.localScale = new Vector3(resizeRate, resizeRate, resizeRate);
        originModel.transform.position = realSize;
    }
}
