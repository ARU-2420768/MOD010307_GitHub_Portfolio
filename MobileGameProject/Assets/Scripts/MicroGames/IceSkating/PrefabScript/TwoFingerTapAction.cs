using UnityEngine;

public class TwoFingerTapAction : MonoBehaviour
{

    [SerializeField] private RectTransform boxClick1;
    [SerializeField] private RectTransform boxClick2;

    public RectTransform BoxClick1 => boxClick1;
    public RectTransform BoxClick2 => boxClick2;

}
