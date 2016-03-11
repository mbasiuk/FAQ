<#
.SYNOPSIS
install faq apllication 
.EXAMPLE
run powershell as administrator.
.\install.ps1 -Rebuild -CreateShortcut
#>

param([string]$OutDir="$PSScriptRoot\Faq.Wpf\bin\Release", 
      [string]$TargetPath="$env:ProgramFiles\Faq",
      [string]$WorkingDirectory="$env:ProgramData\Faq",
      [string]$MsBuildDir="${env:ProgramFiles(x86)}\MSBuild\14.0\Bin",
      [switch]$Rebuild,
      [switch]$CreateShortcut,
      [string]$Shortcut="$env:USERPROFILE\Desktop\Faq.lnk")
       
  if($Rebuild)
  {      
      pushd $PSScriptRoot
      & "$MsBuildDir\Msbuild.exe" /p:Configuration=Release     
      popd
  }

  if(!(test-path "$WorkingDirectory"))
  {
      md "$WorkingDirectory"
  }

  if(Test-Path "$TargetPath")
  {
      ri "$TargetPath" -Recurse -Force
  }
  
  copy "$OutDir" -Destination "$TargetPath" -Recurse
  
  if($CreateShortCut)
  {
      "creating shortcut at $Shortcut"
      $creatorObj = New-Object -ComObject WScript.Shell
      $shortCutObj = $creatorObj.CreateShortcut("$Shortcut");     
      $shortCutObj.TargetPath = "$TargetPath\FaqWpf.exe";
      $shortCutObj.WorkingDirectory = "$WorkingDirectory";
      $shortCutObj.Save();      
  }