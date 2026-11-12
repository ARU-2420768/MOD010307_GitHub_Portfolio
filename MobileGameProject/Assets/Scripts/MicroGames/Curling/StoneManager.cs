using System;
using UnityEngine;

namespace MicrogameCourse.Microgames
{ 

    public class StoneManager : MonoBehaviour
    {

        public Rigidbody rb;

        public bool IsMoving => rb.linearVelocity.magnitude > 0.05f;
        public bool IsAbleToPlay = true;
        public event Action OnStoneStopped;

        private bool wasMoving = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            bool isCurrentlyMoving = IsMoving;
            if(wasMoving && !isCurrentlyMoving)
            {
                Debug.Log("Stone Stopped");
                OnStoneStopped?.Invoke();
            }
            wasMoving = isCurrentlyMoving;
        }

        public void Launch(Vector2 force)
        {
            rb.linearVelocity = Vector2.zero;

            rb.AddForce(
                force,
                ForceMode.Impulse);

            wasMoving = true;
        }

    }
}
