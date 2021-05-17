using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    public GameObject checkObject;
    public GameObject modelHeight;
    public GameObject modelWidth;
    public GameObject modelDepth;
    public GameObject humanGirl;
    public GameObject humanBoy;
    public GameObject heightText;
    public GameObject directionLight;
    public GameObject lightPanel;
    
    private GameObject spawnObject;
    private bool buttonClick = true;
    private Rigidbody myRigid;
    private Vector3 rotation;
    private Vector3 position;
    
    private float sliderValue ;
    private int mode = 1; // 1->이동, 2->회전, 3->배치
    private bool getRealSize = true;
    
    private float scaleRate = -0.15f;
    private float rotationRate = 0.15f;
    private float rotateY;
    private float originScale;
    private bool modelOk = true;
    private bool humanVis = false;
    public ARPlaneManager arPlaneManager;
    private int touchThreshold = 120;
    private void Start()
    {
        sliderValue = 0;
        arPlaneManager.planesChanged += OnPlaneChanged;
        rotation = new Vector3(0, 0, 0);
        modelHeight.SetActive(false);
        modelWidth.SetActive(false);
        modelDepth.SetActive(false);
        humanBoy.SetActive(false);
        humanGirl.SetActive(false);
        heightText.SetActive(false);
        lightPanel.SetActive(false);
    }
    void Update()
    {
        // tx.text = placeObject.transform.localPosition.ToString();
        if (mode == 1) // 이동 및 초기화
        {
            UpdateCenterObject();
        }
        else if (mode == 2) // 회전
        {
            rotateObject();
        }
        else if (mode == 3) // 배치
        {
            mode = 4;
            placeObject.transform.position = checkObject.transform.position;
            tx.text = checkObject.transform.position.ToString();
            checkObject.SetActive(true);
        }
        else if (mode == 4) // 가구 이동
        {
            placeObjectByTouch();
        }
        else if (mode == 5) // 리사이징 모드
        {
            resizeObjectByTouch();
        }
    }
    private void placeObjectByTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (touch.position.y < 220) return;
            if(arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                placeObject.SetActive(true);
                placeObject.transform.SetPositionAndRotation(hitPose.position, placeObject.transform.rotation);
            }
        }
    }
    private void rotateObject()
    {
        if (Input.touchCount >= 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            if (touchZero.position.y < 220 || touchOne.position.y < 220) return;
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
        if (Input.touchCount >= 2)
        {
            if (getRealSize)
            {
                originScale = GameObject.FindWithTag("OriginModel").transform.localScale.x;
                getRealSize = false;
            }
            // 실제 저장해놨던 scale 가져오기
            
            // Get Touch points.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            if (touchZero.position.y < 120 || touchOne.position.y < 120) return;
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
            float pinchAmount = Mathf.Clamp(prevScale.x + deltaMagnitudeDiff * Time.deltaTime*(-originScale), originScale/2, originScale);

            // Set new scale. 
            Vector3 newScale = new Vector3(pinchAmount, pinchAmount, pinchAmount);
            placeObject.transform.localScale = Vector3.Lerp(prevScale, newScale, Time.deltaTime);
            modelDepth.GetComponent<TextMeshPro>().text = (51.8f * placeObject.transform.localScale.x * 100).ToString();
            modelHeight.GetComponent<TextMeshPro>().text = (77.3f * placeObject.transform.localScale.y * 100).ToString();
            modelWidth.GetComponent<TextMeshPro>().text = (53.0f * placeObject.transform.localScale.z * 100).ToString();
        }
    }
    void OnPlaneChanged(ARPlanesChangedEventArgs args)
    {
        if(args.updated != null && args.updated.Count > 0)
        {
            foreach (ARPlane plane in args.updated.Where(plane => plane.extents.x * plane.extents.y >= 0.25f))
            {
                plane.gameObject.SetActive(true);
            }
        }
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
            
            if (modelOk)
            {
                humanGirl.SetActive(true);
                heightText.SetActive(true);
                heightText.GetComponent<TextMeshPro>().text = "180cm";
                heightText.transform.SetPositionAndRotation(placementPose.position + new Vector3(0,2,0), placementPose.rotation);
                humanGirl.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
                humanGirl.transform.Rotate(0,
                    -180, 0, Space.World);
                modelOk = false;
                humanVis = true;
            }
            placeObject.SetActive(true);
            checkObject.SetActive(true);
            placeObject.transform.position = position;
            
            checkObject.transform.localScale = new Vector3(placeObject.transform.localScale.x, 0, placeObject.transform.localScale.z);
            checkObject.transform.SetPositionAndRotation(placementPose.position, Quaternion.Euler(new Vector3(0,0,0)));
        }
        else // 인식되는 평면이 없는 경우
        {
            checkObject.SetActive(false);
            placeObject.SetActive(false);
        }
    }
    public void buttonToPosition() // 이동
    {
        mode = 1;
    }
    public void buttonToRotate() // 회전
    {
        mode = 2;
    }
    public void buttonToBatch() // 배치
    {
        mode = (mode == 4 || mode == 2) ? 4 : 3;
        // 회전 또는 터치일 때 배치 누르면 mode 4로
    }
    public void buttonToTouch() // 
    {
        mode = 4;
    }
    public void buttonToResize()
    {
        mode = 5;
    }

    public void toggleHuman()
    {
        if (humanVis)
        {
            humanGirl.SetActive(false);
            humanVis = false;
        }
        else
        {
            humanGirl.SetActive(true);
            humanVis = true;
            modelOk = true;
        }
    }

    public void selectLight()
    {
        lightPanel.SetActive(true);
    }

    public void setLightRed()
    {
        Light lt = directionLight.GetComponent<Light>();
        lt.color = new Color(255/255f,160/255f,160/255f,140/255);
        // lt.color = Color.red;
        lightPanel.SetActive(false);
    }
    public void setLightYellow()
    {
        Light lt = directionLight.GetComponent<Light>();
        // lt.color = Color.yellow;
        lt.color = new Color(1, 0.92f, 0.016f, 0.5f);
        lightPanel.SetActive(false);
    }
    public void setLightBlue()
    {
        Light lt = directionLight.GetComponent<Light>();
        lt.color = new Color((167/255f),251/255f,255/255f,150/255);
        // lt.color = Color.blue;
        lightPanel.SetActive(false);        
        touchThreshold = 120;
    }
    public void setLightGreen()
    {
        Light lt = directionLight.GetComponent<Light>();
        lt.color = new Color(174/255f,255/255f,160/255f,123/255);
        // lt.color = Color.green;
        lightPanel.SetActive(false);
        touchThreshold = 120;
    }
    public void setLightPupple()
    {
        Light lt = directionLight.GetComponent<Light>();
        lt.color = new Color(255/255f,255/255f,255/255f,123/255);
        // lt.color = Color.white;
        lightPanel.SetActive(false);
        touchThreshold = 120;
    }
    
}
