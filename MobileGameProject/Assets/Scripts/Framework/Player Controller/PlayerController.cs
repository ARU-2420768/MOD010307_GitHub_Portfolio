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
        [SerializeField] private float swipeThreshold = 100f;



        private MicrogameSession session;
        private Camera mainCamera;
        private bool isDraggingPointer;
        private float currentSpeed;

        private Vector3 originalPosition;
        private Vector3 currentDirection;
        private Vector3 currentWindDirection;
        private Vector2 dragStartPosition;
        private bool twoFingerDragTriggered;

        private bool hasMovedPastThreshold;

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


            #if UNITY_EDITOR
                HandleEditorInput();
            #endif

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

    private void HandleEditorInput()
    {
        if (Keyboard.current == null)
            return;

        // Tap
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Debug.Log($"1 Key Pressed This Frame");
            Debug.Log($"Game Reference: {game}");
            game?.OnPointerPressed();
        }

        // Swipe
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Debug.Log($"2 Key Pressed This Frame");
            game?.OnDragRelease(Vector2.zero);
        }

        // Two Finger Tap
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Debug.Log($"3 Key Pressed This Frame");
            game?.OnTwoFingerTap();
        }

        // Two Finger Swipe
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            Debug.Log($"4 Key Pressed This Frame");
            game?.OnTwoFingerDrag(Vector2.zero);
        }
    }


        private void HandlePointerMovement()
        {
            if(!Supports(InputMode.PointerMovement)) return;
            
            MovementProfileData movement = gameConfigData.movementProfile;

            if (Pointer.current == null)
                return;

            Vector2 screenPosition = Pointer.current.position.ReadValue();

            if (!float.IsFinite(screenPosition.x) ||
                !float.IsFinite(screenPosition.y))
            {
                Debug.LogWarning($"Invalid pointer position: {screenPosition}");
                return;
            }
            
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x,screenPosition.y, Mathf.Abs(mainCamera.transform.position.z)));

            Vector3 position = transform.position;

            position.x = Mathf.Lerp(position.x, worldPosition.x, movement.acceleration * Time.deltaTime);

            position.y = Mathf.Lerp(position.y, worldPosition.y, movement.acceleration * Time.deltaTime);

            position.x = Mathf.Clamp(position.x, originalPosition.x + movement.minX, originalPosition.x + movement.maxX);

            position.y = Mathf.Clamp(position.y, originalPosition.y + movement.minY, originalPosition.y + movement.maxY);

            transform.position = position;
        }

