; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "龙翔物流"
!define PRODUCT_VERSION "1.0"
!define PRODUCT_PUBLISHER "Ellis"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\LongManagerClient.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

; MUI 1.67 compatible ------
!include "MUI.nsh"
!include "WordFunc.nsh"

; 安装VC环境
Section - "InstallVC"
   Push $R0
   ClearErrors
   ReadRegStr $R0 HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{74d0e5db-b326-4dae-a6b2-445b9de1836e}" "BundleVersion" ;这里检测的是 vc_redist.x86 2015
   ; 检测含有vc的注册表信息是否存在
   IfErrors 0 VSRedistInstalled
        MessageBox MB_ICONINFORMATION|MB_OK "检测到当前系统缺少 vc_redist.x86 组件。"
        SetDetailsPrint textonly
        DetailPrint "准备安装 vc_redist.x86 组件"
        SetDetailsPrint listonly
        SetOutPath "$TEMP"
        SetOverwrite on
        File "Framework\vc_redist.x86.exe"
        ExecWait '$TEMP\vc_redist.x86.exe ' $R1
        Delete "$TEMP\vc_redist.x86.exe"   ;若不存在，执行安装
        StrCpy $R0 "-1"  ;MessageBox MB_OK  "安装完毕"

    VSRedistInstalled: ;MessageBox MB_OK  "已安装"
        pop $R0
SectionEnd


Function GetNetFrameworkVersion ;获取.Net Framework版本支持
    Push $1
    Push $0
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full""Install"
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full""Version"
    StrCmp $0 1 KnowNetFrameworkVersion +1
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5""Install"
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5""Version"
    StrCmp $0 1 KnowNetFrameworkVersion +1
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0\Setup" "InstallSuccess"
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0\Setup" "Version"
    StrCmp $0 1 KnowNetFrameworkVersion +1
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727" "Install"
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727" "Version"
    StrCmp $1 "" +1 +2
    StrCpy $1 "2.0.50727.832"
    StrCmp $0 1 KnowNetFrameworkVersion +1
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v1.1.4322" "Install"
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v1.1.4322" "Version"
    StrCmp $1 "" +1 +2
    StrCpy $1 "1.1.4322.573"
    StrCmp $0 1 KnowNetFrameworkVersion +1
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\.NETFramework\policy\v1.0""Install"
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\.NETFramework\policy\v1.0""Version"
    StrCmp $1 "" +1 +2
    StrCpy $1 "1.0.3705.0"
    StrCmp $0 1 KnowNetFrameworkVersion +1
    StrCpy $1 "not .NetFramework"
    KnowNetFrameworkVersion:
    Pop $0
    Exch $1
FunctionEnd

Section - "比较版本号"
  DetailPrint "正在检测安装环境..."
    Call GetNetFrameworkVersion
    Pop $R1
    ${VersionCompare} "4.6.01590" "$R1" $R2
    ${If} $R2 == 0
        DetailPrint "当前版本($R1)，无需安装组件"
    ${ElseIf} $R2 == 1
        DetailPrint "当前组件版本($R1)过低,需要安装(4.6.2)版本的组件"
    ${ElseIf} $R2 == 2
        DetailPrint "当前版本($R1)，无需安装组件"
    ${EndIf}
SectionEnd

Section -.NET
 Call GetNetFrameworkVersion
 Pop $R1
 ${VersionCompare} "4.6.01590" $R1 $R2
  ${If} $R2 == 1
   MessageBox MB_ICONINFORMATION|MB_OK "检测到当前系统缺少微软.NetFramework 4.6.2组件。"
   SetDetailsPrint textonly
   DetailPrint "准备安装.NetFramework 4.6.2组件"
   SetDetailsPrint listonly
   SetOutPath "$TEMP"
   SetOverwrite on
   File "Framework\NDP462-DevPack-KB3151934-ENU.exe"
   MessageBox MB_ICONINFORMATION|MB_OK $R1
   ExecWait '$TEMP\NDP462-DevPack-KB3151934-ENU.exe ' $R1
   Delete "$TEMP\NDP462-DevPack-KB3151934-ENU.exe"
  ${EndIf}
