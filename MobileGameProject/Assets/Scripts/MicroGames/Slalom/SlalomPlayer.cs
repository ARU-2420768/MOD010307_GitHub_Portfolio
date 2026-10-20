using UnityEngine;
using UnityEngine.UI;

namespace MicrogameCourse.Microgames
{
    public class SlalomPlayer : MonoBehaviour
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