;------------------------------------------------------------------------------
; un.RemoveSharedDLL
;
; Decrements a shared DLLs reference count, and removes if necessary.
; Use by passing one item on the stack (the full path of the DLL).
; Note: for use in the main installer (not the uninstaller), rename the
; function to RemoveSharedDLL.
; 
; Usage:
;   Push $SYSDIR\myDll.dll
;   Call un.RemoveSharedDLL
;

Function un.RemoveSharedDLL
  Exch $9
  Push $0
  ReadRegDword $0 HKLM Software\Microsoft\Windows\CurrentVersion\SharedDLLs $9
  StrCmp $0 "" remove
    IntOp $0 $0 - 1
    IntCmp $0 0 rk rk uk
    rk:
      DeleteRegValue HKLM Software\Microsoft\Windows\CurrentVersion\SharedDLLs $9
    goto Remove
    uk:
      WriteRegDWORD HKLM Software\Microsoft\Windows\CurrentVersion\SharedDLLs $9 $0
    Goto noremove
  remove:
    # Reboot NOT Ok: Delete /REBOOTOK $9
    Delete $9
  noremove:
  Pop $0
  Pop $9
FunctionEnd


;------------------------------------------------------------------------------
; AddSharedDLL
;
; Increments a shared DLLs reference count.
; Use by passing one item on the stack (the full path of the DLL).
;
; Usage: 
;   Push $SYSDIR\myDll.dll
;   Call AddSharedDLL
;

Function AddSharedDLL
  Exch $9
  Push $0
  ReadRegDword $0 HKLM Software\Microsoft\Windows\CurrentVersion\SharedDLLs $9
  IntOp $0 $0 + 1
  WriteRegDWORD HKLM Software\Microsoft\Windows\CurrentVersion\SharedDLLs $9 $0
  Pop $0
  Pop $9
FunctionEnd  


;------------------------------------------------------------------------------
; UpgradeDLL (macro)
;
; Updates a DLL (or executable) based on version resource information.
;
; Input: ${DLL_NAME} = input source file.
;        top of stack = full path on system to install file to.
;
; Output: none (removes full path from stack)
;
; Usage:
;
;  Push "$SYSDIR\atl.dll"
;  !define DLL_NAME atl.dll
;  !insertmacro UpgradeDLL
;

!macro UpgradeDLL
  Exch $0
  Push $1
  Push $2
  Push $3
  Push $4
  ClearErrors
  GetDLLVersionLocal ${DLL_NAME} $1 $2
  GetDLLVersion $0 $3 $4
  IfErrors upgrade_${DLL_NAME}
    IntCmpU $1 $3 noupgrade_${DLL_NAME} noupgrade_${DLL_NAME} upgrade_${DLL_NAME}
    IntCmpU $2 $4 noupgrade_${DLL_NAME} noupgrade_${DLL_NAME}
  upgrade_${DLL_NAME}:
    UnRegDLL $0
    File /oname=$0 ${DLL_NAME}
    RegDLL $0
  noupgrade_${DLL_NAME}:
  Pop $4
  Pop $3
  Pop $2
  Pop $1
  Pop $0
  !undef DLL_NAME
!macroend


