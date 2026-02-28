!define PROGRAM_NAME "Canonic ea:// EA.ProtocolHander.AddIn"
!define PROGRAM_BASE_FILENAME "EA.ProtocolHander.AddIn"

Function InstallFiles
	#Non-Shared Files
	
	#MessageBox MB_ICONEXCLAMATION $INSTDIR
	
	file ..\..\bin\debug\EA.ProtocolHandler.AddIn.xml
	
	#Shared Files
	file ..\..\bin\debug\EA.ProtocolHandler.AddIn.dll
	push $INSTDIR\EA.ProtocolHandler.AddIn.dll
	Call AddSharedDLL

	file ..\..\bin\debug\Interop.EA.dll
	push $INSTDIR\Interop.EA.dll
	Call AddSharedDLL
	
	file ..\..\bin\debug\ICSharpCode.SharpZipLib.dll
	push $INSTDIR\ICSharpCode.SharpZipLib.dll
	Call AddSharedDLL

	file ..\..\bin\debug\NLog.dll
	push $INSTDIR\NLog.dll
	Call AddSharedDLL
	
	file ..\..\..\Utility\bin\debug\EA.ProtocolHandler.Utility.dll
	push $INSTDIR\EA.ProtocolHandler.Utility.dll
	Call AddSharedDLL
							
FunctionEnd

Function un.InstallFiles
	
	#Shared Files
	push $INSTDIR\EA.ProtocolHandler.AddIn.dll
	Call un.RemoveSharedDLL
		
	push $INSTDIR\Interop.EA.dll
	Call un.RemoveSharedDLL
	
	push $INSTDIR\ICSharpCode.SharpZipLib.dll
	Call un.RemoveSharedDLL
	
	push $INSTDIR\NLog.dll
	Call un.RemoveSharedDLL
	
	push $INSTDIR\EA.ProtocolHandler.Utility.dll
	Call un.RemoveSharedDLL
	
FunctionEnd

Function PostHook
	WriteRegStr HKCU 'Software\Sparx Systems\EAAddins\EA.ProtocolHandler.AddIn' '' 'EA.ProtocolHandler.AddIn.Main'
  pop $0  
  #MessageBox MB_OK $0	

  push "v2.0"
  Call GetDotNetDir
  pop $0
  SetOutPath $INSTDIR
  #File "EA.ProtocolHandler.AddIn.dll"
  ExecWait '"$0\RegAsm.exe" EA.ProtocolHandler.AddIn.dll /codebase'

 FunctionEnd

Function un.PostHook
	push "v2.0"
	Call un.GetDotNetDir
	pop $0
	SetOutPath $INSTDIR
	#File "EA.ProtocolHandler.AddIn.dll"
	ExecWait '"$0\RegAsm.exe" EA.ProtocolHandler.AddIn.dll /unregister'
	
	DeleteRegKey HKCU 'Software\Sparx Systems\EAAddins\EA.ProtocolHandler.AddIn'
FunctionEnd

# Sync this with AssemblyInfo.cs:
VIProductVersion "0.9.9.0"
VIAddVersionKey "FileDescription" "ea:// Protocol Handler AddIn"
VIAddVersionKey "FileVersion" "0.9.9.0"
VIAddVersionKey "LegalCopyright" "Based on work of the Canonic Corporation, released under Apache Public License 2.0"

!include "..\CanonicStandard.nsh"