private bool trackingTapCandidate;
private bool tapCandidateExceededThreshold;

        private void HandleClick()
        {
            if (!Supports(InputMode.Click)) return;

            if (Supports(InputMode.DragRelease))
            {
                if (Pointer.current.press.wasPressedThisFrame)
                {
                    trackingTapCandidate = true;
                    tapCandidateExceededThreshold = false;
                }
                else if (trackingTapCandidate && Pointer.current.press.isPressed)
                {
                    if (Vector2.Distance(dragStartPosition, Pointer.current.position.ReadValue()) >= swipeThreshold)
                    {
                        tapCandidateExceededThreshold = true;
                    }
                }
                else if (trackingTapCandidate && Pointer.current.press.wasReleasedThisFrame)
                {
                    if (!tapCandidateExceededThreshold)
                    {
                        game?.OnPointerPressed();
                    }
                    trackingTapCandidate = false;
                }
                return;
            }

            if (Pointer.current.press.wasPressedThisFrame)
            {
                game?.OnPointerPressed();
            }
        }


        private void HandleClick1()
        {

            if(!Supports(InputMode.Click)) return;

            if (Pointer.current.press.wasPressedThisFrame)
            {
                Debug.Log($"[MobileInput][PC] PressDown pos={Pointer.current.position.ReadValue()} | hasDrag={Supports(InputMode.Drag)} | hasDragRelease={Supports(InputMode.DragRelease)}");
                game?.OnPointerPressed();
            }
        }


        private void HandleDoubleClick()
        {
            if(!Supports(InputMode.DoubleClick)) return;
        }

        private void HandleDrag()
        {
            if (!Supports(InputMode.Drag))
                return;

            if (Pointer.current.press.wasPressedThisFrame)
            {
                dragStartPosition = Pointer.current.position.ReadValue();
                isDraggingPointer = true;
                hasMovedPastThreshold = false;
                Debug.Log($"[MobileInput][PC] DragStart pos={dragStartPosition}");
            }

            if (isDraggingPointer && Pointer.current.press.isPressed)
                {
                    Vector2 currentPos = Pointer.current.position.ReadValue();
                    if (Vector2.Distance(dragStartPosition, currentPos) >= swipeThreshold)
                    {
                        hasMovedPastThreshold = true;
                    }
                    if (hasMovedPastThreshold)
                    {
                        game?.OnDrag(currentPos);
                    }
                    float dist = Vector2.Distance(dragStartPosition, currentPos);
                    Debug.Log($"[MobileInput][PC] DragHold cur={currentPos} | dist={dist:F1}/{swipeThreshold}");
                    //game?.OnDrag(curPos);
                }

            if (Pointer.current.press.wasReleasedThisFrame)
            {
                isDraggingPointer = false;
                hasMovedPastThreshold = false;
                Debug.Log($"[MobileInput][PC] PressUp pos={Pointer.current.position.ReadValue()}");
            }
        }

        private void HandleDrag1()
        {
            if (!Supports(InputMode.Drag))
                return;

            if (Pointer.current.press.wasPressedThisFrame)
            {
                dragStartPosition = Pointer.current.position.ReadValue();
            }

            game?.OnDrag(Pointer.current.position.ReadValue());

        }

        private void HandleDragAndRelease()
        {
            if (!Supports(InputMode.DragRelease))
                return;

            if (Pointer.current.press.wasReleasedThisFrame)
            {
                Vector2 releasePosition = Pointer.current.position.ReadValue();

                float distance = Vector2.Distance(dragStartPosition, releasePosition);

                Debug.Log($"[MobileInput][PC] DragRelease start={dragStartPosition} end={releasePosition} dist={distance:F1}/{swipeThreshold} fired={distance >= swipeThreshold}");

                if (distance >= swipeThreshold)
                {
                    Debug.Log("Swipe Detected");
                    game?.OnDragRelease(releasePosition);
//                    game?.OnDrag(releasePosition);
                }
            }
        }


        private void HandleTwoFingerTap()
        {
            if(!Supports(InputMode.TwoFingerTap)) return;

            if (Touchscreen.current == null)
                return;

            var touch1 = Touchscreen.current.touches[0];
            var touch2 = Touchscreen.current.touches[1];

            bool touch1Pressed = touch1.press.wasPressedThisFrame;

            bool touch2Pressed = touch2.press.wasPressedThisFrame;

            if (touch1Pressed && touch2Pressed)
            {
                Debug.Log("Two Finger Tap");

                game?.OnTwoFingerTap();
            }
        }

        private void HandleTwoFingerDrag()
        {
            if (!Supports(InputMode.TwoFingerDrag))
                return;

            if (Touchscreen.current == null)
                return;

            var touch1 = Touchscreen.current.touches[0];
            var touch2 = Touchscreen.current.touches[1];

            if (!touch1.press.isPressed ||
                !touch2.press.isPressed)
            {
                twoFingerDragTriggered = false;
                return;
            }

            Vector2 delta =
                touch1.delta.ReadValue() +
                touch2.delta.ReadValue();

            if (delta.magnitude > 20f &&
                !twoFingerDragTriggered)
            {
                twoFingerDragTriggered = true;

                game?.OnTwoFingerDrag(delta);
            }
        }


        public void StopMovement()
        {
            currentDirection = Vector2.zero;
            game?.OnStopMovement();
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
                    game?.OnDirectionalInput(direction, button);
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
