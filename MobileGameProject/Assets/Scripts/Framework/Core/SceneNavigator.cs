using System;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MicrogameCourse.Framework
{

    public sealed class SceneNavigator : MonoBehaviour
    {
        private const string MenuScene = "MainMenu";

        private const string PracticeScene = "PracticeTap";

        private const string SlalomScene = "Slalom";
        private const string ShootingScene = "Shooting";
        private const string SkiJumpScene = "SkiJump";
        private const string CurlingScene = "Curling";

        private bool isLoading;

        public void OpenSlalom()
        {
            SceneManager.LoadScene(SlalomScene);                
        }

        public void OpenShootingGame()
        {
            SceneManager.LoadScene(ShootingScene);                
        }

        public void OpenSkiJumpGame()
        {
            SceneManager.LoadScene(SkiJumpScene);                
        }

        public void OpenCurlingGame()
        {
            SceneManager.LoadScene(CurlingScene);                
        }

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