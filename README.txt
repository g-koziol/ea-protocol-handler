EEEEE   A           //  //  
E     AA AA  ::    //  //
EEE   A   A       //  //
E     AAAAA  ::  //  //
EEEEE A   A     //  //

This project was initiated by Canonic Corp to provide a protocol handler (ea://) to link 
web pages to resources in Enterprise Architect® (EA) repositories.  The original code was
in use in limited beta form, however it has been modified to remove some code that Canonic
did not want to open source.

Enterprise Architect is a registered Trademark of Sparx Systems Ltd.

STATUS: Useful deployment can be made.

===============
Getting Started
===============

The original contributors have placed relatively extensive comments in the code to 
describe key pieces.  That's currently the best starting point.

Look for //TODO statements in the code to figure out where immediate work is needed.

================================
Reason behind current namespaces
================================

The original code, though intended for free distribution, linked to a number of proprietary
Canonic DLL's.  Required code bits have been removed from those DLLs and are provided
under the project's Open Source license.  To date, namespaces have not been changed to
reflect that reorganization.

====================
Project dependencies
====================

Interop.EA.dll
Provided by Sparx Systems as part of the Enterprise Architect product.

ICSharpCode.SharpZipLib.dll
http://www.icsharpcode.net/OpenSource/SharpZipLib/
Used to compress certain information contained in the URL

GlassButton.dll
http://www.codeproject.com/KB/buttons/glassbutton.aspx
Used to provide a visually more appealing version of a WinForms button.

=========
Installer
=========

NSIS Installer code has been added to the source tree but probably needs some testing and
fixing.  See the XML Doc comment in Program.cs to understand installation/registration issues.
One specific problem with the installer is that it references Canonic DLL's whose required 
source code has been merged into this project.

Status (2009-10-15): Fixed

==================
Connection Strings
==================

This code works with both a fully qualified connection string (including password, etc).
The connection string is encrypted.  However, anyone with access to this source code can
easily decrypt the connection string.  A means of providing a site-managed key would reduce
this weakness considerably.  The key could be distributed in a configuration file, compiled
into the executable, or... open to suggestions.  

I recall also providing support for EA's pre-encrypted connection strings, though that would
need to be tested again. 

File-based repositories should also work, though they would need to be referened either by 
an UNC name available to all users, or all users would have to map the same network drive
(very error prone in anything but small organizations).

Status (2009-10-15): Added preconfigured connection strings in the app configuration to ease connection process.

=======
License
=======

/*
   Copyright 2009 Canonic Corp

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */
