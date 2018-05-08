
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


	$testCoverageDirectory = "$outputDirectory\TestCoverage"
	$testCoverageReportPath = "$testCoverageDirectory\OpenCover.xml"
	$testCoverageFilter = "+[*]* +[SourceMediaPaidPub*]* -[SourceMediaPaidPubTest*]*"
	$testCoverageExcludeAttribute = "*.ExcludeFromCoverage*"
	$testCoverageExcludeByFile = "*\*Designer.cs;*\*.g.cs;*\*.g.i.cs"

	$buildConfiguration = "Release"
	$buildPlatform = "Any CPU"

	$packagesPath = "$solutionDirectory\packages"
	$applicationsOutputDirectory = "$publishedApplicationDirectory\Applications"

	$NUnitExe = (Find-PackagePath $packagesPath "NUnit.ConsoleRunner.*") + "\tools\nunit3-console.exe"
	$7ZipExe = (Find-PackagePath $packagesPath "7ZipCLI.*") + "\tools\7za.exe"	
	$openCoverExe = (Find-PackagePath $packagesPath "OpenCover.*") + "\tools\OpenCover.Console.exe"	
	$reportGeneerator = (Find-PackagePath $packagesPath "ReportGenerator.*") + "\tools\ReportGenerator.exe"	
}

task default -depends Test

task Init -description "Initializes build by removing previous artificats and creating output directories" -requiredVariables outputDirectory, temporaryOutputDirtectory{

	Assert -conditionToCheck ("Debug", "Release", "Prod" -contains $buildConfiguration)`
	-failureMessage "Inavlid build configuration '$buildConfiguration'. Valid values are Debug and Release"

	Assert -conditionToCheck ("x86", "x64", "Any CPU", "Mixed Platforms" -contains $buildPlatform)`
	-failureMessage "Inavlid build configuration '$buildPlatform'. Valid values are Debug and Release"
	
	
	Assert (Test-Path "$NUnitExe")"NUnit console could not be found"
	Assert (Test-Path "$openCoverExe")"OpenCover console could not be found"
	Assert (Test-Path "$reportGeneerator")"ReportGenerator console could not be found"
	
	

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

	$testAssemblies = Prepare-Tests -testRunnerName "NUnit" -publishedTestsDirectory $publishedNUnitDirectory -testResultsDirectory $NUnitTestResultsDirectory -testCoverageDirectory $testCoverageDirectory
	$targetArgs = "$testAssemblies"

	#--result= `"`"$NUnitTestResultsDirectory\NUnit.xml`"`" --trace= `"`"Error`"`"
	 #Exec { & $NUnitExe $testAssemblies --result="$NUnitTestResultsDirectory\\NUnit.xml" --framework="net-4.5" --trace="Error" }
	 #Exec { &$NUnitExe $testAssemblies --result="$NUnitTestResultsDirectory\\NUnit.xml" --framework="net-4.5" --trace="Error" }

		Run-Tests -openCoverExe $openCoverExe -targetExe $NUnitExe -targetArgs $targetArgs -coveragePath $testCoverageReportPath -filter $testCoverageFilter -excludebyattribute:$testCoverageExcludeAttribute -excludebyfile:$testCoverageExcludeByFile
	
}

task Test -depends Compile, TestNunit, Clean -requiredVariables testCoverageDirectory, testCoverageReportPath {
	 
	if(Test-Path $testCoverageReportPath){


		Write-Host "Parsing OPenCover Result"
		$coverage = [xml](Get-Content -Path $testCoverageReportPath)
		$coverageSummary = $coverage.CoverageSession.Summary


		# Write class coverage
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsCCovered' value='$($coverageSummary.visitedClasses)']"
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsCTotal' value='$($coverageSummary.numClasses)']"
		#Write-Host ("##teamcity[buildStatisticValue key='CodeCoverageC' value='{0:N2}']" -f (($coverageSummary.visitedClasses / $coverageSummary.numClasses)*100))

		# Report method coverage
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsMCovered' value='$($coverageSummary.visitedMethods)']"
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsMTotal' value='$($coverageSummary.numMethods)']"
		#Write-Host ("##teamcity[buildStatisticValue key='CodeCoverageM' value='{0:N2}']" -f (($coverageSummary.visitedMethods / $coverageSummary.numMethods)*100))
		
		# Report branch coverage
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsBCovered' value='$($coverageSummary.visitedBranchPoints)']"
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsBTotal' value='$($coverageSummary.numBranchPoints)']"
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageB' value='$($coverageSummary.branchCoverage)']"

		# Report statement coverage
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsSCovered' value='$($coverageSummary.visitedSequencePoints)']"
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsSTotal' value='$($coverageSummary.numSequencePoints)']"
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageS' value='$($coverageSummary.sequenceCoverage)']"

		Exec { &$reportGeneerator $testCoverageReportPath $testCoverageDirectory }
	}

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

