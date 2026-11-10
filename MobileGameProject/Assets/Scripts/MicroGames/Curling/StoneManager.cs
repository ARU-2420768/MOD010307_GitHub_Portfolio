using UnityEngine;

namespace MicrogameCourse.Microgames
{ 

    public class StoneManager : MonoBehaviour
    {

        private Rigidbody rb;

        public bool IsMoving => rb.linearVelocity.magnitude > 0.05f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            Debug.Log("Stone Manager Awake");
        }



        public void Launch(Vector2 force)
        {
            rb.linearVelocity = Vector2.zero;

            rb.AddForce(
                force,
                ForceMode.Impulse);
        }

    }
}
