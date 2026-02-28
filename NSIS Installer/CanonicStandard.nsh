!include "MUI.nsh"
!include "..\DllUtil.nsh"

Function .onInit
	
; Set the install path

    # Get the EA install path, read the value from the registry into the $0 register
    ReadRegStr $0 HKCU "SOFTWARE\Sparx Systems\EA400\EA" "Install Path"
	StrCmp $0 "" pathNotInReg
    StrCpy $INSTDIR "$0\ProtocolHandler\"
		
	; Uninstall any previous version first
	ReadRegStr $R0 HKLM \
	"Software\Microsoft\Windows\CurrentVersion\Uninstall\${PROGRAM_NAME}" \
	"UninstallString"

	StrCmp $R0 "" doneUninstall

	MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
	"${PROGRAM_NAME} is already installed. $\n$\nClick 'OK' to remove the \
	previous version before upgrading, or 'Cancel' to cancel this upgrade." \
	IDOK uninst
	Abort

;Run the uninstaller
uninst:
	ClearErrors

	ExecWait '"$R0" _?=$INSTDIR' ;Do not copy the uninstaller to a temp file

  
doneUninstall:
	Return

pathNotInReg:
	MessageBox MB_OK "The EA install path could not be located.  Please ensure EA is installed."
	Abort

FunctionEnd

# name the installer
Name "${PROGRAM_NAME}"
BrandingText " "

OutFile "bin\Setup${PROGRAM_BASE_FILENAME}.exe"

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_WELCOME
  #!insertmacro MUI_PAGE_LICENSE "..\License.rtf"
  ;!insertmacro MUI_PAGE_COMPONENTS
  ;!insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  !insertmacro MUI_PAGE_FINISH

  !insertmacro MUI_UNPAGE_WELCOME
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  !insertmacro MUI_UNPAGE_FINISH

;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE "English"

#default section start
section

    MessageBox MB_OK "Installing EA protocol handler addin to $INSTDIR. Please make sure Enterprise Architect is not running. If it is open make sure to close it now. Otherwise installation might not succeed"
    
    StrCpy $OUTDIR $INSTDIR
	
	;Support add/remove programs
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PROGRAM_NAME}" "DisplayName" "${PROGRAM_NAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PROGRAM_NAME}" "UninstallString" "$INSTDIR\Remove${PROGRAM_BASE_FILENAME}.exe"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PROGRAM_NAME}" "NoModify" "1"	
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PROGRAM_NAME}" "NoRepair" "1"	
		
	SetOutPath $INSTDIR
	
	Call InstallFiles

    # define uninstaller name
    WriteUninstaller $INSTDIR\Remove${PROGRAM_BASE_FILENAME}.exe
	
	Call PostHook

    Return
 
# default section end
sectionEnd

# create a section to define what the uninstaller does.
# the section will always be named "Uninstall"
section "Uninstall"
 
    MessageBox MB_OK "Uninstalling EA protocol handler addin from $INSTDIR. Please make sure Enterprise Architect is not running. If it is open make sure to close it now. Otherwise deinstallation might not succeed"

    # Always delete uninstaller first
    delete $INSTDIR\Remove${PROGRAM_BASE_FILENAME}.exe


	
	; remove the Uninstall registry keys
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PROGRAM_NAME}"
 
    # now delete installed files

	Call un.InstallFiles
	
	Call un.PostHook

    Return

sectionEnd