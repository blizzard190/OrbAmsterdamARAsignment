using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class WorkingTapToPlace : MonoBehaviour
{
    //Made with https://youtu.be/xguiSueY1Lw
    
    public GameObject placableObject;
       
    private ARRaycastManager _raycastManager;
    private Pose _placementPose;
    private bool _placementPoseIsValid = false;
  
    static List<ARRaycastHit> _hits = new List<ARRaycastHit>();
        
    void Start() 
    {
        _raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    private void Update()
    {
        if(!GetTouchPosition(out Vector2 touchPos))
            return;

        if (_raycastManager.Raycast(touchPos, _hits, TrackableType.Planes))
        {
            var hitPose = _hits[0].pose;
            
            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x,0, cameraForward.z).normalized;
            _placementPose.rotation = Quaternion.LookRotation(cameraBearing);

            Instantiate(placableObject, hitPose.position, _placementPose.rotation);
        }
    }

    bool GetTouchPosition(out Vector2 touchPos)
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPos = Input.GetTouch(0).position;
            return true;
        }

        touchPos = default;
        return false;
    }
}
