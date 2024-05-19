using UnityEngine;
using TFM.Debug.Scripts.Interaction.Managers;

namespace TFM.Debug.Scripts.Interaction
{
    /// <summary>
    /// Class <c>RadialMenu</c> contains the logic for the radial menu.
    /// </summary>
    public class RadialMenu : MonoBehaviour
    {
        #region General
            
            /// <value>Property <c>GapBetweenOptions</c> represents the gap between options.</value>
            private const float GapBetweenOptions = 1.0f;

            /// <value>Property <c>target</c> represents the target.</value>
            public Transform target;
            
            /// <value>Property <c>targetItem</c> represents the target item.</value>
            public Item targetItem;
            
        #endregion

        #region Inner menu

            /// <value>Property <c>radialMenuOptions</c> represents the radial menu options.</value>
            public RadialMenuOption[] innerOptions;
                
            /// <value>Property <c>radialMenuOptionUIs</c> represents the radial menu option UIs.</value>
            private RadialMenuOptionUI[] _innerOptionUIs;

            /// <value>Property <c>radialMenuOptionUIPrefab</c> represents the radial menu option UI prefab.</value>
            public RadialMenuOptionUI innerOptionPrefab;
            
            /// <value>Property <c>innerOptionsContainer</c> represents the radial menu inner options container.</value>
            public Transform innerOptionsContainer;
            
        #endregion
        
        #region Outer menu

            /// <value>Property <c>radialMenuOptions</c> represents the radial menu options.</value>
            private RadialMenuOption[] _outerOptions = new RadialMenuOption[8];
                
            /// <value>Property <c>radialMenuOptionUIs</c> represents the radial menu option UIs.</value>
            private RadialMenuOptionUI[] _outerOptionUIs = new RadialMenuOptionUI[8];
            
            /// <value>Property <c>_itemType</c> represents the item type.</value>
            private ItemProperties.Types _itemType;
        
            /// <value>Property <c>radialMenuOptionUIPrefab</c> represents the radial menu option UI prefab.</value>
            public RadialMenuOptionUI outerOptionPrefab;
            
            /// <value>Property <c>outerOptionsContainer</c> represents the radial menu outer options container.</value>
            public Transform outerOptionsContainer;
            
        #endregion

        /// <summary>
        /// Method <c>Start</c> is called on the frame when a script is enabled just before any of the Update methods are called the first time
        /// </summary>
        private void Start()
        {
            // Spawn the inner menu options
            _innerOptionUIs = new RadialMenuOptionUI[innerOptions.Length];
        }

        /// <summary>
        /// Method <c>SpawnOuterMenu</c> spawns the outer menu.
        /// </summary>
        /// <param name="itemType">The item type.</param>
        public void SpawnOuterMenu(ItemProperties.Types itemType)
        {
            // Destroy current outer menu
            CleanOptions(ref _outerOptions, ref _outerOptionUIs, 8);

            // Get the picked items for the type
            _itemType = itemType;
            var items = ItemManager.Instance.GetPickedItems(_itemType);
            for (var i = 0; i < _outerOptions.Length; i++)
            {
                // Set the outer options
                _outerOptions[i] = i < items.Count
                    ? new RadialMenuOption
                    {
                        icon = items[i].Icon,
                        title = items[i].Title,
                        type = items[i].Type,
                        item = items[i]
                    }
                    : new RadialMenuOption
                    {
                        icon = null,
                        title = "",
                        type = _itemType,
                        item = null
                    };
            }
            
            // Spawn the outer menu options
            SpawnMenuOptions(ref _outerOptions, out _outerOptionUIs, outerOptionPrefab, outerOptionsContainer);
        }

