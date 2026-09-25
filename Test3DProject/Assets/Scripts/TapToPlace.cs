using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
 
// Part 1: tap on a detected surface to place the model there.
public class TapToPlace : MonoBehaviour
{
    [Tooltip("The AR Raycast Manager on the XR Origin.")]
    public ARRaycastManager raycastManager;
 
    [Tooltip("The model to place. It is hidden until you tap.")]
    public Transform model;
 
    // The raycast writes its results into this list.
    readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();
 
    void Start()
    {
        model.gameObject.SetActive(false);
    }
 
    void Update()
    {
        // Pointer means a touchscreen on the phone, or the mouse in the Editor.
        if (Pointer.current == null || !Pointer.current.press.wasPressedThisFrame)
            return;
 
        Vector2 screenPosition = Pointer.current.position.ReadValue();
 
        // Fire a ray from the screen into the real world. Did it hit a detected surface?
        if (raycastManager.Raycast(screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose; // hits[0] is the closest surface
            model.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            model.gameObject.SetActive(true);
        }
    }
}
