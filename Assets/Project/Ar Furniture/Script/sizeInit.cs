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
        resizing(new Vector3(2.0f, 0.5f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDrawGizmosSelected()
    {
        Bounds totalBounds = new Bounds();
        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            totalBounds.Encapsulate(meshRenderer.bounds);
        }
        Color temp = Color.red;
        temp.a = 0.3f;
        Gizmos.color = temp;
        Gizmos.DrawCube(totalBounds.center, totalBounds.size);
    }
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
        originModel.transform.localScale = new Vector3(resizeRate, resizeRate, resizeRate);
        originModel.transform.position = new Vector3(boundSize.x * resizeRate, boundSize.y * resizeRate, boundSize.z * resizeRate);
    }
}
