using System;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MicrogameCourse.Framework
{

    public sealed class SceneNavigator : MonoBehaviour
    {
        private const string MenuScene = "MainMenu";
        private const string PracticeScene = "PracticeTap";
        private bool isLoading;

        public void OpenPractice()
        {
            SceneManager.LoadScene(PracticeScene);                
        }

        public void OpenMenu()
        {
            SceneManager.LoadScene(MenuScene);
        }

        public void Replay()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
        
    
        private void Load(string sceneName)
        {
            if(isLoading)
            {
                return;
            }

            if (Application.CanStreamedLevelBeLoaded(sceneName))

            {
                isLoading = true;
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogError($"Scene '{sceneName}' is not in the Build Profiles Scene List.");
            }

            isLoading = true;
            SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Single);
        }

    }

}