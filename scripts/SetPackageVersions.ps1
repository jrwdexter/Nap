$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'

@('Nap', 'Nap.Configuration', 'Nap.Html') | %{
  Write-Host "Setting .nuspec for $_ version tag to $($env:APPVEYOR_BUILD_VERSION)"
  $content = (Get-Content $root\$_\$_.nuspec) 
  $content = $content -replace '\$version\$',$env:APPVEYOR_BUILD_VERSION
  $content | Out-File $root\$_\$_.nuspec
}