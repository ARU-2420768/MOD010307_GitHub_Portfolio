using UnityEngine;
using MicrogameCourse.Framework;
using MicrogameCourse.Microgames;
using Unity.VisualScripting;


namespace MicrogameCourse.Framework
{

public sealed class GateSuccess : MicrogameBehaviour
    {
        private SlalomGame game;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            game = FindFirstObjectByType<SlalomGame>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {

            switch (this.tag)
            {
                case "GateSuccess":
                    Debug.Log("Gate Successfull - GateSuccess.cs - calling game.GatePassed");
                    game.GatePassed();
                    //game.ShowFeedback("Gate Success !", Color.green);
                    //game.gateMoveSpeed = game.gateMinMoveSpeed;
                    break;

                case "GateFailure":
                    Debug.Log("Gate Failed");
                    SetGateColour(Color.red);
                    game.ShowFeedback("Gate Missed !", Color.red);
                    game.gateMoveSpeed = game.gateMinMoveSpeed;
                    break;

                default:
                    Debug.Log($"Unknown gate: {gameObject.name}, This Tag: {this.tag}, Collision Tag: {collision.tag}.");
                    break;
            }
        }

        public void SetGateColour(Color colour)
        {
            //    spriteRenderer.color = colour;
        }
    }
}