using UnityEngine;
using UnityEngine.UI;
using MicrogameCourse.Framework;

namespace MicrogameCourse.Microgames
{
    public class SlalomPlayer : MicrogameBehaviour
    {
        [SerializeField] private Image left;
        [SerializeField] private Image right;
        public Color colour;

        public void OnValidate()
        {
            left.color = right.color = colour;
        }
    }
}