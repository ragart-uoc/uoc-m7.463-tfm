using UnityEngine;

namespace TFM.Debug.Scripts.AgeChanging
{
    /// <summary>
    /// Struct <c>AgeGroup</c> contains the properties for age groups.
    /// </summary>
    [System.Serializable]
    public struct AgeGroup
    {
        /// <value>Property <c>group</c> represents the age group.</value>
        public AgeProperties.Groups group;
        
        /// <value>Property <c>objects</c> represents the objects in the age group.</value>
        public GameObject[] objects;
    }
}
