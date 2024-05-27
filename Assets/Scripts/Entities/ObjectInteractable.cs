using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TFM.Managers;

namespace TFM.Entities
{
    /// <summary>
    /// Class <c>ObjectInteractable</c> contains the logic for making an object interactable.
    /// </summary>
    public class ObjectInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        /// <value>Property <c>itemName</c> represents the name of the item.</value>
        public string itemName;
        
        /// <value>Property <c>itemDescription</c> represents the description of the item.</value>
        public string itemDescription;
        
        /// <value>Property <c>itemUnpickedDescription</c> represents the unpicked description of the item.</value>
        public string itemUnpickedDescription;

        /// <value>Property <c>pickableItem</c> represents the pickable item.</value>
        public Item pickableItem;
        
        /// <value>Property <c>objectInteracions</c> represents the item interactions.</value>
        public ItemInteraction[] objectInteracions;
        
        /// <value>Property <c>_outline</c> represents the outline of the object.</value>
        private Outline _outline;
        
        /// <value>Property <c>_isInteractionPossible</c> represents if the interaction is possible.</value>
        private bool _isInteractionPossible;
        
        /// <value>Property <c>_isPointerOver</c> represents if the pointer is over the object.</value>
        private bool _isPointerOver;
        
        /// <value>Property <c>_isKeyPressed</c> represents if a key is pressed over the object.</value>
        private bool _isKeyPressed;
        
        /// <value>Property <c>_coroutine</c> represents the coroutine.</value>
        private Coroutine _coroutine;

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            _outline = gameObject.GetComponent<Outline>() ?? gameObject.AddComponent<Outline>();
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            _outline.enabled = false;
        }
        
        /// <summary>
        /// Method <c>OnEnable</c> is called when the behaviour becomes enabled.
        /// </summary>
        private void OnEnable()
        {
            if (ItemManager.Instance == null)
                return;
            ItemManager.Instance.Ready += HandleItemManagerReady;
        }

        /// <summary>
        /// Method <c>OnDisable</c> is called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            if (ItemManager.Instance == null)
                return;
            ItemManager.Instance.Ready -= HandleItemManagerReady;
        }
        
        private void HandleItemManagerReady(Item item)
        {
            LateStart();
        }
        
        /// <summary>
        /// Method <c>LateStart</c> is the coroutine for the late start.
        /// </summary>
        private void LateStart()
        {
            if (ItemManager.Instance.IsItemPickedOrDiscarded(pickableItem))
                pickableItem = null;
        }

        /// <summary>
        /// Method <c>Update</c> is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            _isInteractionPossible = UIManager.Instance.InteractionsEnabled();
            Highlight();
        }
        
        /// <summary>
        /// Method <c>OnPointerEnter</c> is called when the pointer enters the object.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOver = true;
            if (_isInteractionPossible)
                UIManager.Instance.SetStatusBarText(itemName);
        }
        
        /// <summary>
        /// Method <c>OnPointerExit</c> is called when the pointer exits the object.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;
        }
        
        /// <summary>
        /// Method <c>OnPointerDown</c> is called when the pointer is down on the object.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isInteractionPossible)
                return;
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    var message = (pickableItem == null) ? itemDescription : itemUnpickedDescription;
                    StartCoroutine(UIManager.Instance.ShowMessage(message, 3f, itemName));
                    if (pickableItem != null
                        && !ItemManager.Instance.IsItemPickedOrDiscarded(pickableItem))
                    {
                        ItemManager.Instance.AddItem(pickableItem);
                        StartCoroutine(UIManager.Instance.ShowItemNotice(pickableItem.Icon, "Picked", pickableItem.Title));
                        pickableItem = null;
                    }
                    break;
                case PointerEventData.InputButton.Right:
                    _isKeyPressed = true;
                    UIManager.Instance.EnableInteractions(false);
                    UIManager.Instance.radialMenu.Open(transform);
                    break;
                case PointerEventData.InputButton.Middle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// Method <c>OnPointerUp</c> is called when the pointer is up on the object.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    break;
                case PointerEventData.InputButton.Right:
                    _isKeyPressed = false;
                    UIManager.Instance.EnableInteractions();
                    UIManager.Instance.radialMenu.Close();
                    break;
                case PointerEventData.InputButton.Middle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Method <c>Highlight</c> highlights the object.
        /// </summary>
        private void Highlight()
        {
            if (!_isInteractionPossible || _isKeyPressed)
                return;
            _outline.enabled = _isPointerOver;
            if (!_isPointerOver && UIManager.Instance.GetStatusBarText() == itemName)
                UIManager.Instance.SetStatusBarText(string.Empty);
        }

        /// <summary>
        /// Method <c>Interact</c> interacts with the item.
        /// </summary>
        /// <param name="item">The item to interact with.</param>
        public void Interact(Item item)
        {
            foreach (var objectInteracion in objectInteracions)
            {
                if (objectInteracion.item != item)
                    continue;
                objectInteracion.actionSequence.ExecuteSequence();
                if (objectInteracion.discardItem) 
                    ItemManager.Instance.DiscardItem(item);
                return;
            }
            StartCoroutine(UIManager.Instance.ShowMessage("That doesn't work."));
        }
    }
}
