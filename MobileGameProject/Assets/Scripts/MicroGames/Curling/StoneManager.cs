using System;
using UnityEngine;

namespace MicrogameCourse.Microgames
{ 

    public class StoneManager : MonoBehaviour
    {
        public Rigidbody rb;
        public bool IsMoving => rb.linearVelocity.magnitude > 0.3f;
        public bool IsAbleToPlay = true;
        public event Action OnStoneStopped;

        private bool hasBeenLaunched = false;
        private bool hasStartedMoving = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!hasBeenLaunched)
                return;

            if (!hasStartedMoving)
            {
                // Wait until the stone actually starts moving
                if (IsMoving)
                {
                    hasStartedMoving = true;
                }
                return;
            }

            // Now we know it has moved — wait for it to stop
            if (!IsMoving)
            {
                hasBeenLaunched = false;
                hasStartedMoving = false;
                Debug.Log($"{name} Stopped");
                OnStoneStopped?.Invoke();
            }
        }

        public void Launch(Vector2 force)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.WakeUp();

            rb.AddForce(force, ForceMode.Impulse);

            hasBeenLaunched = true;
            hasStartedMoving = false;   // reset the flag
        }
    }
}
