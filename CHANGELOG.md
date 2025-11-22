# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Comprehensive README with setup instructions, API reference, and examples
- MIT License file
- Contributing guidelines (CONTRIBUTING.md)
- This changelog file
- Unit tests for core functionality
- XML documentation comments for all public APIs
- Editor tooltips for all serialized fields
- Configurable `swapDistanceThreshold` field for element swap distance
- Configurable `dragSortingOrder` field for canvas sorting during drag
- Pivot-aware distance calculation for accurate element swapping
- Error handling and logging with consistent `[ClassName]` prefixes
- Validation warnings for missing component references

### Changed
- Refactored `ScrollableCardPanel` to reduce code duplication with unified `PerformScroll` method
- Moved `RandomColor` class into `DefaultNamespace` for consistency
- Improved null safety throughout all components

### Fixed
- Elements with non-centered pivots now swap correctly (pivot-aware distance calculation)

### Removed
- Unused `_totalSiblingCount` field from `DraggableCard`

## [1.0.0] - 2024-01-01

### Added
- Initial release
- `DraggableCard` component for making UI elements draggable
- `DraggableCardHandle` component for input handling
- `DraggableCardPanelBase` base class for drag containers
- `ScrollableCardPanel` with auto-scroll support
- `HandlePointerEventHandler` for MonoBehaviour-based input
- Support for vertical and horizontal layout groups
- Automatic element swapping during drag
- Event system for drag start, drag, and drag end
- Sample scene demonstrating functionality
- Card prefab for quick setup
