using UnityEngine;
using UnityEngine.EventSystems;
using MicrogameCourse.Framework;

public class InputButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public enum ButtonType
    {
        Up,
        Down,
        Left,
        Right,
        Action,
        Select
    }

    [SerializeField] private PlayerController playerController;
    [SerializeField] private ButtonType buttonType;


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"{buttonType} pressed");
        switch(buttonType)
        {
            case ButtonType.Up:
                playerController.HandleDirectionalButtonClick(Vector2.up);
                break;
            case ButtonType.Down:
                playerController.HandleDirectionalButtonClick(Vector2.down);
                break;
            case ButtonType.Left:
                playerController.HandleDirectionalButtonClick(Vector2.left);
                break;
            case ButtonType.Right:
                playerController.HandleDirectionalButtonClick(Vector2.right);
                break;
            case ButtonType.Action:
                playerController.HandleActionButtonClick();
                break;
            case ButtonType.Select:
                playerController.HandleSelectButtonClick();
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch(buttonType)
        {
            case ButtonType.Up:
            case ButtonType.Down:
            case ButtonType.Left:
            case ButtonType.Right:
                playerController.StopMovement();
                break;
        }
    }

}
