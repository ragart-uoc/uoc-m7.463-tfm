namespace TFM.Entities
{
    /// <summary>
    /// Class <c>ActionBase</c> represents an action.
    /// </summary>
    [System.Serializable]
    public abstract class ActionBase
    {
        /// <value>Property <c>waitForInput</c> represents if an input is expected to continue.</value>
        public int waitForInput;

        /// <value>Property <c>waitAfterAction</c> represents the time to wait after the action.</value>
        public float waitAfterAction;

        /// <summary>
        /// Method <c>Execute</c> executes the action.
        /// </summary>
        public abstract void Execute();
    }
}
