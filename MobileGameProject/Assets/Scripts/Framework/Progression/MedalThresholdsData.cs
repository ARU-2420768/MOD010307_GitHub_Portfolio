using UnityEngine;

namespace MicrogameCourse.ProgressionFramework
{
    [CreateAssetMenu(fileName = "MedalThresholdsData", menuName = "Winter Olympics/Medal Thresholds")]
    public class MedalThresholdsData : ScriptableObject
    {
        public int bronze, silver, gold;        
    }
}


