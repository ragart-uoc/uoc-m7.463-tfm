using UnityEngine;
using UnityEngine.InputSystem;

namespace TFM.Minigames
{
    /// <summary>
    /// Class <c>KiteMinigameController</c> contains the logic for the kite controller.
    /// </summary>
    public class KiteMinigameController : MonoBehaviour
    {
        
        /// <value>Property <c>mainCamera</c> represents the main camera.</value>
        public Camera mainCamera;
        
        /// <value>Property <c>moveSpeed</c> represents the move speed.</value>
        private float _moveSpeed;
        
        /// <value>Property <c>_moveInput</c> represents the move input.</value>
        private Vector2 _moveInput;
        
        /// <value>Property <c>_controls</c> represents the controls.</value>
        private Controls _controls;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _controls = new Controls();
        }

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            _moveSpeed = KiteMinigameManager.Instance.kiteMoveSpeed;
            mainCamera = mainCamera ?? Camera.main;
        }
        
        /// <summary>
        /// Method <c>OnEnable</c> is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            _controls.Kite.Enable();
        }
        
        /// <summary>
        /// Method <c>OnDisable</c> is called when the object becomes disabled and inactive.
        /// </summary>
        private void OnDisable()
        {
            _controls.Kite.Disable();
        }

        /// <summary>
        /// Method <c>OnMove</c> is called when the move input is received.
        /// </summary>
        /// <param name="value">The move input value.</param>
        public void OnMove(InputValue value)
        {
            _moveInput = value.Get<Vector2>();
        }

        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            var screenToWorldPosition = new Vector3(_moveInput.x, _moveInput.y, -mainCamera.transform.position.z);
            var mousePosition = mainCamera.ScreenToWorldPoint(screenToWorldPosition);
            var newPosition = new Vector3(transform.position.x, Mathf.Clamp(mousePosition.y, 2.5f, 12f), transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, _moveSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the object enters the trigger.
        /// </summary>
        /// <param name="col">The collider that the object collided with.</param>
        private void OnTriggerEnter(Collider col)
        {
            switch (col.gameObject.tag)
            {
                case "KiteObstacle":
                    KiteMinigameManager.Instance.GameOver();
                    break;
                case "KiteScore":
                    KiteMinigameManager.Instance.IncreaseScore();
                    break;
            }
        }
    }
}