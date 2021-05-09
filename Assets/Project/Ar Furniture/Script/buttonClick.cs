using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonClick : MonoBehaviour
{
    public GameObject cube;
    private Rigidbody myRigid;
    // Start is called before the first frame updat

    public void buttonOnClick()
    {
        myRigid = cube.GetComponent<Rigidbody>();
        myRigid.useGravity = true;
        
    }
}
