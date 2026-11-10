using MicrogameCourse.Framework.Progression;
using MicrogameCourse.ProgressionFramework;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private MedalDisplay[] medalDisplays;

    private void Start()
    {
        RefreshMedals();
    }

    private void RefreshMedals()
    {
        foreach(MedalDisplay medalDisplay in medalDisplays)
        {
            MedalType medal = MedalManager.Instance.GetMedal(medalDisplay.eventName);

            switch (medal)
            {
                case MedalType.Gold:
                    medalDisplay.medalImage.color = new Color32(255, 215, 0, 255);
                    medalDisplay.medalText.text = "1";
                    break;                    
                case MedalType.Silver:
                    medalDisplay.medalImage.color = new Color32(216, 216, 216, 255);
                    medalDisplay.medalText.text = "2";
                    break;                    
                case MedalType.Bronze:
                    medalDisplay.medalImage.color = new Color32(205, 127, 50, 255);
                    medalDisplay.medalText.text = "3";
                    break;                    
                default:
                    medalDisplay.medalImage.color = new Color32(138, 138, 138, 255);
                    medalDisplay.medalText.text = "U";
                    break;
            }

        }
    }
}