SectionEnd

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Welcome page
!insertmacro MUI_PAGE_WELCOME
; License page
!insertmacro MUI_PAGE_LICENSE "SoftLicence.txt"
; Directory page
!insertmacro MUI_PAGE_DIRECTORY
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!define MUI_FINISHPAGE_RUN "$INSTDIR\LongManagerClient.exe"
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "SimpChinese"

; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Setup.exe"
InstallDir "$PROGRAMFILES\龙翔物流"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite try
  File "..\src\LongManagerClient\bin\x86\Debug\cef.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\CefSharp.BrowserSubprocess.Core.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\CefSharp.BrowserSubprocess.exe"
  File "..\src\LongManagerClient\bin\x86\Debug\CefSharp.Core.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\CefSharp.Core.xml"
  File "..\src\LongManagerClient\bin\x86\Debug\CefSharp.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\CefSharp.Wpf.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\CefSharp.Wpf.XML"
  File "..\src\LongManagerClient\bin\x86\Debug\CefSharp.XML"
  File "..\src\LongManagerClient\bin\x86\Debug\cef_100_percent.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\cef_200_percent.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\cef_extensions.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\chrome_elf.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\d3dcompiler_47.dll"
  SetOutPath "$INSTDIR\DB"
  File "..\src\LongManagerClient\bin\x86\Debug\DB\LongClient.db"
  SetOutPath "$INSTDIR"
  File "..\src\LongManagerClient\bin\x86\Debug\devtools_resources.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\e_sqlite3.dll"
  SetOutPath "$INSTDIR\Htmls\Content\js"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\js\echarts.js"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\js\jquery-3.3.1.min.js"
  SetOutPath "$INSTDIR\Htmls\Content\js\layer"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\js\layer\layer.js"
  SetOutPath "$INSTDIR\Htmls\Content\js\layer\theme\default"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\js\layer\theme\default\icon-ext.png"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\js\layer\theme\default\icon.png"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\js\layer\theme\default\layer.css"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\js\layer\theme\default\loading-0.gif"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\js\layer\theme\default\loading-1.gif"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\js\layer\theme\default\loading-2.gif"
  SetOutPath "$INSTDIR\Htmls\Content\img"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\img\1.jpg"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\img\2.jpg"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\img\3.jpg"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\img\4.jpg"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\img\5.jpg"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\img\6.jpg"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\Content\img\7.jpg"
  SetOutPath "$INSTDIR\Htmls\pages"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\pages\welcome.html"
  File "..\src\LongManagerClient\bin\x86\Debug\Htmls\pages\index.html"
  SetOutPath "$INSTDIR"
  File "..\src\LongManagerClient\bin\x86\Debug\icudtl.dat"
  SetOutPath "$INSTDIR\Images"
  File "..\src\LongManagerClient\bin\x86\Debug\Images\favicon.ico"
  SetOutPath "$INSTDIR"
  File "..\src\LongManagerClient\bin\x86\Debug\libcef.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\libEGL.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\libGLESv2.dll"
  SetOutPath "$INSTDIR\locales"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\am.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\ar.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\bg.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\bn.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\ca.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\cs.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\da.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\de.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\el.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\en-GB.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\en-US.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\es-419.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\es.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\et.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\fa.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\fi.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\fil.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\fr.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\gu.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\he.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\hi.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\hr.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\hu.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\id.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\it.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\ja.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\kn.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\ko.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\lt.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\lv.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\ml.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\mr.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\ms.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\nb.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\nl.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\pl.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\pt-BR.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\pt-PT.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\ro.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\ru.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\sk.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\sl.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\sr.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\sv.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\sw.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\ta.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\te.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\th.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\tr.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\uk.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\vi.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\zh-CN.pak"
  File "..\src\LongManagerClient\bin\x86\Debug\locales\zh-TW.pak"
  SetOutPath "$INSTDIR"
  File "..\src\LongManagerClient\bin\x86\Debug\log4net.dll"
  SetOutPath "$INSTDIR"
  File "..\src\LongManagerClient\bin\x86\Debug\LongManagerClient.Core.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\LongManagerClient.exe"
  CreateDirectory "$SMPROGRAMS\龙翔物流"
  CreateShortCut "$SMPROGRAMS\龙翔物流\龙翔物流.lnk" "$INSTDIR\LongManagerClient.exe"
  CreateShortCut "$DESKTOP\龙翔物流.lnk" "$INSTDIR\LongManagerClient.exe"
  File "..\src\LongManagerClient\bin\x86\Debug\LongManagerClient.exe.config"
  File "..\src\LongManagerClient\bin\x86\Debug\MaterialDesignColors.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\MaterialDesignThemes.Wpf.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Data.Sqlite.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.DotNet.PlatformAbstractions.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.EntityFrameworkCore.Abstractions.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.EntityFrameworkCore.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.EntityFrameworkCore.Relational.Design.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.EntityFrameworkCore.Relational.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.EntityFrameworkCore.Sqlite.Design.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.EntityFrameworkCore.Sqlite.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Extensions.Caching.Abstractions.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Extensions.Caching.Memory.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Extensions.Configuration.Abstractions.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Extensions.Configuration.Binder.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Extensions.Configuration.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Extensions.DependencyInjection.Abstractions.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Extensions.DependencyInjection.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Extensions.DependencyModel.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Extensions.Logging.Abstractions.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Extensions.Logging.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Extensions.Options.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Extensions.Primitives.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Microsoft.Win32.Primitives.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\natives_blob.bin"
  File "..\src\LongManagerClient\bin\x86\Debug\netstandard.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Newtonsoft.Json.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Quartz.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\EPPlus.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\Autofac.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\README.txt"
  File "..\src\LongManagerClient\bin\x86\Debug\Remotion.Linq.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\snapshot_blob.bin"
  File "..\src\LongManagerClient\bin\x86\Debug\SQLitePCLRaw.batteries_green.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\SQLitePCLRaw.batteries_v2.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\SQLitePCLRaw.core.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\SQLitePCLRaw.provider.e_sqlite3.dll"
  SetOutPath "$INSTDIR\swiftshader"
  File "..\src\LongManagerClient\bin\x86\Debug\swiftshader\libEGL.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\swiftshader\libGLESv2.dll"
  SetOutPath "$INSTDIR"
  File "..\src\LongManagerClient\bin\x86\Debug\System.AppContext.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Buffers.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Collections.Concurrent.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Collections.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Collections.Immutable.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Collections.NonGeneric.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Collections.Specialized.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.ComponentModel.Annotations.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.ComponentModel.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.ComponentModel.EventBasedAsync.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.ComponentModel.Primitives.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.ComponentModel.TypeConverter.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Console.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Data.Common.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Diagnostics.Contracts.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Diagnostics.Debug.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Diagnostics.DiagnosticSource.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Diagnostics.FileVersionInfo.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Diagnostics.Process.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Diagnostics.StackTrace.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Diagnostics.TextWriterTraceListener.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Diagnostics.Tools.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Diagnostics.TraceSource.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Diagnostics.Tracing.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Drawing.Primitives.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Dynamic.Runtime.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Globalization.Calendars.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Globalization.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Globalization.Extensions.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Interactive.Async.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.IO.Compression.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.IO.Compression.ZipFile.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.IO.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.IO.FileSystem.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.IO.FileSystem.DriveInfo.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.IO.FileSystem.Primitives.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.IO.FileSystem.Watcher.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.IO.IsolatedStorage.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.IO.MemoryMappedFiles.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.IO.Pipes.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.IO.UnmanagedMemoryStream.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Linq.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Linq.Expressions.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Linq.Parallel.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Linq.Queryable.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Memory.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Net.Http.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Net.NameResolution.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Net.NetworkInformation.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Net.Ping.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Net.Primitives.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Net.Requests.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Net.Security.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Net.Sockets.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Net.WebHeaderCollection.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Net.WebSockets.Client.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Net.WebSockets.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Numerics.Vectors.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.ObjectModel.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Reflection.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Reflection.Extensions.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Reflection.Primitives.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Resources.Reader.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Resources.ResourceManager.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Resources.Writer.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Runtime.CompilerServices.Unsafe.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Runtime.CompilerServices.VisualC.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Runtime.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Runtime.Extensions.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Runtime.Handles.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Runtime.InteropServices.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Runtime.InteropServices.RuntimeInformation.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Runtime.Numerics.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Runtime.Serialization.Formatters.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Runtime.Serialization.Json.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Runtime.Serialization.Primitives.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Runtime.Serialization.Xml.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Security.Claims.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Security.Cryptography.Algorithms.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Security.Cryptography.Csp.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Security.Cryptography.Encoding.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Security.Cryptography.Primitives.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Security.Cryptography.X509Certificates.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Security.Principal.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Security.SecureString.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Text.Encoding.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Text.Encoding.Extensions.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Text.RegularExpressions.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Threading.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Threading.Overlapped.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Threading.Tasks.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Threading.Tasks.Parallel.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Threading.Thread.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Threading.ThreadPool.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Threading.Timer.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.ValueTuple.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Xml.ReaderWriter.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Xml.XDocument.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Xml.XmlDocument.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Xml.XmlSerializer.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Xml.XPath.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\System.Xml.XPath.XDocument.dll"
  File "..\src\LongManagerClient\bin\x86\Debug\v8_context_snapshot.bin"
  SetOutPath "$INSTDIR\x64"
  File "..\src\LongManagerClient\bin\x86\Debug\x64\e_sqlite3.dll"
  SetOutPath "$INSTDIR\x86"
  File "..\src\LongManagerClient\bin\x86\Debug\x86\e_sqlite3.dll"
  SetOutPath "$INSTDIR"
  File "..\src\LongManagerClient\bin\x86\Debug\Zen.Barcode.Core.dll"
