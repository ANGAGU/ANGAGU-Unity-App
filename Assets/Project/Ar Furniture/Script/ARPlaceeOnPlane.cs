using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Slider = UnityEngine.UI.Slider;

public class ARPlaceeOnPlane : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public GameObject placeObject;
    public Text tx;
    public GameObject spawnObject;
    public Slider rotateSlider;
    
    private bool buttonClick = true;
    private Rigidbody myRigid;
    private Vector3 rotation;
    private Vector3 position;
    private float sliderValue ;
    private int mode = 1; // 1->이동, 2->회전, 3->배치
    
    public ARPlaneManager arPlaneManager;

    // Update is called once per frame
    private void Start()
    {
        sliderValue = 0;
        arPlaneManager.planesChanged += OnPlaneChanged;
        rotation = new Vector3(0, 1, 0);
    }

    void Update()
    {
        tx.text = mode.ToString();
        if (mode == 1) // 이동
        {
            UpdateCenterObject();
        }
        else if (mode == 2) // 회전
        {
            //PlaceObjectRotate();
        }
        else if (mode == 3) // 배치
        {
            position = placeObject.transform.position - new Vector3(0, 0.4f, 0);
            Vector3 currentRotation = rotation + new Vector3(0, 1, 0) * sliderValue;
            placeObject.transform.SetPositionAndRotation(position, Quaternion.Euler(currentRotation));
            mode = 4;
        }
        else if (mode == 4)
        {

        }
    }

    private void PlaceObjectRotate()
    {
        placeObject.transform.Rotate(rotation + new Vector3(0,1,0) * sliderValue);
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

    private void PlaceObjectByTouch() {
        /*if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                Vector3 upPosition = hitPose.position + new Vector3(0, 0.4f, 0);
                if (!spawnObject)
                {
                    spawnObject = Instantiate(placeObject, upPosition, hitPose.rotation);
                    //Vector3 upPosition = hitPose.position;
                    tx.text = upPosition.ToString();
                }
                else
                {
                    spawnObject.transform.SetPositionAndRotation(upPosition, hitPose.rotation);
                    //Vector3 upPosition = hitPose.position;
                    tx.text = upPosition.ToString();
                }
            }
        }*/

        /*if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!spawnObject)
            {
                Debug.Log(touch.position);
                spawnObject = Instantiate(placeObject, touch.position, Quaternion.Euler(new Vector3(0,0,0)));
            }
            else 
            {
                spawnObject.transform.SetPositionAndRotation(touch.position, Quaternion.Euler(new Vector3(0,0,0)));
            }
            
            spawnObject.SetActive(true);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            if (!spawnObject)
            {
                Debug.Log(mousePos);
                spawnObject = Instantiate(placeObject, Vector3.zero, Quaternion.Euler(new Vector3(0,0,0)));
            }
            else 
            {
                spawnObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(new Vector3(0,0,0)));
            }
            
            spawnObject.SetActive(true);
        }*/
    }

    private void UpdateCenterObject()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycaster.Raycast(screenCenter, hits,TrackableType.PlaneWithinPolygon);

        if (hits.Count > 0) // 인식되는 평면이 있는 경우
        {
            Pose placementPose = hits[0].pose;
            position = placementPose.position + new Vector3(0, 0.4f, 0);
            placeObject.SetActive(true);
            Vector3 currentRotation = rotation + new Vector3(0, 1, 0) * sliderValue;
            placeObject.transform.SetPositionAndRotation(position, Quaternion.Euler(currentRotation));
        }
        else // 인식되는 평면이 없는 경우
        {
            placeObject.SetActive(false);
        }
    }
    
    // Start is called before the first frame updat
    public void buttonOnClick()
    {
        mode = 3; // 배치
        myRigid = placeObject.GetComponent<Rigidbody>();
        myRigid.useGravity = false;
    }
    public void buttonOnInit()
    {
        mode = 1; // 이동
    }
    public void sliderOnChange()
    {
        mode = 2; // 회전
        sliderValue = rotateSlider.value;
        rotation = rotation + new Vector3(0,1,0) * sliderValue;
    }
    
}
