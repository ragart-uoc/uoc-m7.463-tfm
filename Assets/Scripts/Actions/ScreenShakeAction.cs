using UnityEngine;
using TFM.Components;
using TFM.Entities;

namespace TFM.Actions
{
    /// <summary>
    /// Class <c>ScreenShakeAction</c> represents the screen shake action.
    /// </summary>
    [System.Serializable]
    public class ScreenShakeAction : ActionBase
    {
        /// <summary>
        /// Method <c>Execute</c> executes the action.
        /// </summary>
        public override void Execute()
        {
            Camera.main?.GetComponent<ScreenShake>()?.ShakeScreen();
        }
    }
}
