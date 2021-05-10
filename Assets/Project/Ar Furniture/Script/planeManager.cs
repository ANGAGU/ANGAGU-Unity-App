using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class planeManager : MonoBehaviour
{
    public ARPlane _ARPlane;
    public MeshRenderer _PlaneMeshRenderer;
    public TextMesh _TextMesh;
    public GameObject _TextObj;
    GameObject _mainCam;

    public ARPlaneManager arPlaneManager;

    private List<ARPlane> arPlanes;
    // Start is called before the first frame update
    void Start()
    {
        _mainCam = FindObjectOfType<Camera>().gameObject;
        arPlaneManager.planesChanged+=OnPlaneChanged;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLabel();
        UpdatePlaneColor();
        UpdateActive();
    }

    void UpdateLabel(){
        //_TextMesh.text = _ARPlane.classification.ToString();
        _TextMesh.text = (_ARPlane.extents.x * _ARPlane.extents.y).ToString();
        _TextObj.transform.position = _ARPlane.center;
        _TextObj.transform.LookAt(_mainCam.transform);
        _TextObj.transform.Rotate(new Vector3(0,180,0));
    }

    void UpdateActive(){
        if(_ARPlane.extents.x * _ARPlane.extents.y < 0.1f)
            {
                _ARPlane.gameObject.SetActive(false);
            }
        else{
            _ARPlane.gameObject.SetActive(true);
        }
    }
    void UpdatePlaneColor()
    {
        Color planeMatColor = Color.cyan;
        switch(_ARPlane.classification)
        {
            case PlaneClassification.None:
                planeMatColor = Color.cyan;
                break;
            case PlaneClassification.Wall:
                planeMatColor = Color.white;
                break;
            case PlaneClassification.Floor:
                planeMatColor = Color.green;
                break;
            case PlaneClassification.Ceiling:
                planeMatColor = Color.blue;
                break;
            case PlaneClassification.Table:
                planeMatColor = Color.yellow;
                break;
            case PlaneClassification.Seat:
                planeMatColor = Color.magenta;
                break;
            case PlaneClassification.Door:
                planeMatColor = Color.red;
                break;
            case PlaneClassification.Window:
                planeMatColor = Color.clear;
                break;
        }
        planeMatColor.a=0.33f;
        _PlaneMeshRenderer.material.color = planeMatColor;
    }

    void OnPlaneChanged(ARPlanesChangedEventArgs args)
    {
        if(args.updated != null && args.updated.Count>0){

        foreach (ARPlane plane in args.updated.Where(plane => plane.extents.x * plane.extents.y >= 0.1f))
        {
            plane.gameObject.SetActive(true);
        }
        }
    }

}