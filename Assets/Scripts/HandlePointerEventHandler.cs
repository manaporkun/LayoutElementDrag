using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    /// <summary>
    /// MonoBehaviour-based input handler that implements Unity's pointer event interfaces.
    /// Used as a fallback when EventTrigger is not available.
    /// This component is automatically added by DraggableCardHandle when needed.
    /// </summary>
    public class HandlePointerEventHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private DraggableCardHandle _cardHandle;

        private void Awake()
        {
            _cardHandle = GetComponent<DraggableCardHandle>();

            if (_cardHandle == null)
            {
                Debug.LogError($"[HandlePointerEventHandler] No DraggableCardHandle found on {gameObject.name}. " +
                              "This component should be added automatically by DraggableCardHandle.");
                enabled = false;
            }
        }

        /// <summary>
        /// Called when the pointer is pressed down on this element.
        /// </summary>
        /// <param name="eventData">Pointer event data.</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_cardHandle != null)
            {
                _cardHandle.OnPointerDown(eventData);
            }
        }

        /// <summary>
        /// Called continuously while the pointer is dragging this element.
        /// </summary>
        /// <param name="eventData">Pointer event data containing drag delta.</param>
        public void OnDrag(PointerEventData eventData)
        {
            if (_cardHandle != null)
            {
                _cardHandle.OnDrag(eventData);
            }
        }

        /// <summary>
        /// Called when the pointer is released after dragging.
        /// </summary>
        /// <param name="eventData">Pointer event data.</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (_cardHandle != null)
            {
                _cardHandle.OnPointerUp(eventData);
            }
        }
    }
}
