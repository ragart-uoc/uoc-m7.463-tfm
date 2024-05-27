using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TFM.Managers;

namespace TFM.Entities
{
    /// <summary>
    /// Class <c>RadialMenuOptionUI</c> contains the logic for the radial menu UI option
    /// </summary>
    public class RadialMenuOptionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /// <value>Property <c>Type</c> representes the types that the radial menu option can have.</value>
        public enum Type
        {
            Inner,
            Outer
        }
        
        /// <value>Property <c>type</c> represents the type of the radial menu option.</value>
        public Type type;
        
        /// <value>Property <c>background</c> represents the background image.</value>
        public Image background;
        
        /// <value>Property <c>iconSprite</c> represents the icon image.</value>
        public Image icon;
        
        /// <value>Property <c>radialMenuOption</c> represents the radial menu option.</value>
        public RadialMenuOption radialMenuOption;
        
        /// <summary>
        /// Method <c>OnPointerEnter</c> is called when the pointer enters the object.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            var color = background.color;
            background.color = new Color(color.r, color.g, color.b, 1.0f);
            switch (type)
            {
                case Type.Inner:
                    UIManager.Instance.radialMenu.SpawnOuterMenu(radialMenuOption.type);
                    break;
                case Type.Outer when radialMenuOption.item != null:
                    UIManager.Instance.radialMenu.targetItem = radialMenuOption.item;
                    UIManager.Instance.SetStatusBarText("Use " + radialMenuOption.item.Title + " on " +
                                                        UIManager.Instance.radialMenu.target.name);
                    break;
            }
        }
        
        /// <summary>
        /// Method <c>OnPointerExit</c> is called when the pointer exits the object.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            var color = background.color;
            background.color = new Color(color.r, color.g, color.b, 0.5f);
            switch (type)
            {
                case Type.Inner:
                    break;
                case Type.Outer when radialMenuOption.item != null:
                    UIManager.Instance.radialMenu.targetItem = null;
                    UIManager.Instance.SetStatusBarText(UIManager.Instance.radialMenu.target.name);
                    break;
            }
        }
    }
}
