# Contributing to LayoutElementDrag

Thank you for your interest in contributing to LayoutElementDrag! This document provides guidelines and instructions for contributing.

## Getting Started

1. **Fork the repository** on GitHub
2. **Clone your fork** locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/LayoutElementDrag.git
   ```
3. **Open the project** in Unity 2021.3 or later

## Development Setup

### Requirements

- Unity 2021.3.19f1 or later
- Git

### Project Structure

```
Assets/
├── Scripts/           # Core library scripts
├── Tests/             # Unit tests
├── Prefabs/           # Sample prefabs
└── Scenes/            # Demo scenes
```

## Making Changes

### Coding Standards

- Follow C# naming conventions:
  - `PascalCase` for public members and types
  - `_camelCase` with underscore prefix for private fields
  - `camelCase` for local variables and parameters
- Add XML documentation comments to all public members
- Add `[Tooltip]` attributes to all `[SerializeField]` fields
- Include error handling with meaningful log messages using the `[ClassName]` prefix

### Example

```csharp
/// <summary>
/// Brief description of what this method does.
/// </summary>
/// <param name="paramName">Description of the parameter.</param>
/// <returns>Description of the return value.</returns>
public ReturnType MethodName(ParamType paramName)
{
    if (paramName == null)
    {
        Debug.LogError("[ClassName] paramName cannot be null.");
        return default;
    }

    // Implementation
}
```

## Submitting Changes

### Pull Request Process

1. **Create a feature branch** from `master`:
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make your changes** following the coding standards above

3. **Test your changes**:
   - Run the sample scene and verify drag functionality works
   - Run unit tests in Unity Test Runner (Window > General > Test Runner)

4. **Commit your changes** with a clear message:
   ```bash
   git commit -m "Add feature: brief description"
   ```

5. **Push to your fork**:
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Create a Pull Request** on GitHub with:
   - Clear description of changes
   - Screenshots/GIFs for UI changes
   - Reference to related issues (if any)

### Commit Message Guidelines

- Use present tense ("Add feature" not "Added feature")
- Use imperative mood ("Move cursor to..." not "Moves cursor to...")
- Limit the first line to 72 characters
- Reference issues and pull requests when relevant

Examples:
- `Add configurable swap distance threshold`
- `Fix pivot calculation for non-centered elements`
- `Refactor ScrollableCardPanel to reduce code duplication`

## Reporting Issues

When reporting issues, please include:

1. **Unity version** you're using
2. **Steps to reproduce** the issue
3. **Expected behavior** vs **actual behavior**
4. **Screenshots or GIFs** if applicable
5. **Console error messages** if any

## Feature Requests

Feature requests are welcome! Please:

1. Check existing issues to avoid duplicates
2. Clearly describe the feature and its use case
3. Explain why this feature would benefit the project

## Code of Conduct

- Be respectful and inclusive
- Provide constructive feedback
- Focus on the code, not the person

## Questions?

If you have questions, feel free to:

- Open an issue with the "question" label
- Check existing issues and documentation

Thank you for contributing!
