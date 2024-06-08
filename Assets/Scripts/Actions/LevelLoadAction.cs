using TFM.Entities;
using TFM.Managers;

namespace TFM.Actions
{
    /// <summary>
    /// Class <c>LevelLoadAction</c> represents the level load action.
    /// </summary>
    [System.Serializable]
    public class LevelLoadAction : ActionBase
    {
        /// <value>Property <c>levelName</c> represents the name of the scene.</value>
        public Level level;
        
        /// <value>Property <c>fadeOverlay</c> represents the fade overlay.</value>
        public int fadeOverlay;

        /// <value>Property <c>destroyPersistentManagers</c> represents the destroy persistent managers.</value>
        public int destroyPersistentManagers;

        /// <summary>
        /// Method <c>Execute</c> executes the action.
        /// </summary>
        public override void Execute()
        {
            if (fadeOverlay == 1)
                UIManager.Instance?.FadeOverlay(1.0f, 3.0f, LevelLoad);
            else
                LevelLoad();
        }

        /// <summary>
        /// Method <c>LevelLoad</c> triggers the level load.
        /// </summary>
        private void LevelLoad()
        {
            CustomSceneManager.Instance.LoadLevel(level.name, destroyPersistentManagers == 1);
        }
    }
}
