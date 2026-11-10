using UnityEngine;
using MicrogameCourse.Framework;
using MicrogameCourse.Microgames;
using Unity.VisualScripting;

namespace MicrogameCourse.Framework
{
    public sealed class MoveGates : MicrogameBehaviour
    {
        private MicrogameSession session;
        private SlalomGame game;



        private void Awake()
        {
            session = FindFirstObjectByType<MicrogameSession>();
            game = FindFirstObjectByType<SlalomGame>();
            //Debug.Log($"{gameObject.name} tag = {gameObject.tag}");
        }

        private void Update()
        {
            if (!session.isTimerStarted) return;

            Debug.Log($"MoveGates speed = {game.gateMoveSpeed}");
            transform.position += game.gateMoveSpeed * Time.deltaTime * Vector3.up;
            //transform.position += Mathf.Clamp(game.gateMoveSpeed,game.gateMinMoveSpeed, game.gateMaxMoveSpeed) * Time.deltaTime * Vector3.up;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            //if (!collision.CompareTag("Gate")) return;

            Debug.Log(collision.tag);
            
            switch (gameObject.name)
            {
                case "SGate_Start(Clone)":                                        
                case "SGate_Standard(Clone)":
                    SetGateColour(Color.green);
                    //game.GatePassed();
                    break;

                case "SGate_Fast(Clone)":                                      
                    SetGateColour(Color.green);
                    game.ShowFeedback("Fast Gate Passed !", Color.green);
                    game.score ++;
                    game.UpdateProgress();
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