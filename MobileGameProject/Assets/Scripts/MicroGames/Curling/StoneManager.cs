using System;
using UnityEngine;

namespace MicrogameCourse.Microgames
{ 

    public class StoneManager : MonoBehaviour
    {
        public Rigidbody rb;
        [SerializeField] private float stopThreshold = 1f;
        public int teamId;

        public bool IsMoving => rb.linearVelocity.magnitude > stopThreshold;
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

            //
        }

        private void FixedUpdate()
        {
            if (!hasBeenLaunched)
                return;

            if (!hasStartedMoving)
            {
                // Wait until the physics step applies the impulse
                if (IsMoving)
                {
                    hasStartedMoving = true;
                }
                return;
            }

            if (!IsMoving)
            {
                StopStone();
            }

        }

        public void StopStone()
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();

            hasBeenLaunched = false;
            hasStartedMoving = false;

            Debug.Log($"{name} Stopped");
            OnStoneStopped?.Invoke();
        }


        public void Launch(Vector2 force)
        {
            //rb.linearVelocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;
            //rb.WakeUp();

            rb.AddForce(force, ForceMode.Impulse);

            hasBeenLaunched = true;
            hasStartedMoving = false;   // reset the flag
        }
    }
}
