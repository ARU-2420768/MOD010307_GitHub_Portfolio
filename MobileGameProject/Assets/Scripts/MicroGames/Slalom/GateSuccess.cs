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

        public UnityEvent<GameObject, string> onGateTriggered;

        private void Awake()
        {
            game = FindFirstObjectByType<SlalomGame>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {

            if(collision.tag != "Player") return;
            
            onGateTriggered?.Invoke(this.gameObject, this.tag);
        }

        public void SetGateColour(Color colour)
        {
            //    spriteRenderer.color = colour;
        }
    }
}