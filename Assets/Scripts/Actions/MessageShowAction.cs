using TFM.Entities;
using TFM.Managers;

namespace TFM.Actions
{
    /// <summary>
    /// Class <c>MessageShowAction</c> represents the show message action.
    /// </summary>
    [System.Serializable]
    public class MessageShowAction : ActionBase
    {
        /// <value>Property <c>message</c> represents the message text.</value>
        public string message;

        /// <value>Property <c>duration</c> represents the message duration.</value>
        public float duration;

        /// <value>Property <c>afterMessage</c> represents the after message text.</value>
        public string afterMessage;

        /// <summary>
        /// Method <c>Execute</c> executes the action.
        /// </summary>
        public override void Execute()
        {
            UIManager.Instance.ShowMessage(message, duration, afterMessage);
        }
    }
}