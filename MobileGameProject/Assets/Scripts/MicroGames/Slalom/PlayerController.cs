using UnityEngine;
using MicrogameCourse.Framework;
using MicrogameCourse.Microgames;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;

namespace MicrogameCourse.Framework
{

    public sealed class PlayerController : MicrogameBehaviour
    {

        [Header("Player Controls")]
        [SerializeField, Min(0.1f)] private float maxSpeed = 5f;
        [SerializeField, Min(0f)] private float acceleration = 7f;
        [SerializeField] private float minX = -2.3f;
        [SerializeField] private float maxX = 2.3f;

        private MicrogameSession session;
        private float currentSpeed;
        private Camera mainCamera;

        private void Awake()
        {
            session = FindFirstObjectByType<MicrogameSession>();
            mainCamera = Camera.main;
        }

        public void SetSteering(float input)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, input * maxSpeed, acceleration * Time.deltaTime);
        }


        // Update is called once per frame
        void Update()
        {
            if (!session.isTimerStarted) return;

            if (Pointer.current != null)
            {
                float screenX = Pointer.current.position.ReadValue().x;

                Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenX,Screen.height * 0.5f, Mathf.Abs(mainCamera.transform.position.z)));

                float steering = Mathf.InverseLerp(minX, maxX, worldPos.x) * 2f - 1f;
                
                SetSteering(steering);
            }

            Vector3 position = transform.position;

            position.x += currentSpeed * Time.deltaTime;
            position.x = Mathf.Clamp(position.x, minX, maxX);

            transform.position = position;

            
        }
    }
}
