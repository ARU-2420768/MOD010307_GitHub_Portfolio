using UnityEngine;
using MicrogameCourse.Framework;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace MicrogameCourse.Framework
{

    public class PlayerController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private GameConfigurationData gameConfigData;

        [Header("Game Reference")]
        [SerializeField] private MicrogameBehaviour game;

        private MicrogameSession session;
        private Camera mainCamera;

        private float currentSpeed;

        private Vector3 originalPosition;
        private Vector3 currentDirection;
        private Vector3 currentWindDirection;
        

        private void Awake()
        {   
            session = FindFirstObjectByType<MicrogameSession>();
            mainCamera = Camera.main;
            originalPosition = transform.position;
            currentDirection = Vector3.zero;
            currentWindDirection = Vector3.zero;

        }

        private void Update()
        {
            if(session == null || !session.isTimerStarted) return;

            // this method deals with button clicks so doesn't need the pointer values
            HandleMovement();


            if(Pointer.current == null) return;

            HandleClick();
            HandleDoubleClick();
            HandleDrag();
            HandleDragAndRelease();
            HandleTwoFingerTap();
            HandleTwoFingerDrag();
            HandlePointerMovement();
        }

        private void HandlePointerMovement()
        {
            if(!Supports(InputMode.PointerMovement)) return;
            
            MovementProfileData movement = gameConfigData.movementProfile;

            Vector2 screenPosition = Pointer.current.position.ReadValue();

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x,screenPosition.y, Mathf.Abs(mainCamera.transform.position.z)));

            Vector3 position = transform.position;

            position.x = Mathf.Lerp(position.x, worldPosition.x, movement.acceleration * Time.deltaTime);

            position.y = Mathf.Lerp(position.y, worldPosition.y, movement.acceleration * Time.deltaTime);

            position.x = Mathf.Clamp(position.x, originalPosition.x + movement.minX, originalPosition.x + movement.maxX);

            position.y = Mathf.Clamp(position.y, originalPosition.y + movement.minY, originalPosition.y + movement.maxY);

            transform.position = position;
        }




        private void HandleClick()
        {
            if(!Supports(InputMode.Click)) return;

            if (Pointer.current.press.wasPressedThisFrame)
            {
                game?.OnPointerPressed();
            }
        }


        private void HandleDoubleClick()
        {
            if(!Supports(InputMode.DoubleClick)) return;
        }

        private void HandleDrag()
        {
            if(!Supports(InputMode.Drag)) return;
        }

        private void HandleDragAndRelease()
        {
            if(!Supports(InputMode.DragRelease)) return;
        }

        private void HandleTwoFingerTap()
        {
            if(!Supports(InputMode.TwoFingerTap)) return;
        }

        private void HandleTwoFingerDrag()
        {
            if(!Supports(InputMode.TwoFingerDrag)) return;
        }

        public void StopMovement()
        {
            currentDirection = Vector2.zero;
            currentWindDirection = Vector2.zero;
        }

        private void HandleMovement()
        {
            if (currentDirection == Vector3.zero)
            {
                currentSpeed = 0;
                return;
            }

            MovementProfileData movement = gameConfigData.movementProfile;


            // Accelerate up to max speed
            currentSpeed += movement.acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, movement.maxSpeed);

            Vector3 position = transform.position;

            position += (Vector3)currentDirection * currentSpeed * Time.deltaTime;

            position.x = Mathf.Clamp(
                position.x,
                originalPosition.x + movement.minX,
                originalPosition.x + movement.maxX);

            position.y = Mathf.Clamp(
                position.y,
                originalPosition.y + movement.minY,
                originalPosition.y + movement.maxY);

            transform.position = position;
        }

        public void HandleDirectionalButtonClick(Vector2 direction, GameObject button)
        {
            if(!Supports(InputMode.DirectionalButtons)) return;
            switch(button.tag)
            {
                case "TargetDirection":
                    currentDirection = direction;
                    break;
                case "Turret":
                    currentWindDirection = direction;
                    //game?.OnDirectionalInput(direction, button);
                    FindFirstObjectByType<WindManager>().MoveWindIndicator(direction);
                    break;
            }
            //currentDirection = direction;
            //game?.OnDirectionalInput(direction, button);
        }
        
        public void HandleActionButtonClick(GameObject button)
        {
            if(!Supports(InputMode.ButtonTap)) return;
            game?.OnButtonTap(button);
        }

        public void HandleSelectButtonClick()
        {
            if(!Supports(InputMode.ButtonTap)) return;
        }


        private bool Supports(InputMode inputMode)
        {
            return gameConfigData != null && gameConfigData.inputProfile != null && gameConfigData.inputProfile.supportedInputs.HasFlag(inputMode);
        }

    }

}
