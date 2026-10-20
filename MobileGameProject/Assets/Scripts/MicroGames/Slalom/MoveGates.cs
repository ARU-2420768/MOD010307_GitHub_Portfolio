using UnityEngine;
using MicrogameCourse.Framework;
using MicrogameCourse.Microgames;
using Unity.VisualScripting;

namespace MicrogameCourse.Framework
{
    public sealed class MoveGates : MicrogameBehaviour
    {

        private float moveSpeed = 1f;
        private MicrogameSession session;
        private SlalomGame game;


        private void Awake()
        {
            session = FindFirstObjectByType<MicrogameSession>();
            game = FindFirstObjectByType<SlalomGame>();
        }

        private void Update()
        {
            if (!session.isTimerStarted) return;
            transform.position += moveSpeed * Time.deltaTime * Vector3.down;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (gameObject.name)
            {
                case "SGate_Start(Clone)":                                        
                    SetGateColour(Color.green);
                    break;

                case "SGate_Start":
                    Debug.Log("Start Line Crossed");
                    SetGateColour(Color.green);
                    break;
                case "SGate":
                    Debug.Log("Standard gate");
                    SetGateColour(Color.green);
                    break;

                case "SGate_Fast(Clone)":
                    Debug.Log("Fast gate (Clone)");
                    SetGateColour(Color.green);
                    break;

                case "SGate(Clone)":
                    Debug.Log("Standard gate (Clone)");
                    SetGateColour(Color.green);
                    break;

                default:
                    Debug.Log($"Unknown gate: {gameObject.name}");
                    break;
            }
        }

        public void GateSuccess(Color colour)
        {
            SetGateColour(Color.green);
        }
        public void GateFailed(Color colour)
        {
            SetGateColour(Color.red);
        }

        public void SetGateColour(Color colour)
        {
            foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = colour;
            }
        }
    }
}