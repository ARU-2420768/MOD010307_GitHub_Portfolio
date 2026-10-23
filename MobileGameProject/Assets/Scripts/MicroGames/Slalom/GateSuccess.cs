using UnityEngine;
using MicrogameCourse.Framework;
using MicrogameCourse.Microgames;
using Unity.VisualScripting;
using UnityEngine.Events;


namespace MicrogameCourse.Framework
{

public sealed class GateSuccess : MicrogameBehaviour
    {
        private SlalomGame game;

        private SpriteRenderer spriteRenderer;

        public UnityEvent<string> onGateTriggered;

        private void Awake()
        {
            game = FindFirstObjectByType<SlalomGame>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {

            if(collision.tag != "Player") return;
            
            onGateTriggered?.Invoke(this.tag);
            Debug.Log(this.tag);
/*
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
  
  */
        }

        public void SetGateColour(Color colour)
        {
            //    spriteRenderer.color = colour;
        }
    }
}