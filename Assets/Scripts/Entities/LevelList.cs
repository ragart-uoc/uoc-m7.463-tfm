using System.Collections.Generic;
using UnityEngine;

namespace TFM.Entities
{
    /// <summary>
    /// Class <c>LevelList</c> represents a list of levels.
    /// </summary>
    [CreateAssetMenu(fileName = "LevelList", menuName = "Custom/LevelList")]
    public class LevelList : ScriptableObject
    {
        public List<Level> levels = new List<Level>();
        
        /// <value>Property <c>levelsFolder</c> represents the levels folder.</value>
        public string levelsFolder = "Assets/ScriptableObjects/Levels";
    }
}
