using UnityEngine;
using MicrogameCourse.Framework;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Unity.Android.Gradle.Manifest;

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

        private void Awake()
        {   
            session = FindFirstObjectByType<MicrogameSession>();
            mainCamera = Camera.main;
            originalPosition = transform.position;

        }

        private void Update()
        {
            if(session == null || !session.isTimerStarted) return;

            if(Pointer.current == null) return;

            HandleMovement();
            HandleClick();
            HandleDoubleClick();
            HandleDrag();
            HandleDragAndRelease();
            HandleTwoFingerTap();
            HandleTwoFingerDrag();
        }

        private void HandleMovement()
        {

            
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

            Debug.Log("Click");

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
        private bool Supports(InputMode inputMode)
        {
            return gameConfigData != null && gameConfigData.inputProfile != null && gameConfigData.inputProfile.supportedInputs.HasFlag(inputMode);
        }

    }

}
