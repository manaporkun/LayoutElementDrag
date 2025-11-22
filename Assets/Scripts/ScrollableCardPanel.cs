using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    /// <summary>
    /// Extended draggable card panel that supports auto-scrolling when dragging near container edges.
    /// Use this component instead of DraggableCardPanelBase when cards are inside a ScrollRect.
    /// </summary>
    public class ScrollableCardPanel : DraggableCardPanelBase
    {
        [Header("Scroll Configuration")]
        [Tooltip("Whether the layout is vertical (true) or horizontal (false)")]
        [SerializeField] private bool isVertical;

        [Tooltip("Base scroll speed multiplier")]
        [SerializeField] private float scrollSpeed = 0.01f;

        [Header("Scroll Components")]
        [Tooltip("Reference to the ScrollRect component")]
        [SerializeField] private ScrollRect scrollRect;

        [Tooltip("RectTransform defining the area that triggers scrolling down/right")]
        [SerializeField] public RectTransform downRectTransform;

        [Tooltip("RectTransform defining the area that triggers scrolling up/left")]
        [SerializeField] public RectTransform upRectTransform;

        [Tooltip("Vertical scrollbar reference (required for vertical layouts)")]
        [SerializeField] private Scrollbar verticalScrollbar;

        [Tooltip("Horizontal scrollbar reference (required for horizontal layouts)")]
        [SerializeField] private Scrollbar horizontalScrollbar;

        private bool _dragStarted;
        private DraggableCard _card;
        private Rect _cardRect;

        private readonly Vector3[] _rectCorners = new Vector3[4];
        private readonly Vector3[] _downRectCornerPos = new Vector3[4];
        private readonly Vector3[] _upRectCornerPos = new Vector3[4];

        private void Start()
        {
            ValidateConfiguration();

            if (scrollRect == null) return;

            scrollRect.content.GetWorldCorners(_rectCorners);

            if (downRectTransform != null)
                downRectTransform.GetLocalCorners(_downRectCornerPos);

            if (upRectTransform != null)
                upRectTransform.GetLocalCorners(_upRectCornerPos);
        }

        private void ValidateConfiguration()
        {
            if (scrollRect == null)
            {
                Debug.LogWarning($"[ScrollableCardPanel] ScrollRect is not assigned on {gameObject.name}. " +
                                "Auto-scrolling will be disabled.");
            }

            if (downRectTransform == null || upRectTransform == null)
            {
                Debug.LogWarning($"[ScrollableCardPanel] Scroll trigger zones (upRectTransform/downRectTransform) " +
                                $"are not fully assigned on {gameObject.name}. Auto-scrolling may not work correctly.");
            }

            if (isVertical && verticalScrollbar == null)
            {
                Debug.LogWarning($"[ScrollableCardPanel] Vertical layout specified but verticalScrollbar is not assigned on {gameObject.name}.");
            }

            if (!isVertical && horizontalScrollbar == null)
            {
                Debug.LogWarning($"[ScrollableCardPanel] Horizontal layout specified but horizontalScrollbar is not assigned on {gameObject.name}.");
            }
        }

        private void Update()
        {
            if (!_dragStarted) return;

            var mousePoint = Input.mousePosition;

            if (downRectTransform != null && RectTransformUtility.RectangleContainsScreenPoint(downRectTransform, mousePoint))
                PerformScroll(isIncreasing: !isVertical, downRectTransform, _downRectCornerPos);

            if (upRectTransform != null && RectTransformUtility.RectangleContainsScreenPoint(upRectTransform, mousePoint))
                PerformScroll(isIncreasing: isVertical, upRectTransform, _upRectCornerPos);
        }

        /// <summary>
        /// Performs scroll operation based on mouse proximity to the trigger zone edge.
        /// </summary>
        /// <param name="isIncreasing">Whether to increase or decrease the normalized scroll position.</param>
        /// <param name="triggerRect">The RectTransform of the scroll trigger zone.</param>
        /// <param name="cornerPositions">Cached corner positions of the trigger zone.</param>
        private void PerformScroll(bool isIncreasing, RectTransform triggerRect, Vector3[] cornerPositions)
        {
            var positionChange = Time.deltaTime * scrollSpeed;
            var localPointerPosition = triggerRect.InverseTransformPoint(Input.mousePosition);

            if (isVertical)
            {
                if (verticalScrollbar == null) return;

                // Calculate speed multiplier based on distance from trigger zone edge
                var edgeY = isIncreasing ? cornerPositions[0].y : cornerPositions[1].y;
                positionChange *= Mathf.Abs(edgeY - localPointerPosition.y);

                var direction = isIncreasing ? 1f : -1f;
                scrollRect.verticalNormalizedPosition = Mathf.Clamp01(
                    scrollRect.verticalNormalizedPosition + (positionChange * direction));
            }
            else
            {
                if (horizontalScrollbar == null) return;

                // Calculate speed multiplier based on distance from trigger zone edge
                var edgeX = isIncreasing ? cornerPositions[1].x : cornerPositions[2].x;
                positionChange *= Mathf.Abs(edgeX - localPointerPosition.x);

                var direction = isIncreasing ? 1f : -1f;
                scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(
                    scrollRect.horizontalNormalizedPosition + (positionChange * direction));
            }
        }

        /// <summary>
        /// Called when a card starts being dragged. Enables scroll monitoring.
        /// </summary>
        public override void OnDragStart(DraggableCardEventArgs args)
        {
            base.OnDragStart(args);
            _dragStarted = true;
        }

        /// <summary>
        /// Called when a card stops being dragged. Disables scroll monitoring and recalculates layout.
        /// </summary>
        public override void OnDragEnd(DraggableCardEventArgs args)
        {
            base.OnDragEnd(args);
            _dragStarted = false;

            // Reset layout group to recalculate positions
            if (LayoutGroup != null)
            {
                LayoutGroup.CalculateLayoutInputVertical();
                LayoutGroup.CalculateLayoutInputHorizontal();
                LayoutGroup.SetLayoutVertical();
                LayoutGroup.SetLayoutHorizontal();
            }
        }
    }
}
