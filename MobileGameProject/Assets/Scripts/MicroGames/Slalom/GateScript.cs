using UnityEngine;
using MicrogameCourse.Framework;
using MicrogameCourse.Microgames;
using Unity.VisualScripting;


namespace MicrogameCourse.Framework
{

public class GateScript : MicrogameBehaviour
    {

        private enum GateSide {Left, Right, Centre};

        [SerializeField] private GateSide position;


        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            Debug.Log("Gate Script Awake");
            spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Gate Script Trigger");
            switch (position)
            {
                case GateSide.Left:
                    Debug.Log("Missed Gate Left");
                    SetGateColour(Color.red);
                    break;

                case GateSide.Right:
                    Debug.Log("Missed Gate Right");
                    SetGateColour(Color.red);
                    break;

                case GateSide.Centre:
                    Debug.Log("Successfully through date");
                    SetGateColour(Color.green);
                    break;

                default:
                    Debug.Log($"Unknown gate: {gameObject.name}");
                    break;
            }
        }

        public void SetGateColour(Color colour)
        {
            spriteRenderer.color = colour;
        }

    }


}