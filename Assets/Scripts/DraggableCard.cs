using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    /// <summary>
    /// Core component for making UI elements draggable within a layout group.
    /// Handles drag input, position updates, and automatic swapping with nearby elements.
    /// </summary>
    public class DraggableCard : MonoBehaviour
    {
        /// <summary>
        /// Reference to the parent panel that contains this card.
        /// </summary>
        public DraggableCardPanelBase CardPanelBase { get; private set; }

        [Tooltip("Distance threshold in pixels for triggering element swap")]
        [SerializeField] private float swapDistanceThreshold = 10f;

        [Tooltip("Canvas sorting order applied during drag to render above other elements")]
        [SerializeField] private int dragSortingOrder = 2;

        private RectTransform _rectTransform;
        private Canvas _canvas;

        private int _defaultSortingOrder;

        private bool _isAlreadyHaveCanvas;
        private bool _isLayoutVertical;
        private bool _isDragStarted;

        private Vector3 _currentPosition;

        private DraggableCardEventArgs _args;

        private void Awake()
        {
            CardPanelBase = GetComponentInParent<DraggableCardPanelBase>();

            if (CardPanelBase == null)
            {
                Debug.LogError($"[DraggableCard] No DraggableCardPanelBase found in parent hierarchy for {gameObject.name}. " +
                               "Please ensure this card is a child of a GameObject with DraggableCardPanelBase component.");
                enabled = false;
                return;
            }

            _rectTransform = GetComponent<RectTransform>();
            _isAlreadyHaveCanvas = TryGetComponent(out _canvas);
        }

        private void Start()
        {
            if (CardPanelBase == null) return;

            _isLayoutVertical = CardPanelBase.LayoutGroup is VerticalLayoutGroup;

            _args = new DraggableCardEventArgs
            {
                StartIndex = _rectTransform.GetSiblingIndex(),
                EndIndex = -1,
                Card = this
            };
        }

        /// <summary>
        /// Initiates the drag operation. Creates a temporary canvas for proper sorting.
        /// </summary>
        public void StartDragging()
        {
            if (!gameObject.TryGetComponent(out _canvas))
            {
                _canvas = gameObject.AddComponent<Canvas>();
            }

            _canvas.overrideSorting = true;
            _canvas.sortingOrder = dragSortingOrder;

            _defaultSortingOrder = _canvas.sortingOrder;
            _currentPosition = _rectTransform.position;

            CardPanelBase.OnDragStart(_args);
            _isDragStarted = true;
        }

        private void Update()
        {
            if (_isDragStarted) DragCard();
        }

        /// <summary>
        /// Updates the card position based on drag delta from input events.
        /// </summary>
        /// <param name="eventDelta">The drag delta from the pointer event.</param>
        public void Drag(Vector3 eventDelta)
        {
            DragCard(eventDelta);
        }

        private void DragCard(Vector3 delta = default)
        {
            var rectTransformPosition = _rectTransform.position;

            Vector3 position;
            if (delta == default)
            {
                position = Input.mousePosition;
            }
            else
            {
                position = delta + rectTransformPosition;
            }

            _rectTransform.position = _isLayoutVertical
                ? new Vector3(rectTransformPosition.x, position.y, rectTransformPosition.z)
                : new Vector3(position.x, rectTransformPosition.y, rectTransformPosition.z);

            var currentSiblingIndex = _rectTransform.GetSiblingIndex();
            for (var i = 0; i < CardPanelBase.GetChildCount; i++)
            {
                if (i == currentSiblingIndex) continue;

                var otherTransform = CardPanelBase.GetChild(i);

                // Calculate distance using pivot-aware positioning
                var distance = CalculatePivotAwareDistance(_rectTransform, otherTransform as RectTransform);
                if (distance > swapDistanceThreshold) continue;

                var otherOldPosition = otherTransform.position;
                var rectPosition = _rectTransform.position;

                if (_isLayoutVertical)
                {
                    otherTransform.position = new Vector3(otherOldPosition.x, _currentPosition.y, otherOldPosition.z);
                    rectPosition = new Vector3(rectPosition.x, otherOldPosition.y, rectPosition.z);
                }
                else
                {
                    otherTransform.position = new Vector3(_currentPosition.x, otherOldPosition.y, otherOldPosition.z);
                    rectPosition = new Vector3(otherOldPosition.x, rectPosition.y, rectPosition.z);
                }

                _rectTransform.position = rectPosition;
                _currentPosition = rectPosition;

                _rectTransform.SetSiblingIndex(otherTransform.GetSiblingIndex());
            }

            _args.EndIndex = currentSiblingIndex;
            CardPanelBase.OnDrag(_args);
        }

        /// <summary>
        /// Calculates the distance between two RectTransforms accounting for different pivot points.
        /// Uses the center of each rect for accurate distance calculation.
        /// </summary>
        /// <param name="rect1">First RectTransform.</param>
        /// <param name="rect2">Second RectTransform.</param>
        /// <returns>Distance between the centers of the two rects.</returns>
        private float CalculatePivotAwareDistance(RectTransform rect1, RectTransform rect2)
        {
            if (rect2 == null) return float.MaxValue;

            // Get world center positions accounting for pivot differences
            var center1 = GetWorldCenter(rect1);
            var center2 = GetWorldCenter(rect2);

            return Vector3.Distance(center1, center2);
        }

        /// <summary>
        /// Gets the world center position of a RectTransform regardless of its pivot setting.
        /// </summary>
        /// <param name="rectTransform">The RectTransform to get the center of.</param>
        /// <returns>World position of the rect's center.</returns>
        private Vector3 GetWorldCenter(RectTransform rectTransform)
        {
            var rect = rectTransform.rect;
            var pivot = rectTransform.pivot;

            // Calculate offset from current position to center based on pivot
            var offsetX = (0.5f - pivot.x) * rect.width;
            var offsetY = (0.5f - pivot.y) * rect.height;

            var localOffset = new Vector3(offsetX, offsetY, 0);
            return rectTransform.position + rectTransform.TransformVector(localOffset);
        }

        /// <summary>
        /// Ends the drag operation and restores the canvas state.
        /// </summary>
        public void OnDragEnd()
        {
            if (_isAlreadyHaveCanvas)
            {
                _canvas.sortingOrder = _defaultSortingOrder;
            }
            else
            {
                Destroy(_canvas);
            }

            if(_isDragStarted) _rectTransform.position = _currentPosition;
            _args.StartIndex = _rectTransform.GetSiblingIndex();

            CardPanelBase.OnDragEnd(_args);
            _isDragStarted = false;
        }
    }
}
