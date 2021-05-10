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

public class ARPlaceOnPlane : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public GameObject placeObject;
    public Text tx;
    public Slider rotateSlider;
    public GameObject checkObject;
    public Bounds bounds;
    public GameObject originModel;
    public Text deltaDiff;
    public Text pinch;
    public Text pos;
    
    private GameObject spawnObject;
    private bool buttonClick = true;
    private Rigidbody myRigid;
    private Vector3 rotation;
    private Vector3 position;
    private float sliderValue ;
    private int mode = 1; // 1->이동, 2->회전, 3->배치
    

    private float rotationRate = 0.15f;
    private float rotateY;
    
    public ARPlaneManager arPlaneManager;
    
    private void Start()
    {
        sliderValue = 0;
        arPlaneManager.planesChanged += OnPlaneChanged;
        rotation = new Vector3(0, 0, 0);
    }

    void Update() //
    {
        tx.text = mode.ToString();
        if (mode == 1) // 이동
        {
            UpdateCenterObject();
            //resizeObjectByTouch(); // 손가락 두개로 사이징
        }
        else if (mode == 2) // 회전
        {
            PlaceObjectRotate();
            mode = 4;
        }
        else if (mode == 3) // 배치
        {
            Vector3 newPosition = placeObject.transform.position - new Vector3(0, 0.4f, 0);
            Vector3 currentRotation = rotation + new Vector3(0, 0.03f, 0) * sliderValue;
            placeObject.transform.SetPositionAndRotation(newPosition, Quaternion.Euler(currentRotation));
            mode = 4;
        }
        else if (mode == 4) // 사이징 모드
        {
            placeObjectByTouch();
            resizeObjectByTouch(); // 손가락 2개로 사이징
            // 리사이징
            // 회전
        }
    }
    private void placeObjectByTouch()
    {
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            if(arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                placeObject.SetActive(true);
                placeObject.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            }
        }
    }
    private void rotateObject()
    {
        if (Input.touchCount >= 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (touchZero.phase == TouchPhase.Moved && touchOne.phase == TouchPhase.Moved)
            {
                rotateY = (touchOne.deltaPosition.x + touchZero.deltaPosition.x) / 2;

                placeObject.transform.Rotate(0,
                    -rotateY * rotationRate, 0, Space.World);
            }
        }
    }
    private void resizeObjectByTouch()
    {
        if (Input.touchCount == 2)
        {
            float originScale = GameObject.FindWithTag("OriginModel").transform.localScale.x;
            // 실제 저장해놨던 scale 가졍괴
            
            // Get Touch points.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Get prev object scale.
            Vector3 prevScale = placeObject.transform.localScale;

            // Calculate pinch amount with max, min.
            float pinchAmount = Mathf.Clamp(prevScale.x + deltaMagnitudeDiff * Time.deltaTime*(-0.1f), originScale/2, originScale);
            
            //deltaDiff.text = deltaMagnitudeDiff.ToString();
            //pinch.text = pinchAmount.ToString();
            //pos.text = placeObject.transform.localScale.x.ToString();

           

            // Set new scale. 
            Vector3 newScale = new Vector3(pinchAmount, pinchAmount, pinchAmount);
            placeObject.transform.localScale = Vector3.Lerp(prevScale, newScale, Time.deltaTime);

            //deltaDiff.text = (51.8f * placeObject.transform.localScale.x * 100).ToString();
            //pinch.text = (77.3f * placeObject.transform.localScale.y * 100).ToString();
            //pos.text = (53.0f * placeObject.transform.localScale.z * 100).ToString();
        }
    }
    
    private void PlaceObjectRotate()
    {
        //placeObject.transform.localEulerAngles; 
        //placeObject.transform.Rotate(rotation + new Vector3(0,0.03f,0) * sliderValue);
        ////
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
            checkObject.SetActive(true);
            Vector3 currentRotation = rotation + new Vector3(0, 1, 0) * sliderValue;
            placeObject.transform.SetPositionAndRotation(position, Quaternion.Euler(currentRotation));
            checkObject.transform.SetPositionAndRotation(placementPose.position, Quaternion.Euler(new Vector3(0,0,0)));
        }
        else // 인식되는 평면이 없는 경우
        {
            checkObject.SetActive(false);
            placeObject.SetActive(false);
        }
    }
    
    
    public void buttonOnClickTo1()
    {
        mode = 1; // 이동
    }
    public void buttonOnClickTo2()
    {
        mode = 2; // 회전
        sliderValue = rotateSlider.value;
        //rotation = rotation + new Vector3(0,1,0) * sliderValue;
    }
    public void buttonOnClickTo3()
    {
        mode = 3; // 배치
        checkObject.SetActive(false);
        // myRigid = placeObject.GetComponent<Rigidbody>();
        // myRigid.useGravity = false;
    }
}
