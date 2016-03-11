<#
.SYNOPSIS
Unistall faq application
#>

param([string]$TargetPath="$env:ProgramFiles\Faq", [string]$WorkingDirectory="$env:ProgramData\Faq", [string]$Shortcut="$env:USERPROFILE\Desktop\Faq.lnk")

ri $TargetPath -Recurse -Force
ri $WorkingDirectory -Recurse -Force
ri $Shortcut -Recurse -Force