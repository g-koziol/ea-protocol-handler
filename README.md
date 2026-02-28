# EA Protocol Handler

The EA Protocol Handler is a program, targeting the Windows operating system, that defines and handles an `ea://` protocol to link from web pages to specific artifacts in Enterprise Architect repositories.

## Overview

This project enables direct linking to Enterprise Architect (EA) artifacts through a custom protocol handler. By using `ea://` URLs, users can create clickable links in web pages, documents, and other applications that open specific elements within Enterprise Architect.

## Features

- Custom `ea://` protocol registration for Windows
- Deep linking to Enterprise Architect repository artifacts
- Browser extension/plugin capabilities for seamless integration
- Win32 native application for protocol handling

## Technology Stack

- **Language**: C#
- **Platform**: Windows (Win32)
- **UI Framework**: Windows Forms / WPF
- **Protocol**: Custom `ea://` URI scheme
- **License**: Apache License 2.0

## Development Status

- **Current Status**: 4 - Beta
- **Project Start**: October 8, 2009
- **Repository**: SVN with 38 commits
- **Activity Status**: Active

## Source Code

The original source code is hosted on SourceForge:

- **Project Page**: https://sourceforge.net/projects/eaprotocol/
- **SVN Repository**: https://svn.code.sf.net/p/eaprotocol/code/
- **Anonymous SVN**: `svn checkout svn://svn.code.sf.net/p/eaprotocol/code/`

## Original Authors

- Adam Hearn
- Eric Johannsen
- Oliver Fels

## Resources

- **SourceForge Project**: https://sourceforge.net/p/eaprotocol/
- **Bug Reports**: https://sourceforge.net/p/eaprotocol/bugs/
- **Feature Requests**: https://sourceforge.net/p/eaprotocol/feature-requests/
- **Discussions**: https://sourceforge.net/p/eaprotocol/discussion/

## Further Development

This repository is intended for continued development of the EA Protocol Handler. The codebase is being migrated from SourceForge SVN to modern version control for:

- Easier collaboration and contribution
- Integration with modern CI/CD pipelines
- Enhanced development workflows
- Broader platform support considerations

### Building from Source

Prerequisites:
- Windows OS
- .NET Framework (version TBD based on original sources)
- Visual Studio or compatible IDE

### Contributing

Contributions are welcome! Please refer to the project's issue trackers on SourceForge or create issues in this repository for:

- Bug reports
- Feature requests
- Code contributions
- Documentation improvements

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.
