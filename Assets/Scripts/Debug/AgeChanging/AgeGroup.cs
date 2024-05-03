using UnityEngine;

namespace TFM.Debug.AgeChanging
{
    /// <summary>
    /// Struct <c>AgeGroup</c> contains the properties for age groups.
    /// </summary>
    [System.Serializable]
    public struct AgeGroup
    {
        /// <value>Property <c>Group</c> represents the age group.</value>
        public AgeProperties.Groups group;
        
        /// <value>Property <c>Characters</c> represents the characters in the age group.</value>
        public GameObject[] characters;
    }
}
