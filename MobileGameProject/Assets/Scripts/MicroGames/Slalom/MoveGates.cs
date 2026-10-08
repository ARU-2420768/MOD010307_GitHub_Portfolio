using UnityEngine;
using MicrogameCourse.Framework;
using MicrogameCourse.Microgames;
using Unity.VisualScripting;

namespace MicrogameCourse.Framework
{
    public sealed class MoveGates : MicrogameBehaviour
    {

        [SerializeField] float gateSpeedIncrease = 2f;
        [SerializeField] float fastGateSpeedIncrease = 4f;

        //private float moveSpeed = 1f;
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
            transform.position += game.gateMoveSpeed * Time.deltaTime * Vector3.down;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (gameObject.name)
            {
                case "SGate_Start(Clone)":                                        
                case "SGate_Standard(Clone)":
                    SetGateColour(Color.green);
                    game.gateMoveSpeed *= gateSpeedIncrease; 
                    break;

                case "SGate_Fast(Clone)":                                      
                    SetGateColour(Color.green);
                    game.gateMoveSpeed *= fastGateSpeedIncrease; 
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