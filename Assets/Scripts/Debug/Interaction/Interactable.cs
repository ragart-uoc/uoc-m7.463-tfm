using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TFM.Debug.Interaction
{
    /// <summary>
    /// Class <c>Interactable</c> contains the logic for making an object interactable.
    /// </summary>
    public class Interactable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        /// <value>Property <c>_meshRenderer</c> represents the mesh renderer.</value>
        private MeshRenderer _meshRenderer;
        
        /// <value>Property <c>_meshOriginalMaterials</c> represents the original materials of the mesh.</value>
        private readonly List<Material> _meshOriginalMaterials = new List<Material>();
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            foreach (var material in _meshRenderer.materials)
                _meshOriginalMaterials.Add(material);
        }
        
        /// <summary>
        /// Method <c>OnPointerEnter</c> is called when the pointer enters the object.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            GameManager.Instance.SetStatusBarText(gameObject.name);
            var materials = _meshRenderer.materials;
            for (var i = 0; i < materials.Length; i++)
                materials[i] = GameManager.Instance.highlightMaterial;
            _meshRenderer.materials = materials;
        }
        
        /// <summary>
        /// Method <c>OnPointerExit</c> is called when the pointer exits the object.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            GameManager.Instance.SetStatusBarText("");
            var materials = _meshRenderer.materials;
            for (var i = 0; i < materials.Length; i++)
                materials[i] = _meshOriginalMaterials[i];
            _meshRenderer.materials = materials;
        }
        
        /// <summary>
        /// Method <c>OnPointerDown</c> is called when the pointer is down on the object.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    UnityEngine.Debug.Log("Pointer down left: " + gameObject.name);
                    break;
                case PointerEventData.InputButton.Right:
                    UnityEngine.Debug.Log("Pointer down right: " + gameObject.name);
                    break;
                case PointerEventData.InputButton.Middle:
                    UnityEngine.Debug.Log("Pointer down middle: " + gameObject.name);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
