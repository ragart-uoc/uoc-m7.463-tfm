using TFM.Entities;
using TFM.Managers;

namespace TFM.Actions
{
    /// <summary>
    /// Class <c>LevelChangeAgeGroupAction</c> represents the level change age group action.
    /// </summary>
    [System.Serializable]
    public class LevelChangeAgeGroupAction : ActionBase
    {
        /// <value>Property <c>ageGroup</c> represents the age group.</value>
        public AgeGroupProperties.Groups ageGroup;

        /// <summary>
        /// Method <c>Execute</c> executes the action.
        /// </summary>
        public override void Execute()
        {
            LevelManager.Instance.CurrentLevelChangeAgeGroup(ageGroup);
        }
    }
}