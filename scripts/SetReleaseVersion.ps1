if($env:APPVEYOR_REPO_TAG_NAME -match "^release-") {
  $version = New-Object version($env:APPVEYOR_REPO_TAG_NAME -replace "release-","")
  $env:build = $version.Build
  $env:version = $version.ToString()
}