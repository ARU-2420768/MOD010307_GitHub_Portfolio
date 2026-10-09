using UnityEngine;
using MicrogameCourse.Framework;
using MicrogameCourse.Microgames;
using Unity.VisualScripting;


namespace MicrogameCourse.Framework
{

public sealed class GateScript : MicrogameBehaviour
    {
        [SerializeField] float gateSpeedReset = 1f;


        private enum GateSide {Left, Right, Centre};

        [SerializeField] private GateSide positionOfGate;

        private SlalomGame game;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            game = FindFirstObjectByType<SlalomGame>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {

            //if (collision.gameObject.name != "Bottom")
            //    return;

            switch (positionOfGate)
            {
                case GateSide.Left:
                case GateSide.Right:
                    SetGateColour(Color.red);
                    game.ShowFeedback("Gate Missed !", Color.red);
                    game.gateMoveSpeed = game.gateMinMoveSpeed;
                    break;

                case GateSide.Centre:
                    SetGateColour(Color.green);
                    break;

                default:
                    Debug.Log($"Unknown gate: {gameObject.name}, Position Of Gate: {positionOfGate}.");
                    break;
            }
        }

        public void SetGateColour(Color colour)
        {
                spriteRenderer.color = colour;
        }
    }
}