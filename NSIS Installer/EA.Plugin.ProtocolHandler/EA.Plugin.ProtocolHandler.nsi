!define PROGRAM_NAME "ea:// Protocol Handler"
!define PROGRAM_BASE_FILENAME "EA.ProtocolHandler"

Function InstallFiles
	#Non-Shared Files
	file ..\..\..\..\ReleaseDLLs\EA.Plugin.ProtocolHandler.exe
	file ..\..\..\..\ReleaseDLLs\EA.Plugin.ProtocolHandler.exe.config
	file ..\..\doc\EAProtocolHandler_Manual.pdf
	
	#Shared Files
	file ..\..\..\..\ReleaseDlls\GlassButton.dll
	push $INSTDIR\GlassButton.dll
	Call AddSharedDLL
				
	file ..\..\..\..\ReleaseDlls\ICSharpCode.SharpZipLib.dll
	push $INSTDIR\ICSharpCode.SharpZipLib.dll
	Call AddSharedDLL
		
	file ..\..\..\..\ReleaseDlls\Interop.EA.dll
	push $INSTDIR\Interop.EA.dll
	Call AddSharedDLL

	file ..\..\..\..\ReleaseDlls\NLog.dll
	push $INSTDIR\NLog.dll
	Call AddSharedDLL
							
FunctionEnd

Function un.InstallFiles
	# Non-Shared Files
	delete $INSTDIR\EA.Plugin.ProtocolHandler.exe
	delete $INSTDIR\EAProtocolHandler_Manual.pdf
	
	#Shared Files
	push $INSTDIR\GlassButton.dll
	Call un.RemoveSharedDLL

	push $INSTDIR\ICSharpCode.SharpZipLib.dll
	Call un.RemoveSharedDLL
		
	push $INSTDIR\Interop.EA.dll
	Call un.RemoveSharedDLL
	
	
FunctionEnd

Function PostHook
	WriteRegStr HKCR 'ea' '' 'URL:ea Protocol Handler'
	WriteRegStr HKCR 'ea' 'URL Protocol' ''
	WriteRegStr HKCR 'ea\DefaultIcon' '' '$INSTDIR\EA.Plugin.ProtocolHandler.exe'
	WriteRegStr HKCR 'ea\shell\open\command' '' '"$INSTDIR\EA.Plugin.ProtocolHandler.exe" %1'
		
  pop $0  
  #MessageBox MB_OK $0	
	
FunctionEnd

Function un.PostHook
	DeleteRegValue HKCR 'ea' '' 
	DeleteRegValue HKCR 'ea' 'URL Protocol' 
	DeleteRegValue HKCR 'ea\DefaultIcon' '' 
	DeleteRegValue HKCR 'ea\shell\open\command' '' 
	
FunctionEnd

# Sync this with AssemblyInfo.cs:
VIProductVersion "0.9.9.0"
VIAddVersionKey "FileDescription" "ea:// Protocol Handler (BETA)"
VIAddVersionKey "FileVersion" "0.9.9.0"
VIAddVersionKey "LegalCopyright" "Based on work of Canonic Corp, released under Apache Public License 2.0"

!include "..\CanonicStandard.nsh"
