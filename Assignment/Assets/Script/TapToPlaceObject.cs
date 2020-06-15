using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine;

public class TapToPlaceObject : MonoBehaviour
{
    public GameObject placableObject;
       
   private ARRaycastManager _raycastManager;
   private Pose _placementPose;
   private bool _placementPoseIsValid = false;
   private Touch _touch;
   void Start() 
   {
       _raycastManager = FindObjectOfType<ARRaycastManager>();
   }
   
   void Update()
   {
       if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetKeyDown(KeyCode.A))
       {
           _touch = Input.GetTouch(0);
           PlaceObject();
       }
   }

   private void PlaceObject()
   {
       var screenPosition = Camera.main.ScreenToWorldPoint(_touch.position);
       var hits = new List<ARRaycastHit>();
       _raycastManager.Raycast(_touch.position, hits,TrackableType.Planes);
       _placementPoseIsValid = hits.Count > 0;
       if (_placementPoseIsValid)
       {
           _placementPose = hits[0].pose;
   
           var cameraForward = Camera.main.transform.forward;
           var cameraBearing = new Vector3(cameraForward.x,0, cameraForward.z).normalized;
           _placementPose.rotation = Quaternion.LookRotation(cameraBearing);

           Instantiate(placableObject, _placementPose.position, _placementPose.rotation);
       }
   }
}
