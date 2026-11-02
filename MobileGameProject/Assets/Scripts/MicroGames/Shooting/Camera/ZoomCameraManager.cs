using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ZoomCameraManager : MonoBehaviour
{
    [SerializeField] private Transform scopeAnchor;
    [SerializeField] private float zoomFOV = 20f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (scopeAnchor == null)
            return;

        // Match weapon/scope position
        transform.position = scopeAnchor.position;

        // Match weapon/scope rotation
        transform.rotation = scopeAnchor.rotation;

        // Zoom
        cam.fieldOfView = zoomFOV;
    }
}