if($env:APPVEYOR_REPO_TAG_NAME -match "^release-") {
  $version = New-Object version($env:APPVEYOR_REPO_TAG_NAME -replace "release-","")
  $env:APPVEYOR_BUILD_NUMBER = $version.Build
  $env:APPVEYOR_BUILD_VERSION = $version.ToString()
}