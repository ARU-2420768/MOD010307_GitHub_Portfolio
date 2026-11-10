using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ZoomCameraManager : MonoBehaviour
{
    [SerializeField] private Transform scopeAnchor;
    [SerializeField] private float zoomFOV = 20f;
    [SerializeField] private float shakeStrength = 0.001f;

    private Coroutine shakeCoroutine;

    private Camera cam;

    private float shakeTimeLeft;

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

        shakeTimeLeft -= Time.deltaTime;

        if(shakeTimeLeft > 0f)
        {
            Vector2 wobble = Random.insideUnitCircle * 0.1f;

            transform.position = scopeAnchor.position + (Vector3)wobble;

            //transform.rotation = scopeAnchor.rotation * Quaternion.Euler(wobble.y, wobble.x, 0);
        }

        // Zoom
        cam.fieldOfView = zoomFOV;
    }

    public void Shake(float duration, float multiplier)
    {
        shakeTimeLeft = duration * multiplier;

        if(shakeCoroutine == null)
            shakeCoroutine = StartCoroutine(ShakeAction(shakeTimeLeft));
    }

    private IEnumerator ShakeAction(float duration)
    {
        float durationLeft = duration;

        while (durationLeft > 0)
        {
            durationLeft -= Time.deltaTime;
            Vector2 wobble = Random.insideUnitCircle * 0.1f;
            transform.position = scopeAnchor.position + (Vector3)wobble;

            yield return null;
        }

        transform.position = scopeAnchor.position;
        shakeCoroutine = null;
    }

}