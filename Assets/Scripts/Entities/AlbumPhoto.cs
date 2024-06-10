using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TFM.Managers;
using TFM.Managers.SceneManagers;

namespace TFM.Entities
{
    /// <summary>
    /// Class <c>AlbumPhoto</c> represents the album photos.
    /// </summary>
    public class AlbumPhoto : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        #region Object properties and components
        
            /// <value>Property <c>_image</c> represents the image component of the album photo.</value>
            private Image _image;
        
            /// <value>Property <c>_initialColor</c> represents the initial color of the image.</value>
            private Color _imageInitialColor;
            
            /// <value>Property <c>_currentPosition</c> represents the current position of the album photo.</value>
            private Vector3 _currentPosition;
            
            /// <value>Property <c>_currentParent</c> represents the current parent of the album photo.</value>
            private Transform _currentParent;
            
            /// <value>Property <c>_rootCanvas</c> represents the root canvas of the album photo.</value>
            private Transform _rootCanvas;
        
        #endregion
        
        #region Settings
            
                /// <value>Property <c>levelName</c> represents the level name of the album photo.</value>
                public string levelName;
                
                /// <value>Property <c>dragColor</c> represents the drag color of the album photo.</value>
                public Color dragColor;
        
        #endregion
        
        #region State
        
            /// <value>Property <c>_isDragging</c> represents the dragging state of the album photo.</value>
            private bool _isDragging;
            
        #endregion

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Get the image component and its color
            _image = GetComponent<Image>();
            _imageInitialColor = _image.color;
            
            // Get the current position, parent and root canvas
            _currentPosition = transform.position;
            _currentParent = transform.parent;
            _rootCanvas = GetComponentInParent<Canvas>().rootCanvas.transform;
        }
        
        /// <summary>
        /// Method <c>OnBeginDrag</c> is called when a drag event is beginning.
        /// </summary>
        /// <param name="eventData">The data of the event.</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (SceneAlbumManager.Instance.dragEnabled == false)
                return;

            // Set the dragging state to true
            _isDragging = true;
            
            // Disable the raycast target
            _image.raycastTarget = false;
            
            // Save the current position
            _currentPosition = transform.position;
            
            // Set the parent to the root canvas
            transform.SetParent(_rootCanvas);
            
            // Change the color of the image
            _image.color = dragColor;
        }

        /// <summary>
        /// Method <c>OnDrag</c> is called when a drag event is happening.
        /// </summary>
        /// <param name="eventData">The data of the event.</param>
        public void OnDrag(PointerEventData eventData)
        {
            // Move the album photo to the event data position
            transform.position = eventData.position;
        }
        
        /// <summary>
        /// Method <c>OnEndDrag</c> is called when a drag event is ending.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            // If the parent is the root canvas, move back to the original parent and position
            if (transform.parent == _rootCanvas)
            {
                transform.SetParent(_currentParent);
                transform.position = _currentPosition;
            }
            // Update the current parent
            _currentParent = transform.parent;
            
            // Reset the color
            _image.color = _imageInitialColor;
            
            // Enable the raycast target
            _image.raycastTarget = true;
            
            // Set the dragging state to false
            _isDragging = false;
        }
        
        /// <summary>
        /// Method <c>OnPointerDown</c> is called when a pointer down event is happening.
        /// </summary>
        /// <param name="eventData">The data of the event.</param>
        public void OnPointerDown(PointerEventData eventData)
        {
        }
        
        /// <summary>
        /// Method <c>OnPointerUp</c> is called when a pointer up event is happening.
        /// </summary>
        /// <param name="eventData">The data of the event.</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isDragging)
                return;
            CustomSceneManager.Instance.LoadLevel(levelName);
        }
        
        /// <summary>
        /// Method <c>SetPlaceholder</c> sets the placeholder of the album photo.
        /// </summary>
        /// <param name="placeholder">The placeholder transform.</param>
        /// <param name="animateMove">Whether to animate the move.</param>
        public void SetPlaceholder(Transform placeholder, bool animateMove = true)
        {
            transform.SetParent(placeholder);
            if (animateMove)
                StartCoroutine(MoveTowards(transform, placeholder.position, 0.1f));
            else
                transform.position = placeholder.position;
        }
        
        /// <summary>
        /// Method <c>MoveTowards</c> moves the album photo towards the destination.
        /// </summary>
        /// <param name="target">The album photo transform.</param>
        /// <param name="destination">The destination position.</param>
        /// <param name="duration">The duration of the movement.</param>
        private IEnumerator MoveTowards(Transform target, Vector3 destination, float duration)
        {
            var time = 0f;
            var startPosition = target.position;
            while (time < duration)
            {
                target.position = Vector3.Lerp(startPosition, destination, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            target.position = destination;
        }
    }
}
