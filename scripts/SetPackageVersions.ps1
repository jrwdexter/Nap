$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'

@('Nap', 'Nap.Configuration', 'Nap.Html') | %{
  $version = [System.Reflection.Assembly]::LoadFile("$root\$_\bin\Release\$_.dll").GetName().Version
  $versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

  Write-Host "Setting .nuspec for $_ version tag to $versionStr"
  $content = (Get-Content $root\$_\$_.nuspec) 
  $content = $content -replace '\$version\$',$versionStr
  $content | Out-File $root\$_\$_.nuspec
}