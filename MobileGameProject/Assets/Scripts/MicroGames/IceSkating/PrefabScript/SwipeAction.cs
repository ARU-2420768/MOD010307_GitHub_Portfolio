using UnityEngine;

public class SwipeAction : MonoBehaviour
{
    [SerializeField] private RectTransform startMarker;
    [SerializeField] private RectTransform endMarker;

    public RectTransform StartMarker => startMarker;
    public RectTransform EndMarker => endMarker;
}