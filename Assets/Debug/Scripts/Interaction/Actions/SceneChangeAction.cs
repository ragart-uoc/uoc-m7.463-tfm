namespace TFM.Debug.Scripts.Interaction.Actions
{
    /// <summary>
    /// Class <c>SceneChangeAction</c> represents a scene change action.
    /// </summary>
    [System.Serializable]
    public class SceneChangeAction : ActionBase
    {
        /// <value>Property <c>sceneName</c> represents the name of the scene.</value>
        public string sceneName;

        /// <summary>
        /// Method <c>Execute</c> executes the action.
        /// </summary>
        public override void Execute()
        {
            UnityEngine.Debug.Log("Change Scene to: " + sceneName);
        }
    }
}
