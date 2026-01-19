# Contributing to AV Waypoints

Thank you for your interest in contributing! This document provides guidelines for contributing to the project.

## How to Contribute

### Reporting Bugs

- Use the GitHub issue tracker
- Describe the bug in detail
- Include Unity version and reproduction steps
- Attach screenshots or GIFs if applicable

### Feature Requests

- Open a GitHub issue with the "enhancement" label
- Clearly describe the feature and its use case
- Explain why it would be valuable

### Pull Requests

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes following our coding standards
4. Test thoroughly in Unity
5. Commit with clear messages (`git commit -m 'Add amazing feature'`)
6. Push to your branch (`git push origin feature/amazing-feature`)
7. Open a Pull Request

## Coding Standards

- Follow C# naming conventions (PascalCase for public, camelCase for private)
- Add XML documentation comments for all public APIs
- Keep methods focused and under 50 lines when possible
- Use `#if UNITY_EDITOR` for all editor-only code
- Include null checks and validation
- Write defensive code

## Testing

- Test in Unity 2021.3 LTS minimum
- Verify both Vector3 and float3 compatibility
- Test with arrays and lists
- Check Scene view and Inspector behavior
- Ensure no console errors or warnings

## Code Review Process

All pull requests require:
- Passing CI checks (if configured)
- Code review approval from maintainers
- No merge conflicts
- Clear description of changes

## Questions?

Open a GitHub discussion or issue for any questions!