        /// <summary>
        /// Method <c>SpawnMenuOptions</c> spawns the menu options.
        /// </summary>
        /// <param name="radialMenuOptions">The radial menu options.</param>
        /// <param name="radialMenuOptionUIs">The radial menu option UIs.</param>
        /// <param name="radialMenuOptionUIPrefab">The radial menu option UI prefab.</param>
        /// <param name="radialMenuOptionContainer">The option container.</param>
        private void SpawnMenuOptions(ref RadialMenuOption[] radialMenuOptions, out RadialMenuOptionUI[] radialMenuOptionUIs, RadialMenuOptionUI radialMenuOptionUIPrefab, Transform radialMenuOptionContainer = null)
        {
            // Calculate the degrees per option
            var degreesPerOption = 360.0f / radialMenuOptions.Length;
            
            // Calculate the distance from the background to the icon
            var distanceToIcon = Vector3.Distance(radialMenuOptionUIPrefab.icon.transform.position, radialMenuOptionUIPrefab.background.transform.position);
            
            // Create the radial menu option UIs
            radialMenuOptionUIs = new RadialMenuOptionUI[radialMenuOptions.Length];
            
            // Create the options
            for (var i = 0; i < radialMenuOptions.Length; i++)
            {
                // Instantiate the radial menu option UI prefab
                radialMenuOptionUIs[i] = Instantiate(radialMenuOptionUIPrefab, radialMenuOptionContainer ?? transform);
                
                // Set the radial menu option
                radialMenuOptionUIs[i].radialMenuOption = radialMenuOptions[i];
                
                // Set the background fill amount, rotation and color
                radialMenuOptionUIs[i].background.fillAmount = (1.0f / radialMenuOptions.Length) - (GapBetweenOptions / 360.0f);
                radialMenuOptionUIs[i].background.transform.localRotation = Quaternion.Euler(
                    0, 0, degreesPerOption / 2.0f + GapBetweenOptions / 2.0f + i * degreesPerOption);
                var optionColor = radialMenuOptionUIs[i].background.color;
                radialMenuOptionUIs[i].background.color = new Color(optionColor.r, optionColor.g, optionColor.b, 0.5f);
                
                // Set the icon and position
                radialMenuOptionUIs[i].icon.sprite = radialMenuOptions[i].icon;
                radialMenuOptionUIs[i].icon.enabled = radialMenuOptions[i].icon != null;
                var directionVector = Quaternion.AngleAxis(degreesPerOption * i, Vector3.forward) * Vector3.up;
                var movementVector = directionVector * distanceToIcon;
                radialMenuOptionUIs[i].icon.transform.localPosition =
                    radialMenuOptionUIs[i].background.transform.localPosition + movementVector;
            }
        }

        /// <summary>
        /// Method <c>Open</c> opens the radial menu.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        public void Open(Transform targetObject = null)
        {
            SpawnMenuOptions(ref innerOptions, out _innerOptionUIs, innerOptionPrefab, innerOptionsContainer);
            target = targetObject;
            gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Method <c>Close</c> closes the radial menu.
        /// </summary>
        public void Close()
        {
            if (target != null && targetItem != null)
                target.GetComponent<Interactable>().Interact(targetItem);
            CleanOptions(ref innerOptions, ref _innerOptionUIs); 
            CleanOptions(ref _outerOptions, ref _outerOptionUIs, 8);
            target = null;
            targetItem = null;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Method <c>CleanOptions</c> cleans the options.
        /// </summary>
        /// <param name="radialMenuOptions">The radial menu options.</param>
        /// <param name="radialMenuOptionUIs">The radial menu option UIs.</param>
        /// <param name="length">The length.</param>
        private void CleanOptions(ref RadialMenuOption[] radialMenuOptions,
            ref RadialMenuOptionUI[] radialMenuOptionUIs, int length = 0)
        {
            foreach (var radialMenuOptionUI in radialMenuOptionUIs)
            {
                if (radialMenuOptionUI == null)
                    continue;
                Destroy(radialMenuOptionUI.gameObject);
            }
            if (length > 0)
                radialMenuOptions = new RadialMenuOption[length];
            radialMenuOptionUIs = new RadialMenuOptionUI[radialMenuOptions.Length];
        }
    }
}
