using UnityEngine;
using System.Collections.Generic;


namespace MicrogameCourse.Microgames
{

    public class CalculateScore : MonoBehaviour
    {

        [SerializeField] private Transform center;
        [SerializeField] private SpriteRenderer outerCircle;
        public List<StoneManager> stones = new();

        public Vector3 centrePosition;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            centrePosition = center.position;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public int Calculate(List<StoneManager> allStones)
        {
            if (outerCircle == null || allStones == null || allStones.Count == 0)
                return 0;

            List<StoneManager> sortedStones = SortStones(allStones);

            // If the closest stone is outside the outerCircle's X/Y perimeter, score is 0
            if (!IsStoneInHouse(sortedStones[0]))
                return 0;

            int winningTeam = WinningTeam(sortedStones[0]);
            int points = 0;

            for (int i = 0; i < sortedStones.Count; i++)
            {

                // Count only if inside the outerCircle in 2D AND on the winning team
                if (IsStoneInHouse(sortedStones[i]) && sortedStones[i].teamId == winningTeam)
                {
                    Debug.Log($"Scoring Stone: {sortedStones[i].name}");
                    points++;
                }
                else
                {
                    break;
                }
            }

            return points;
        }

        private bool IsStoneInHouse(StoneManager stone)
        {
            // Outer radius of the sprite on the 2D X/Y plane
            float outerRadius = outerCircle.bounds.extents.x;

            // Add the collider of the stone to calculate whether the stone is touching the house circle.
            float stoneRadius = 0f;
            Collider col = stone.GetComponentInChildren<Collider>();
            if (col != null)
            {
                stoneRadius = col.bounds.extents.x;
            }

            Vector2 center2D = new Vector2(center.transform.position.x, center.transform.position.y);
            Vector2 stone2D = new Vector2(stone.transform.position.x, stone.transform.position.y);

            float distance = Vector2.Distance(center2D, stone2D);

            return distance <= (outerRadius + stoneRadius);
        }

        public List<StoneManager> SortStones(List<StoneManager> allStones)
        {
            allStones.Sort((a, b) =>
            {
                float distA = (a.transform.position - centrePosition).sqrMagnitude;
                float distB = (b.transform.position - centrePosition).sqrMagnitude;
                return distA.CompareTo(distB);
            });
            return allStones;
        }

        public int WinningTeam(StoneManager winningStone)
        {
            return winningStone.teamId;
        }


    }
}
