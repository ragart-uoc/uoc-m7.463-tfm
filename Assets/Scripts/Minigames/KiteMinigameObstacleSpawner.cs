using System.Collections;
using UnityEngine;

namespace TFM.Minigames
{
    /// <summary>
    /// Class <c>KiteMinigameObstacleSpawner</c> contains the logic for the obstacle spawner.
    /// </summary>
    public class KiteMinigameObstacleSpawner : MonoBehaviour
    {
        /// <value>Property <c>obstaclePrefab</c> represents the obstacle prefab.</value>
        public GameObject obstaclePrefab;
        
        /// <value>Property <c>spawnRate</c> represents the spawn rate.</value>
        public float spawnRate = 2f;
        
        /// <value>Property <c>obstacleHeightRange</c> represents the obstacle height range.</value>
        public float obstacleHeightRange = 1.5f;

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            StartCoroutine(SpawnObstacles());
        }

        /// <summary>
        /// Method <c>SpawnObstacles</c> spawns the obstacles.
        /// </summary>
        private IEnumerator SpawnObstacles()
        {
            while (true)
            {
                var obstacleHeight = Random.Range(-obstacleHeightRange, obstacleHeightRange);
                var spawnPosition = new Vector3(transform.position.x, obstacleHeight, 0);
                Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnRate);
            }
        }
    }
}