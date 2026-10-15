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

        private void Start()
        {
            gameHeadingText.text = gameConfig.gameName;
            gameReadyText.text = gameConfig.description;
        }
    }

}
