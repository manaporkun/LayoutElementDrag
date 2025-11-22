using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    /// <summary>
    /// Input handler component that routes pointer events to the DraggableCard.
    /// Attach this to the element that should receive drag input (can be the card itself or a child handle).
    /// </summary>
    public class DraggableCardHandle : MonoBehaviour
    {
        [Tooltip("Reference to the DraggableCard to control. If not assigned, will search in parent hierarchy.")]
        [SerializeField] private DraggableCard draggableCard;

        [Tooltip("If true, ends the drag operation when this component is disabled")]
        [SerializeField] private bool endDragOnDisable;

        private void Awake()
        {
            if (draggableCard == null)
            {
                draggableCard = GetComponentInParent<DraggableCard>();
            }

            if (draggableCard == null)
            {
                Debug.LogError($"[DraggableCardHandle] No DraggableCard found for {gameObject.name}. " +
                              "Please assign one in the inspector or ensure this object is a child of a DraggableCard.");
                enabled = false;
            }
        }

        private void Start()
        {
            if (draggableCard == null || draggableCard.CardPanelBase == null) return;

            var eventTrigger = draggableCard.CardPanelBase.eventTrigger;
            if (eventTrigger == null)
            {
                gameObject.AddComponent<HandlePointerEventHandler>();
            }
            else
            {
                SetupEventTrigger(eventTrigger);
            }
        }

        private void SetupEventTrigger(EventTrigger eventTrigger)
        {
            var downEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown,
            };
            downEntry.callback.AddListener(OnPointerDown);
            eventTrigger.triggers.Add(downEntry);

            var dragEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.Drag,
            };
            dragEntry.callback.AddListener(OnDrag);
            eventTrigger.triggers.Add(dragEntry);

            var upEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp,
            };
            upEntry.callback.AddListener(OnPointerUp);
            eventTrigger.triggers.Add(upEntry);
        }

        private void OnDisable()
        {
            if (endDragOnDisable && draggableCard != null)
            {
                draggableCard.OnDragEnd();
            }
        }

        /// <summary>
        /// Called when the pointer is pressed down on this element.
        /// </summary>
        /// <param name="arg0">Event data from the pointer event.</param>
        public void OnPointerDown(BaseEventData arg0)
        {
            if (draggableCard != null)
            {
                draggableCard.StartDragging();
            }
        }

        /// <summary>
        /// Called continuously while the pointer is dragging this element.
        /// </summary>
        /// <param name="eventData">Event data containing drag delta.</param>
        public void OnDrag(BaseEventData eventData)
        {
            if (draggableCard != null && eventData is PointerEventData pointerEvent)
            {
                draggableCard.Drag(pointerEvent.delta);
            }
        }

        /// <summary>
        /// Called when the pointer is released after dragging.
        /// </summary>
        /// <param name="eventData">Event data from the pointer event.</param>
        public void OnPointerUp(BaseEventData eventData)
        {
            if (draggableCard != null)
            {
                draggableCard.OnDragEnd();
            }
        }
    }
}
