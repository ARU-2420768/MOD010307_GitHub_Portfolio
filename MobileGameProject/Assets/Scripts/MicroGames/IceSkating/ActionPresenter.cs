using UnityEngine;

public class ActionPresenter : MonoBehaviour
{

    [SerializeField] private Transform skillMovesPlayArea;
    [SerializeField] private GameObject tapPrefab;
    [SerializeField] private GameObject swipePrefab;
    [SerializeField] private GameObject twoFingerTapPrefab;
    [SerializeField] private GameObject twoFingerDragPrefab;

    private GameObject currentObject;

    public void ShowAction(SkillAction action)
    {
        if(currentObject != null)
            Destroy(currentObject);

        switch (action)
        {
            case SkillAction.Tap:
                currentObject = Instantiate(tapPrefab, skillMovesPlayArea);
                break;
            case SkillAction.Swipe:
                currentObject = Instantiate(swipePrefab, skillMovesPlayArea);
                break;
            case SkillAction.TwoFingerTap:
                currentObject = Instantiate(twoFingerTapPrefab, skillMovesPlayArea);
                break;
            case SkillAction.TwoFingerDrag:
                currentObject = Instantiate(twoFingerDragPrefab, skillMovesPlayArea);
                break;


        }
    }

}
