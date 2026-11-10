using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace MicrogameCourse.Framework.UI
{
    
    public class GameInformationDisplay : MonoBehaviour
    {

        [Header("Game Heading and Instructions")]
        [SerializeField] private GameConfigurationData gameConfig;
        [SerializeField] private TextMeshProUGUI gameHeadingText;
        [SerializeField] private TextMeshProUGUI gameReadyText;
        [SerializeField] private TextMeshProUGUI feedbackText;

        private void Start()
        {
            gameHeadingText.text = gameConfig.gameName;
            gameReadyText.text = gameConfig.description;
        }

        public void SuccessFeedback()
        {
            feedbackText.text = gameConfig.gameFeedbackData.success;
        }

        public void FailureFeedback()
        {
            feedbackText.text = gameConfig.gameFeedbackData.failure;
        }

    }

}
