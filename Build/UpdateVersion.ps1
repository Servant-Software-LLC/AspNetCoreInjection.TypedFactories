param
  (
    [Parameter(Position=0, Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string]$buildNumber
  )

if([string]::IsNullOrWhiteSpace($buildNumber)){
    throw "Please specify the version build number to be used"
}



$file = Join-Path -Path $PSScriptRoot -ChildPath "..\AspNetCoreInjection.TypedFactories\AspNetCoreInjection.TypedFactories.csproj"

$xml = [xml](get-content ($file))

$versionNode = $xml.Project.PropertyGroup.Version
if ($versionNode -eq $null) {
    # create version node if it doesn't exist
    $versionNode = $xml.CreateElement("Version")
    $xml.Project.PropertyGroup.AppendChild($versionNode)
    Write-Host "AssemblyVersion XML tag added to $($csproj)"

    $version = "1.0.0"
}
else
{
    $version = $xml.Project.PropertyGroup.Version
}

$index = $version.LastIndexOf('.')
if ($index -eq -1) {
    throw "$version isn't in the expected build format. $version = $($version))"
}

$version = $version.Substring(0, $index + 1) + $buildNumber

Write-Host "Stamping $($csproj) with version number $($version)"

$xml.Project.PropertyGroup.Version = $version


$xml.Save($file)