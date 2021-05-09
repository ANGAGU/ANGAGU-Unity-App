using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceeOnPlane : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public GameObject placeObject;

    public GameObject spawnObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // UpdateCenterObject();
        PlaceObjectByTouch();
    }
    
    private void PlaceObjectByTouch() {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                if (!spawnObject)
                {
                    spawnObject = Instantiate(placeObject, hitPose.position, hitPose.rotation);
                }
                else
                {
                    spawnObject.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                }
            }
        }
    }

    private void UpdateCenterObject()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycaster.Raycast(screenCenter, hits,TrackableType.Planes);

        if (hits.Count > 0) // 인식되는 평면이 있는 경우
        {
            Pose placementPose = hits[0].pose;
            placeObject.SetActive(true);
            placeObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else // 인식되는 평면이 없는 경우
        {
            placeObject.SetActive(false);
        }

    }
}