SectionEnd

Section -AdditionalIcons
  CreateShortCut "$SMPROGRAMS\龙翔物流\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\CefSharp.BrowserSubprocess.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\CefSharp.BrowserSubprocess.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd


Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从你的计算机移除。"
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "你确实要完全移除 $(^Name) ，其及所有的组件？" IDYES +2
  Abort
FunctionEnd

Section Uninstall
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\Zen.Barcode.Core.dll"
  Delete "$INSTDIR\x86\e_sqlite3.dll"
  Delete "$INSTDIR\x64\e_sqlite3.dll"
  Delete "$INSTDIR\v8_context_snapshot.bin"
  Delete "$INSTDIR\System.Xml.XPath.XDocument.dll"
  Delete "$INSTDIR\System.Xml.XPath.dll"
  Delete "$INSTDIR\System.Xml.XmlSerializer.dll"
  Delete "$INSTDIR\System.Xml.XmlDocument.dll"
  Delete "$INSTDIR\System.Xml.XDocument.dll"
  Delete "$INSTDIR\System.Xml.ReaderWriter.dll"
  Delete "$INSTDIR\System.ValueTuple.dll"
  Delete "$INSTDIR\System.Threading.Timer.dll"
  Delete "$INSTDIR\System.Threading.ThreadPool.dll"
  Delete "$INSTDIR\System.Threading.Thread.dll"
  Delete "$INSTDIR\System.Threading.Tasks.Parallel.dll"
  Delete "$INSTDIR\System.Threading.Tasks.dll"
  Delete "$INSTDIR\System.Threading.Overlapped.dll"
  Delete "$INSTDIR\System.Threading.dll"
  Delete "$INSTDIR\System.Text.RegularExpressions.dll"
  Delete "$INSTDIR\System.Text.Encoding.Extensions.dll"
  Delete "$INSTDIR\System.Text.Encoding.dll"
  Delete "$INSTDIR\System.Security.SecureString.dll"
  Delete "$INSTDIR\System.Security.Principal.dll"
  Delete "$INSTDIR\System.Security.Cryptography.X509Certificates.dll"
  Delete "$INSTDIR\System.Security.Cryptography.Primitives.dll"
  Delete "$INSTDIR\System.Security.Cryptography.Encoding.dll"
  Delete "$INSTDIR\System.Security.Cryptography.Csp.dll"
  Delete "$INSTDIR\System.Security.Cryptography.Algorithms.dll"
  Delete "$INSTDIR\System.Security.Claims.dll"
  Delete "$INSTDIR\System.Runtime.Serialization.Xml.dll"
  Delete "$INSTDIR\System.Runtime.Serialization.Primitives.dll"
  Delete "$INSTDIR\System.Runtime.Serialization.Json.dll"
  Delete "$INSTDIR\System.Runtime.Serialization.Formatters.dll"
  Delete "$INSTDIR\System.Runtime.Numerics.dll"
  Delete "$INSTDIR\System.Runtime.InteropServices.RuntimeInformation.dll"
  Delete "$INSTDIR\System.Runtime.InteropServices.dll"
  Delete "$INSTDIR\System.Runtime.Handles.dll"
  Delete "$INSTDIR\System.Runtime.Extensions.dll"
  Delete "$INSTDIR\System.Runtime.dll"
  Delete "$INSTDIR\System.Runtime.CompilerServices.VisualC.dll"
  Delete "$INSTDIR\System.Runtime.CompilerServices.Unsafe.dll"
  Delete "$INSTDIR\System.Resources.Writer.dll"
  Delete "$INSTDIR\System.Resources.ResourceManager.dll"
  Delete "$INSTDIR\System.Resources.Reader.dll"
  Delete "$INSTDIR\System.Reflection.Primitives.dll"
  Delete "$INSTDIR\System.Reflection.Extensions.dll"
  Delete "$INSTDIR\System.Reflection.dll"
  Delete "$INSTDIR\System.ObjectModel.dll"
  Delete "$INSTDIR\System.Numerics.Vectors.dll"
  Delete "$INSTDIR\System.Net.WebSockets.dll"
  Delete "$INSTDIR\System.Net.WebSockets.Client.dll"
  Delete "$INSTDIR\System.Net.WebHeaderCollection.dll"
  Delete "$INSTDIR\System.Net.Sockets.dll"
  Delete "$INSTDIR\System.Net.Security.dll"
  Delete "$INSTDIR\System.Net.Requests.dll"
  Delete "$INSTDIR\System.Net.Primitives.dll"
  Delete "$INSTDIR\System.Net.Ping.dll"
  Delete "$INSTDIR\System.Net.NetworkInformation.dll"
  Delete "$INSTDIR\System.Net.NameResolution.dll"
  Delete "$INSTDIR\System.Net.Http.dll"
  Delete "$INSTDIR\System.Memory.dll"
  Delete "$INSTDIR\System.Linq.Queryable.dll"
  Delete "$INSTDIR\System.Linq.Parallel.dll"
  Delete "$INSTDIR\System.Linq.Expressions.dll"
  Delete "$INSTDIR\System.Linq.dll"
  Delete "$INSTDIR\System.IO.UnmanagedMemoryStream.dll"
  Delete "$INSTDIR\System.IO.Pipes.dll"
  Delete "$INSTDIR\System.IO.MemoryMappedFiles.dll"
  Delete "$INSTDIR\System.IO.IsolatedStorage.dll"
  Delete "$INSTDIR\System.IO.FileSystem.Watcher.dll"
  Delete "$INSTDIR\System.IO.FileSystem.Primitives.dll"
  Delete "$INSTDIR\System.IO.FileSystem.DriveInfo.dll"
  Delete "$INSTDIR\System.IO.FileSystem.dll"
  Delete "$INSTDIR\System.IO.dll"
  Delete "$INSTDIR\System.IO.Compression.ZipFile.dll"
  Delete "$INSTDIR\System.IO.Compression.dll"
  Delete "$INSTDIR\System.Interactive.Async.dll"
  Delete "$INSTDIR\System.Globalization.Extensions.dll"
  Delete "$INSTDIR\System.Globalization.dll"
  Delete "$INSTDIR\System.Globalization.Calendars.dll"
  Delete "$INSTDIR\System.Dynamic.Runtime.dll"
  Delete "$INSTDIR\System.Drawing.Primitives.dll"
  Delete "$INSTDIR\System.Diagnostics.Tracing.dll"
  Delete "$INSTDIR\System.Diagnostics.TraceSource.dll"
  Delete "$INSTDIR\System.Diagnostics.Tools.dll"
  Delete "$INSTDIR\System.Diagnostics.TextWriterTraceListener.dll"
  Delete "$INSTDIR\System.Diagnostics.StackTrace.dll"
  Delete "$INSTDIR\System.Diagnostics.Process.dll"
  Delete "$INSTDIR\System.Diagnostics.FileVersionInfo.dll"
  Delete "$INSTDIR\System.Diagnostics.DiagnosticSource.dll"
  Delete "$INSTDIR\System.Diagnostics.Debug.dll"
  Delete "$INSTDIR\System.Diagnostics.Contracts.dll"
  Delete "$INSTDIR\System.Data.Common.dll"
  Delete "$INSTDIR\System.Console.dll"
  Delete "$INSTDIR\System.ComponentModel.TypeConverter.dll"
  Delete "$INSTDIR\System.ComponentModel.Primitives.dll"
  Delete "$INSTDIR\System.ComponentModel.EventBasedAsync.dll"
  Delete "$INSTDIR\System.ComponentModel.dll"
  Delete "$INSTDIR\System.ComponentModel.Annotations.dll"
  Delete "$INSTDIR\System.Collections.Specialized.dll"
  Delete "$INSTDIR\System.Collections.NonGeneric.dll"
  Delete "$INSTDIR\System.Collections.Immutable.dll"
  Delete "$INSTDIR\System.Collections.dll"
  Delete "$INSTDIR\System.Collections.Concurrent.dll"
  Delete "$INSTDIR\System.Buffers.dll"
  Delete "$INSTDIR\System.AppContext.dll"
  Delete "$INSTDIR\swiftshader\libGLESv2.dll"
  Delete "$INSTDIR\swiftshader\libEGL.dll"
  Delete "$INSTDIR\SQLitePCLRaw.provider.e_sqlite3.dll"
  Delete "$INSTDIR\SQLitePCLRaw.core.dll"
  Delete "$INSTDIR\SQLitePCLRaw.batteries_v2.dll"
  Delete "$INSTDIR\SQLitePCLRaw.batteries_green.dll"
  Delete "$INSTDIR\snapshot_blob.bin"
  Delete "$INSTDIR\Remotion.Linq.dll"
  Delete "$INSTDIR\README.txt"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\netstandard.dll"
  Delete "$INSTDIR\natives_blob.bin"
  Delete "$INSTDIR\Microsoft.Win32.Primitives.dll"
  Delete "$INSTDIR\Microsoft.Extensions.Primitives.dll"
  Delete "$INSTDIR\Microsoft.Extensions.Options.dll"
  Delete "$INSTDIR\Microsoft.Extensions.Logging.dll"
  Delete "$INSTDIR\Microsoft.Extensions.Logging.Abstractions.dll"
  Delete "$INSTDIR\Microsoft.Extensions.DependencyModel.dll"
  Delete "$INSTDIR\Microsoft.Extensions.DependencyInjection.dll"
  Delete "$INSTDIR\Microsoft.Extensions.DependencyInjection.Abstractions.dll"
  Delete "$INSTDIR\Microsoft.Extensions.Configuration.dll"
  Delete "$INSTDIR\Microsoft.Extensions.Configuration.Binder.dll"
  Delete "$INSTDIR\Microsoft.Extensions.Configuration.Abstractions.dll"
  Delete "$INSTDIR\Microsoft.Extensions.Caching.Memory.dll"
  Delete "$INSTDIR\Microsoft.Extensions.Caching.Abstractions.dll"
  Delete "$INSTDIR\Microsoft.EntityFrameworkCore.Sqlite.dll"
  Delete "$INSTDIR\Microsoft.EntityFrameworkCore.Sqlite.Design.dll"
  Delete "$INSTDIR\Microsoft.EntityFrameworkCore.Relational.dll"
  Delete "$INSTDIR\Microsoft.EntityFrameworkCore.Relational.Design.dll"
  Delete "$INSTDIR\Microsoft.EntityFrameworkCore.dll"
  Delete "$INSTDIR\Microsoft.EntityFrameworkCore.Abstractions.dll"
  Delete "$INSTDIR\Microsoft.DotNet.PlatformAbstractions.dll"
  Delete "$INSTDIR\Microsoft.Data.Sqlite.dll"
  Delete "$INSTDIR\MaterialDesignThemes.Wpf.dll"
  Delete "$INSTDIR\MaterialDesignColors.dll"
  Delete "$INSTDIR\LongManagerClient.exe.config"
  Delete "$INSTDIR\LongManagerClient.exe"
  Delete "$INSTDIR\LongManagerClient.Core.dll"
  Delete "$INSTDIR\log4net.dll"
  Delete "$INSTDIR\locales\zh-TW.pak"
  Delete "$INSTDIR\locales\zh-CN.pak"
  Delete "$INSTDIR\locales\vi.pak"
  Delete "$INSTDIR\locales\uk.pak"
  Delete "$INSTDIR\locales\tr.pak"
  Delete "$INSTDIR\locales\th.pak"
  Delete "$INSTDIR\locales\te.pak"
  Delete "$INSTDIR\locales\ta.pak"
  Delete "$INSTDIR\locales\sw.pak"
  Delete "$INSTDIR\locales\sv.pak"
  Delete "$INSTDIR\locales\sr.pak"
  Delete "$INSTDIR\locales\sl.pak"
  Delete "$INSTDIR\locales\sk.pak"
  Delete "$INSTDIR\locales\ru.pak"
  Delete "$INSTDIR\locales\ro.pak"
  Delete "$INSTDIR\locales\pt-PT.pak"
  Delete "$INSTDIR\locales\pt-BR.pak"
  Delete "$INSTDIR\locales\pl.pak"
  Delete "$INSTDIR\locales\nl.pak"
  Delete "$INSTDIR\locales\nb.pak"
  Delete "$INSTDIR\locales\ms.pak"
  Delete "$INSTDIR\locales\mr.pak"
  Delete "$INSTDIR\locales\ml.pak"
  Delete "$INSTDIR\locales\lv.pak"
  Delete "$INSTDIR\locales\lt.pak"
  Delete "$INSTDIR\locales\ko.pak"
  Delete "$INSTDIR\locales\kn.pak"
  Delete "$INSTDIR\locales\ja.pak"
  Delete "$INSTDIR\locales\it.pak"
  Delete "$INSTDIR\locales\id.pak"
  Delete "$INSTDIR\locales\hu.pak"
  Delete "$INSTDIR\locales\hr.pak"
  Delete "$INSTDIR\locales\hi.pak"
  Delete "$INSTDIR\locales\he.pak"
  Delete "$INSTDIR\locales\gu.pak"
  Delete "$INSTDIR\locales\fr.pak"
  Delete "$INSTDIR\locales\fil.pak"
  Delete "$INSTDIR\locales\fi.pak"
  Delete "$INSTDIR\locales\fa.pak"
  Delete "$INSTDIR\locales\et.pak"
  Delete "$INSTDIR\locales\es.pak"
  Delete "$INSTDIR\locales\es-419.pak"
  Delete "$INSTDIR\locales\en-US.pak"
  Delete "$INSTDIR\locales\en-GB.pak"
  Delete "$INSTDIR\locales\el.pak"
  Delete "$INSTDIR\locales\de.pak"
  Delete "$INSTDIR\locales\da.pak"
  Delete "$INSTDIR\locales\cs.pak"
  Delete "$INSTDIR\locales\ca.pak"
  Delete "$INSTDIR\locales\bn.pak"
  Delete "$INSTDIR\locales\bg.pak"
  Delete "$INSTDIR\locales\ar.pak"
  Delete "$INSTDIR\locales\am.pak"
  Delete "$INSTDIR\libGLESv2.dll"
  Delete "$INSTDIR\libEGL.dll"
  Delete "$INSTDIR\libcef.dll"
  Delete "$INSTDIR\Images\favicon.ico"
  Delete "$INSTDIR\icudtl.dat"
  Delete "$INSTDIR\Htmls\pages\welcome.html"
  Delete "$INSTDIR\Htmls\pages\index.html"
  Delete "$INSTDIR\Htmls\Content\js\layer\theme\default\loading-2.gif"
  Delete "$INSTDIR\Htmls\Content\js\layer\theme\default\loading-1.gif"
  Delete "$INSTDIR\Htmls\Content\js\layer\theme\default\loading-0.gif"
  Delete "$INSTDIR\Htmls\Content\js\layer\theme\default\layer.css"
  Delete "$INSTDIR\Htmls\Content\js\layer\theme\default\icon.png"
  Delete "$INSTDIR\Htmls\Content\js\layer\theme\default\icon-ext.png"
  Delete "$INSTDIR\Htmls\Content\js\layer\layer.js"
  Delete "$INSTDIR\Htmls\Content\js\jquery-3.3.1.min.js"
  Delete "$INSTDIR\Htmls\Content\js\echarts.js"
  Delete "$INSTDIR\Htmls\Content\img\1.jpg"
  Delete "$INSTDIR\Htmls\Content\img\2.jpg"
  Delete "$INSTDIR\Htmls\Content\img\3.jpg"
  Delete "$INSTDIR\Htmls\Content\img\4.jpg"
  Delete "$INSTDIR\Htmls\Content\img\5.jpg"
  Delete "$INSTDIR\Htmls\Content\img\6.jpg"
  Delete "$INSTDIR\Htmls\Content\img\7.jpg"
  Delete "$INSTDIR\e_sqlite3.dll"
  Delete "$INSTDIR\devtools_resources.pak"
  Delete "$INSTDIR\DB\Long.db"
  Delete "$INSTDIR\d3dcompiler_47.dll"
  Delete "$INSTDIR\chrome_elf.dll"
  Delete "$INSTDIR\cef_extensions.pak"
  Delete "$INSTDIR\cef_200_percent.pak"
  Delete "$INSTDIR\cef_100_percent.pak"
  Delete "$INSTDIR\CefSharp.XML"
  Delete "$INSTDIR\CefSharp.Wpf.XML"
  Delete "$INSTDIR\CefSharp.Wpf.dll"
  Delete "$INSTDIR\CefSharp.dll"
  Delete "$INSTDIR\CefSharp.Core.xml"
  Delete "$INSTDIR\CefSharp.Core.dll"
  Delete "$INSTDIR\CefSharp.BrowserSubprocess.exe"
  Delete "$INSTDIR\CefSharp.BrowserSubprocess.Core.dll"
  Delete "$INSTDIR\cef.pak"
  Delete "$INSTDIR\debug.log"
  Delete "$INSTDIR\Quartz.dll"
  Delete "$INSTDIR\EPPlus.dll"
  Delete "$INSTDIR\Autofac.dll"

  Delete "$SMPROGRAMS\龙翔物流\Uninstall.lnk"
  Delete "$DESKTOP\龙翔物流.lnk"
  Delete "$SMPROGRAMS\龙翔物流\龙翔物流.lnk"

  RMDir "$SMPROGRAMS\龙翔物流"
  RMDir "$INSTDIR\x86"
  RMDir "$INSTDIR\x64"
  RMDir "$INSTDIR\swiftshader"
  RMDir /r "$INSTDIR\LogFiles"
  RMDir /r "$INSTDIR\GPUCache"
  RMDIR /r "$INSTDIR\blob_storage"
  RMDir "$INSTDIR\locales"
  RMDir "$INSTDIR\Images"
  RMDir "$INSTDIR\Htmls\pages"
  RMDir "$INSTDIR\Htmls\Content\js\layer\theme\default"
  RMDir "$INSTDIR\Htmls\Content\js\layer\theme"
  RMDir "$INSTDIR\Htmls\Content\js\layer"
  RMDir "$INSTDIR\Htmls\Content\js"
  RMDir "$INSTDIR\Htmls\Content\img"
  RMDir "$INSTDIR\Htmls\Content"
  RMDir "$INSTDIR\Htmls"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd