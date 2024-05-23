using UnityEngine.SceneManagement;
using TFM.Managers;

namespace TFM.Actions
{
    /// <summary>
    /// Class <c>SceneChangeAction</c> represents a scene change action.
    /// </summary>
    [System.Serializable]
    public class SceneChangeAction : ActionBase
    {
        /// <value>Property <c>sceneName</c> represents the name of the scene.</value>
        public string sceneName;
        
        /// <value>Property <c>fadeOverlay</c> represents the fade overlay.</value>
        public int fadeOverlay;

        /// <summary>
        /// Method <c>Execute</c> executes the action.
        /// </summary>
        public override void Execute()
        {
            if (fadeOverlay == 1)
                UIManager.Instance.FadeOverlay(1.0f, 3.0f, ChangeScene);
            else
                CustomSceneManager.Instance.LoadNewScene(sceneName);
        }
        
        /// <summary>
        /// Method <c>ChangeScene</c> changes the scene.
        /// </summary>
        private void ChangeScene()
        {
            CustomSceneManager.Instance.LoadNewScene(sceneName);
        }
    }
}
