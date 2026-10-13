using System.Collections.Generic;
using UnityEngine;

namespace MicrogameCourse.ProgressionFramework
{
    public class MedalManager : MonoBehaviour
    {
        private Dictionary<string, MedalType> medals = new Dictionary<string, MedalType>();

        //private List<EventResult> medalsList;

        public void AwardMedal(string eventName, MedalType medal)
        {
            // only award medal if the medal is a better medal that the player already holds for the event
            if(GetMedal(eventName) > medal) return;
            medals[eventName] = medal;
        }

        public MedalType GetMedal(string eventName)
        {
            // search through medals for medal type, if it doesn't exist return default - "Unranked"
            return medals.TryGetValue(eventName, out var medal) ? medal: MedalType.Unranked;
        }

    }
}
