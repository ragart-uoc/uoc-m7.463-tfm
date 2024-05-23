using UnityEngine;

namespace TFM.Minigames
{
    /// <summary>
    /// Class <c>KiteMinigameObstacle</c> contains the logic for the kite minigame obstacle.
    /// </summary>
    public class KiteMinigameObstacle : MonoBehaviour
    {
        /// <value>Property <c>moveSpeed</c> represents the move speed.</value>
        public float moveSpeed = 2f;

        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            transform.position += Vector3.left * (moveSpeed * Time.deltaTime);
            if (transform.position.x < -12f)
                Destroy(gameObject);
        }
    }
}