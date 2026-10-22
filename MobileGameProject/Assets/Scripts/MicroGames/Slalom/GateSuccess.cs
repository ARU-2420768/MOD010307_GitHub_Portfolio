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

            if(collision.tag != "Player") return;
            

            switch (this.tag)
            {
                case "GateSuccess":
                    game.GatePassed();
                    break;

                case "GateFailure":
                    game.GateMissed();
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