using UnityEngine;

namespace TFM.Minigames
{
    /// <summary>
    /// Class <c>KiteMinigameObstacle</c> contains the logic for the kite minigame obstacle.
    /// </summary>
    public class KiteMinigameObstacle : MonoBehaviour
    {
        /// <value>Property <c>_moveSpeed</c> represents the move speed.</value>
        private float _moveSpeed;
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            _moveSpeed = KiteMinigameManager.Instance.obstacleMoveSpeed;
        }

        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            transform.position += Vector3.left * (_moveSpeed * Time.deltaTime);
            if (transform.position.x < KiteMinigameManager.Instance.obstacleDespawnPoint.position.x)
                Destroy(gameObject);
        }
    }
}