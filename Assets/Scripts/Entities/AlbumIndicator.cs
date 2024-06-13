using UnityEngine;
using UnityEngine.EventSystems;
using TFM.Managers;

namespace TFM.Entities
{
    /// <summary>
    /// Class <c>AlbumIndicator</c> represents the UI album indicator.
    /// </summary>
    public class AlbumIndicator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        /// <value>Property <c>albumName</c> represents the name of the album.</value>
        public string albumName;

        /// <value>Property <c>nextLevel</c> represents the next level to load.</value>
        public Level nextLevel;
        
        /// <value>Property <c>_isInteractionPossible</c> represents if the interaction is possible.</value>
        private bool _isInteractionPossible;
        
        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            _isInteractionPossible = UIManager.Instance.AreInteractionsEnabled();
        }
        
        /// <summary>
        /// Method <c>OnPointerEnter</c> is called when the pointer enters the object.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isInteractionPossible)
                return;
            transform.localScale *= 1.1f;
            UIManager.Instance.ShowMessage(albumName);
        }
        
        /// <summary>
        /// Method <c>OnPointerExit</c> is called when the pointer exits the object.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isInteractionPossible)
                return;
            transform.localScale /= 1.1f;
        }
        
        /// <summary>
        /// Method <c>OnPointerDown</c> is called when the pointer is down on the object.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerDown(PointerEventData eventData)
        {
        }
        
        /// <summary>
        /// Method <c>OnPointerUp</c> is called when the pointer is up on the object.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            CustomSceneManager.Instance.LoadLevel(nextLevel.name);
        }
    }
}
