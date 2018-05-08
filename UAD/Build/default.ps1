
Include ".\helpers.ps1"

properties{
	$testMessage = 'Executed Test!'
	$compileMessage = 'Executed Compile!'
	$cleanMessage = 'Executed Clean!'

	$solutionDirectory = (Get-Item $solutionFile).DirectoryName

	$outputDirectory = "$solutionDirectory\.build"
	$temporaryOutputDirtectory = "$outputDirectory\temp"

	$publishedNUnitDirectory = "$temporaryOutputDirtectory\_PublishedNUnitTests"
	$publishedApplicationDirectory = "$temporaryOutputDirtectory\_PublishedApplications"
	$publishedWebApplicationDirectory = "$temporaryOutputDirtectory\_PublishedWebsites"

	$testResultsDirectory = "$outputDirectory\TestResults"
	$NUnitTestResultsDirectory = "$testResultsDirectory\TestResults"

	$buildConfiguration = "Release"
	$buildPlatform = "Any CPU"

	$packagesPath = "$solutionDirectory\packages"
	$applicationsOutputDirectory = "$publishedApplicationDirectory\Applications"

	$NUnitExe = (Find-PackagePath $packagesPath "NUnit.ConsoleRunner.*") + "\tools\nunit3-console.exe"
	$7ZipExe = (Find-PackagePath $packagesPath "7ZipCLI.*") + "\tools\7za.exe"
}

task default -depends Test

task Init -description "Initializes build by removing previous artificats and creating output directories" -requiredVariables outputDirectory, temporaryOutputDirtectory{

	Assert -conditionToCheck ("Debug", "Release" -contains $buildConfiguration)`
	-failureMessage "Inavlid build configuration '$buildConfiguration'. Valid values are Debug and Release"

	Assert -conditionToCheck ("x86", "x64", "Any CPU" -contains $buildPlatform)`
	-failureMessage "Inavlid build configuration '$buildPlatform'. Valid values are Debug and Release"
	
	
	Assert (Test-Path "$NUnitExe")"NUnit console could not be found"
	
	Assert (Test-Path "$7ZipExe")"7zip exe could not be found"

	#Remove previous builds	

	if(Test-Path $temporaryOutputDirtectory){
		Write-Host "Removing output directory locatated at $temporaryOutputDirtectory"
		Remove-Item $temporaryOutputDirtectory -Force -Recurse
	}
	
	if(Test-Path $outputDirectory){
		Write-Host "Removing output directory locatated at $outputDirectory"
		Remove-Item $outputDirectory -Force -Recurse
	}


	Write-Host "Creating output directory located at ..\..build"
	New-Item $outputDirectory -ItemType Directory | Out-Null

	Write-Host "Creating temporary directory located at $temporaryOutputDirtectory"
	New-Item $temporaryOutputDirtectory -ItemType Directory | Out-Null

}


task Clean -description "Remove temporary files"  {

	Write-Host $cleanMessage
}


task Compile -depends Init -description "Compile Solution" -requiredVariables solutionFile, buildConfiguration, buildPlatform, temporaryOutputDirtectory {

	Write-Host "Building Solution $solutionFile $buildConfiguration $buildPlatform $temporaryOutputDirtectory"
	
	 Exec{ 
		 msbuild $solutionFile "/p:Configuration=$buildConfiguration;Platform=$buildPlatform;OutDir=$temporaryOutputDirtectory"
	 }
}

task TestNUnit `
	-depends Compile `
	 -description "Run NUnit tests" `
	 -precondition { return Test-Path $publishedNUnitDirectory } -requiredVariables publishedNUnitDirectory , NUnitTestResultsDirectory {

	$testAssemblies = Prepare-Tests -testRunnerName "NUnit" -publishedTestsDirectory $publishedNUnitDirectory -testResultsDirectory $NUnitTestResultsDirectory

	Exec { & $NUnitExe $testAssemblies --result="$NUnitTestResultsDirectory\\NUnit.xml" --framework="net-4.5" --trace="Error" }
	
}

task Test -depends Compile, TestNunit, Clean {
	 Write-Host $testMessage
}

task Package `
	-depends Compile, Test `
	-description "Package Installation" `
	-requiredVariables publishedWebApplicationDirectory,publishedApplicationDirectory,applicationsOutputDirectory{

		$applications = @(Get-ChildItem $publishedWebApplicationDirectory) + @(Get-ChildItem $publishedApplicationDirectory)

		if($applications.Length -gt 0 -and !(Test-Path $applicationsOutputDirectory))
		{
			New-Item $applicationsOutputDirectory -ItemType Directory | Out-Null
		}

		foreach($application in $applications){

			Write-Host "Packaging $($application.Name) as a zip file"

			$archivePath = "$($applicationsOutputDirectory)\$($application.Name).zip"
			$inputDirectory = "$($application.FullName)\*"

			Exec {&$7ZipExe a -r -mx3 $archivePath $inputDirectory}
		}

	}

