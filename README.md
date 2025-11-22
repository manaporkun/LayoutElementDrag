# LayoutElementDrag

A Unity drag-and-drop system for rearranging elements within vertical and horizontal layout groups. Features smooth dragging, automatic element swapping, and optional auto-scrolling when dragging near container edges.

![Demo](https://github.com/manaporkun/LayoutElementDrag/blob/master/demo.gif)

## Features

- Drag and drop elements within `VerticalLayoutGroup` and `HorizontalLayoutGroup`
- Automatic element swapping when dragging over other elements
- Configurable swap distance threshold
- Auto-scrolling when dragging near scroll container edges
- Event system for tracking drag start, drag, and drag end
- Support for both `EventTrigger` and `MonoBehaviour` input handling
- Works with any pivot configuration

## Requirements

- Unity 2021.3 or later
- Unity UI (uGUI) package

## Installation

### Option 1: Clone the Repository

```bash
git clone https://github.com/manaporkun/LayoutElementDrag.git
```

### Option 2: Copy Scripts

Copy the contents of `Assets/Scripts/` into your Unity project:

- `DraggableCard.cs` - Core dragging logic for individual cards
- `DraggableCardHandle.cs` - Input handler for drag events
- `DraggableCardPanelBase.cs` - Base class for drag-enabled containers
- `ScrollableCardPanel.cs` - Extended container with auto-scroll support
- `HandlePointerEventHandler.cs` - Alternative MonoBehaviour-based input handler

## Quick Start

### Basic Setup

1. **Create a Layout Group**: Add a `VerticalLayoutGroup` or `HorizontalLayoutGroup` to a UI GameObject.

2. **Add the Panel Script**: Attach `DraggableCardPanelBase` (or `ScrollableCardPanel` for scrollable containers) to the layout group GameObject.

3. **Create Draggable Cards**: For each child element you want to be draggable:
   - Add the `DraggableCard` component
   - Add the `DraggableCardHandle` component (can be on the same object or a child handle)

4. **Configure**: Adjust the `swapDistanceThreshold` on `DraggableCard` to control how close elements need to be before swapping.

### With Scrollable Container

1. Set up a `ScrollRect` with your layout group as the content.

2. Use `ScrollableCardPanel` instead of `DraggableCardPanelBase`.

3. Create two RectTransform objects to define scroll trigger zones:
   - `upRectTransform`: Area that triggers upward/leftward scrolling
   - `downRectTransform`: Area that triggers downward/rightward scrolling

4. Assign the `ScrollRect`, scrollbars, and trigger zones in the inspector.

## API Reference

### DraggableCard

The main component for making UI elements draggable.

```csharp
// Public properties
DraggableCardPanelBase CardPanelBase { get; }  // Parent panel reference

// Configurable fields (Inspector)
float swapDistanceThreshold = 10f;  // Distance threshold for element swapping
int dragSortingOrder = 2;           // Canvas sorting order during drag
```

### DraggableCardPanelBase

Base class for containers that hold draggable cards.

```csharp
// Events
event Action<DraggableCardEventArgs> OnDragStartEvent;
event Action<DraggableCardEventArgs> OnDragEvent;
event Action<DraggableCardEventArgs> OnDragEndEvent;

// Properties
int GetChildCount { get; }      // Number of child elements
LayoutGroup LayoutGroup { get; } // Reference to the layout group
```

### DraggableCardEventArgs

Event data passed to drag event handlers.

```csharp
public struct DraggableCardEventArgs
{
    public int StartIndex;       // Original sibling index
    public int EndIndex;         // Current/final sibling index
    public DraggableCard Card;   // Reference to the dragged card
}
```

### ScrollableCardPanel

Extended panel with auto-scroll functionality.

```csharp
// Configurable fields (Inspector)
bool isVertical;                    // Layout orientation
float scrollSpeed;                  // Base scroll speed
ScrollRect scrollRect;              // Reference to ScrollRect
RectTransform upRectTransform;      // Scroll up/left trigger zone
RectTransform downRectTransform;    // Scroll down/right trigger zone
Scrollbar verticalScrollbar;        // Vertical scrollbar reference
Scrollbar horizontalScrollbar;      // Horizontal scrollbar reference
```

## Example Usage

### Subscribing to Drag Events

```csharp
public class CardManager : MonoBehaviour
{
    [SerializeField] private DraggableCardPanelBase cardPanel;

    private void OnEnable()
    {
        cardPanel.OnDragStartEvent += HandleDragStart;
        cardPanel.OnDragEndEvent += HandleDragEnd;
    }

    private void OnDisable()
    {
        cardPanel.OnDragStartEvent -= HandleDragStart;
        cardPanel.OnDragEndEvent -= HandleDragEnd;
    }

    private void HandleDragStart(DraggableCardEventArgs args)
    {
        Debug.Log($"Started dragging card from index {args.StartIndex}");
    }

    private void HandleDragEnd(DraggableCardEventArgs args)
    {
        Debug.Log($"Card moved from {args.StartIndex} to {args.EndIndex}");
    }
}
```

## Project Structure

```
Assets/
├── Scripts/
│   ├── DraggableCard.cs           # Core drag logic
│   ├── DraggableCardHandle.cs     # Input handling
│   ├── DraggableCardPanelBase.cs  # Base container class
│   ├── ScrollableCardPanel.cs     # Scrollable container
│   ├── HandlePointerEventHandler.cs # MonoBehaviour input fallback
│   └── RandomColor.cs             # Demo utility
├── Prefabs/
│   └── Card.prefab                # Sample card prefab
└── Scenes/
    └── SampleScene.unity          # Demo scene
```

## Contributing

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for version history.
