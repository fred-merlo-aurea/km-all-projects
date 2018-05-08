cls


# '[p]sake' is the same as 'psake' but $Error is not polluted
remove-module [p]sake

$psakemodule = (Get-ChildItem ("..\Packages\psake*\tools\psake.psm1")).FullName | Sort-Object $_ | select -Last 1
Import-Module $psakemodule
Invoke-psake -buildFile .\default.ps1 `
						-taskList Package `
						-framework 4.5.1 `
						-properties @{
						"buildConfiguration" = "Debug"
						"buildPlatform" = "Mixed Platforms" } `
						-parameters @{"solutionFile" = "../ECNSites.sln"}`
						

