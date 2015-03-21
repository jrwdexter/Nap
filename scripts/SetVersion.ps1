param(
  [Parameter(Mandatory=$true)]
  [string]$Version
)

$nuspecFiles = Get-ChildItem -Recurse *.nuspec

$nuspecFiles | % {
  $nuspecContent = [xml](gc $_.FullName)
  $nuspec.package.metadata.version = $Version
  $nuspec.Save($_.FullName)
}
