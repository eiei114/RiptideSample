using UnityEngine.SceneManagement;

namespace _ClientSample.Scripts.Util
{
    public class SceneManagerExtension
    {
        //シーン遷移の拡張
        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        
        public static void LoadScene(int sceneBuildIndex)
        {
            SceneManager.LoadScene(sceneBuildIndex);
        }
        
        public static void LoadSceneAsync(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
        
        public static void LoadSceneAsync(int sceneBuildIndex)
        {
            SceneManager.LoadSceneAsync(sceneBuildIndex);
        }
        
        public static void LoadSceneAdditive(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        
        
        public static void LoadSceneAdditive(int sceneBuildIndex)
        {
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Additive);
        }
        
        public static void LoadSceneAdditiveAsync(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}