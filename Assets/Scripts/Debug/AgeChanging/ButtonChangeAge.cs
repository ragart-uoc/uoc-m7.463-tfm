using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TFM.Debug.AgeChanging
{
    /// <summary>
    /// Class <c>ButtonChangeAge</c> contains the logic for changing the age group represented in the scene.
    /// </summary>
    public class ButtonChangeAge : MonoBehaviour
    {
        /// <value>Property <c>ageGroup</c> represents the target age group.</value>
        public AgeProperties.Groups ageGroup;
        
        /// <summary>
        /// Method <c>ChangeAge</c> changes the age group.
        /// </summary>
        public void ChangeAge()
        {
            GameManager.Instance.ChangeAgeGroup(ageGroup);
        }
    }
}
