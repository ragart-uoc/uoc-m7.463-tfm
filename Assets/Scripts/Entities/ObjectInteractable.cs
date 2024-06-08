using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TFM.Actions;
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
        
        /// <value>Property <c>actionSequence</c> represents the action sequence.</value>
        public EventTriggerActionSequence actionSequence;

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
        
        /// <summary>
        /// Method <c>HandleItemManagerReady</c> handles the item manager ready event.
        /// </summary>
        /// <param name="item"></param>
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
            if (UIManager.Instance.GetStatusBarText() == itemName)
                UIManager.Instance.SetStatusBarText(string.Empty);
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
                    HandleLeftClickDown();
                    break;
                case PointerEventData.InputButton.Right:
                    HandleRightClickDown();
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
                    HandleLeftClickUp();
                    break;
                case PointerEventData.InputButton.Right:
                    HandleRightClickUp();
                    break;
                case PointerEventData.InputButton.Middle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// Method <c>HandleLeftClickDown</c> handles the left click down event.
        /// </summary>
        private void HandleLeftClickDown()
        {
            if (!_isInteractionPossible)
                return;
            // Check if a sequence has been defined and has not been completed
            if (actionSequence.actionSequence != null
                && (actionSequence.triggerEvent == null
                    || EventManager.Instance.GetEventState(actionSequence.triggerEvent))
                && (actionSequence.completionEvent == null
                    || !EventManager.Instance.GetEventState(actionSequence.completionEvent)))
            {
                actionSequence.actionSequence.ExecuteSequence();
            }
            // Show the message
            else
            {
                var message = (pickableItem == null) ? itemDescription : itemUnpickedDescription;
                UIManager.Instance.ShowMessage(message, 3f, itemName);
            }
            // Pick the item
            if (pickableItem == null
                    || ItemManager.Instance.IsItemPickedOrDiscarded(pickableItem))
                return;
            ItemManager.Instance.AddItem(pickableItem);
            StartCoroutine(UIManager.Instance.ShowItemNotice(pickableItem.Icon, "Picked", pickableItem.Title));
            pickableItem = null;
        }
        
        /// <summary>
        /// Method <c>HandleLeftClickUp</c> handles the left click up event.
        /// </summary>
        private void HandleLeftClickUp()
        {
        }
        
        /// <summary>
        /// Method <c>HandleRightClickDown</c> handles the right click down event.
        /// </summary>
        private void HandleRightClickDown()
        {
            if (!_isInteractionPossible)
                return;
            _isKeyPressed = true;
            UIManager.Instance.EnableInteractions(false);
            UIManager.Instance.radialMenu.Open(transform);
            Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f));
        }
        
        /// <summary>
        /// Method <c>HandleRightClickUp</c> handles the right click up event.
        /// </summary>
        private void HandleRightClickUp()
        {
            _isKeyPressed = false;
            UIManager.Instance.SetStatusBarText(string.Empty);
            UIManager.Instance.EnableInteractions();
            UIManager.Instance.radialMenu.Close();
        }

        /// <summary>
        /// Method <c>Highlight</c> highlights the object.
        /// </summary>
        private void Highlight()
        {
            if (!_isInteractionPossible || _isKeyPressed)
                return;
            _outline.enabled = _isPointerOver;
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
            UIManager.Instance.ShowMessage("That doesn't work.");
        }
    }
}
