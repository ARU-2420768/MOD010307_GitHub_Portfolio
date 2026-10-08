using UnityEngine;
using MicrogameCourse.Framework;
using MicrogameCourse.Microgames;
using Unity.VisualScripting;


namespace MicrogameCourse.Framework
{

public sealed class RemoveGate : MonoBehaviour

    {

        private SlalomGame game;

        private void Awake()
        {
            game = FindFirstObjectByType<SlalomGame>();
        }

        private void Start()
        {
            BoxCollider2D bc = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Gate"))
            {
                game.CreateNewGate();
                Destroy(collision.gameObject);
            }
        }

    }
}