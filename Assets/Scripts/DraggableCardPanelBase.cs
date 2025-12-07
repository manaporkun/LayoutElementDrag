using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace
{
    /// <summary>
    /// Base class for containers that hold draggable cards.
    /// Provides event broadcasting and child element management.
    /// </summary>
    public class DraggableCardPanelBase : MonoBehaviour
    {
        /// <summary>
        /// Event fired when a card starts being dragged.
        /// </summary>
        public event Action<DraggableCardEventArgs> OnDragStartEvent;

        /// <summary>
        /// Event fired continuously while a card is being dragged.
        /// </summary>
        public event Action<DraggableCardEventArgs> OnDragEvent;

        /// <summary>
        /// Event fired when a card stops being dragged.
        /// </summary>
        public event Action<DraggableCardEventArgs> OnDragEndEvent;

        /// <summary>
        /// Gets the number of child elements in this container.
        /// </summary>
        public int GetChildCount => transform.childCount;

        /// <summary>
        /// Gets the LayoutGroup component attached to this container.
        /// </summary>
        public LayoutGroup LayoutGroup { get; private set; }

        [Tooltip("Optional EventTrigger for handling input events. If not assigned, will search for one in the scene.")]
        public EventTrigger eventTrigger;

        /// <summary>
        /// Initializes the LayoutGroup reference.
        /// </summary>
        protected void Awake()
        {
            LayoutGroup = GetComponent<LayoutGroup>();

            if (LayoutGroup == null)
            {
                Debug.LogWarning($"[DraggableCardPanelBase] No LayoutGroup found on {gameObject.name}. " +
                                "Cards may not be positioned correctly.");
            }
        }

        private void Start()
        {
            eventTrigger ??= FindObjectOfType<EventTrigger>();

            if (eventTrigger == null)
            {
                Debug.Log($"[DraggableCardPanelBase] No EventTrigger found. " +
                         "HandlePointerEventHandler will be used as fallback for input handling.");
            }
        }

        /// <summary>
        /// Called when a card starts being dragged. Override to add custom behavior.
        /// </summary>
        /// <param name="args">Event arguments containing drag information.</param>
        public virtual void OnDragStart(DraggableCardEventArgs args) => OnDragStartEvent?.Invoke(args);

        /// <summary>
        /// Called continuously while a card is being dragged. Override to add custom behavior.
        /// </summary>
        /// <param name="args">Event arguments containing drag information.</param>
        public virtual void OnDrag(DraggableCardEventArgs args) => OnDragEvent?.Invoke(args);

        /// <summary>
        /// Called when a card stops being dragged. Override to add custom behavior.
        /// </summary>
        /// <param name="args">Event arguments containing drag information.</param>
        public virtual void OnDragEnd(DraggableCardEventArgs args) => OnDragEndEvent?.Invoke(args);

        /// <summary>
        /// Gets a child transform at the specified index.
        /// </summary>
        /// <param name="index">The sibling index of the child to retrieve.</param>
        /// <returns>The Transform of the child at the specified index.</returns>
        public Transform GetChild(int index)
        {
            if (index < 0 || index >= transform.childCount)
            {
                Debug.LogError($"[DraggableCardPanelBase] Child index {index} is out of range. " +
                              $"Valid range is 0-{transform.childCount - 1}.");
                return null;
            }

            return transform.GetChild(index);
        }
    }

    /// <summary>
    /// Event data passed to drag event handlers.
    /// Contains information about the drag operation including start/end positions.
    /// </summary>
    public struct DraggableCardEventArgs
    {
        /// <summary>
        /// The sibling index where the drag operation started.
        /// </summary>
        public int StartIndex;

        /// <summary>
        /// The current or final sibling index during/after the drag operation.
        /// </summary>
        public int EndIndex;

        /// <summary>
        /// Reference to the card being dragged.
        /// </summary>
        public DraggableCard Card;
    }
}
