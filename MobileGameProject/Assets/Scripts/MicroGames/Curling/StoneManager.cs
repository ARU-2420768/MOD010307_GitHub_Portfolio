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

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnMouseDown()
        {
            Debug.Log("Stone clicked");
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
