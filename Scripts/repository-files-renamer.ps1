# THIS POWERSHELL SCRIPT RENAMES ALL DIRECTORIES, FILES & REPLACES OCCURENCES OF CERTAIN TEXTS IN ALL FILE CONTENTS OF THE TEMPLATE
# REPOSITORY WITH THE SUPPLIED TEXTS.
# HOW TO RUN:
# 	1. EDIT THE GLOBAL VARIABLE VALUES [ONLY: $toBeReplacedBy, $newDocumentationHeading, $newApplicationName] AS PER REQUIREMENT
#	2. MAKE SURE THIS SCRIPT FILE EXISTS WITHIN THE TEMPLATE REPOSITORY ROOT DIRECTORY\Scripts DIRECTORY
#	3. ALSO MAKE SURE THAT THE PROJECT ROOT FOLDER IS NOT LOCKED BY ANY OTHER PROCESS (VISUAL STUDIO/VS CODE/dotnet.exe)
# 	4. OPEN POWERSHELL / WINDOWS TERMINAL AND CHANGE DIRECTORY TO THE REPOSITORY ROOT\Scripts
#	5. RUN THE SCRIPT BY INVOKING THE FOLLOWING COMMAND:
#########################################################################################################################################
#			powershell.exe -executionpolicy bypass -file .\repository-files-renamer.ps1
#########################################################################################################################################

# GLOBAL VARIABLES START
$componentNamePartsToReplace = @("ASP.NET-Core", "NET-Core.Console.DB", "NET-Core.Library", "NET-Core.XUnit")			# DIRECTORY / FILE NAME PARTS [THIS STRINGS ARE PRESENT IN DIRECTORY NAMES / FILE NAMES IN THE TEMPLATE REPOSITORY]
$namespacePartsToReplace = @("ASP.NET.Core.WebAPI", "NET.Core.Console.DB", "NET.Core.Library", "NET.Core.XUnit")		# NAMESPACE PARTS [THIS STRINGS ARE PRESENT IN THE C# NAMESPACES OF THE TEMPLATE REPOSITORY]

$phrasesToReplace = $componentNamePartsToReplace + $namespacePartsToReplace
$toBeReplacedBy = "AuthorManagementSystem"																			# DIRECTORY / FILE NAMES WILL BE REPLACED BY THIS STRING

$oldDocumentationHeading = "# ASP.NET Core Template"																	# TEMPLATE REPOSITORY MAIN README FILE HEADING TEXT
$newDocumentationHeading = "# Author Management System"																# MAIN README FILE HEADING TEXT WILL BE REPLACED BY THIS STRING

$oldApplicationName = "aspdotnet-template"																				# GITHUB REPOSITORY LINK OF TEMPLATE REPOSITORY CONTAINS THIS TEXT, PUBLISH ACTION CONTAINS THIS TEXT AS APPLICATION NAME
$newApplicationName = "author-management-system"																		# WILL BE REPLACED BY THIS STRING
# GLOBAL VARIABLES END

# FUNCTIONS START
function ProcessDirectoryNames($path)
{
	# RENAMES THE DIRECTORIES OF TEMPLATE REPOSITORY
	Write-Host $pwd
	forEach ($directoryNamePartToReplace in $componentNamePartsToReplace)
	{
		forEach ($childItem in Get-ChildItem -Path $path -Recurse -Force -Attributes Directory -Filter "*NET-Core*")	# GET ONLY DIRECTORIES HAVING '%NET-Core%' IN NAME
		{
			Write-Host "PROCESSING DIRECTORY:" $childItem
			$directoryName = $childItem | Select-Object Name
			$newDirectoryName = $directoryName.Name.replace("$directoryNamePartToReplace", "$toBeReplacedBy")
			
			if ($newDirectoryName -ne $directoryName.Name)
			{
				Rename-Item $childItem $newDirectoryName
				Write-Host "PROCESSED DIRECTORY:" $newDirectoryName
			}
		}
	}
}

function ProcessFileNames($path)
{
	# RENAMES THE FILES OF TEMPLATE REPOSITORY 
	forEach ($phraseToReplace in $phrasesToReplace)
	{
		forEach ($childItem in Get-ChildItem -Path $path -Recurse -File -Filter "*$phraseToReplace*")					# GET ONLY FILES
		{
			Write-Host "PROCESSING FILE:" $childItem
			$fileName = $childItem | Select-Object Name
			$newFileName = $fileName.Name.replace("$phraseToReplace", "$toBeReplacedBy")
			if ($newFileName -ne $fileName.Name)
			{
				$fileFullName = $childItem | Select-Object FullName
				$backupPwd = $pwd
				$newWd = ($fileFullName).FullName.replace($fileName.Name, "")
				Write-Host $newWd
				Set-Location $newWd 
				Rename-Item $fileName.Name $newFileName
				Write-Host "PROCESSED FILE:" $newFileName
				Set-Location $backupPwd
			}
		}
	}
}

function ProcessFileContents($path)
{
	# REPLACES ALL OCCURENCES OF GIVEN STRINGS IN ALL FILES BY DESIRED STRING
	$childItemPath;
	$fileContent;
	forEach ($childItem in Get-ChildItem -Path $path -Recurse -File)
	{
		$childItemPath = ($childItem | Select-Object FullName).FullName
		Write-Host $childItemPath
		
		if ($childItemPath -ne $MyInvocation.ScriptName)					# IGNORING THIS SCRIPT, ELSE IT WILL GET MODIFIED
		{
			$fileContent = Get-Content -Path $childItemPath -Raw
			if ($fileContent -ne $null)
			{
				forEach ($phraseToReplace in $phrasesToReplace)
				{
					$fileContent = $fileContent -replace $phraseToReplace, $toBeReplacedBy
				}
				$fileContent = $fileContent -replace $oldDocumentationHeading, $newDocumentationHeading
				$fileContent = $fileContent -replace $oldApplicationName, $newApplicationName
				Set-Content -Path $childItemPath -Value $fileContent
			}
		}
	}
}

function RenameSolutionFile($path)
{
	# RENAMES THE SOLUTION FILE OF TEMPLATE REPOSITORY TO DESIRED NAME
	forEach ($childItem in Get-ChildItem -Path $path -Recurse -File)
	{
		if (($childItem | Select-Object Name).Name -eq "Template.sln")
		{
			$newSolutionFileName = $toBeReplacedBy + ".sln"
			Rename-Item -Path $childItem -NewName $newSolutionFileName
		}
	}
}

function PreprocessForDotnetResources($path)
{
	# SEARCHES FOR ALL C# PROJECT DIRECTORIES, REMOVE THEIR PREVIOUS BUILD ARTIFACTS AND PERFORMS DOTNET CLEAN FOR ALL OF THEM ONE BY ONE
	$backupPwd = $path
	Write-Host "SEARCHING FOR ALL DIRECTORIES.."
	forEach ($childItem in Get-ChildItem -Path $path -Attributes Directory)												# GET ONLY DIRECTORIES
	{
		ProcessForDotnetResources(($childItem | Select-Object FullName).FullName)
	}
	Set-Location $backupPwd
}

function PostprocessForDotnetResources($path)
{
	# SEARCHES FOR ALL C# PROJECT DIRECTORIES AND PERFORMS DOTNET RESTORE & DOTNET BUILD FOR ALL OF THEM ONE BY ONE
	Write-Host "SEARCHING FOR ALL DIRECTORIES.."
	forEach ($childItem in Get-ChildItem -Path $path -Attributes Directory)												# GET ONLY DIRECTORIES
	{
		RecreateNewBuildArtifacts($childItem)
	}
}

function RecreateNewBuildArtifacts($path)
{
	# PERFORMS DOTNET RESTORE AND DOTNET BUILD FOR A C# PROJECT
	if (Test-Path -Path $path\*.csproj -PathType Leaf)
	{
		$backupPwd = $pwd
		Write-Host "CURRENTLY INSIDE DIRECTORY:" $path
		Write-Host "C# PROJECT FILE (.csproj) FOUND"
		Set-Location $path
		Write-Host "PERFORMING .NET RESTORE.."
		dotnet restore --force --no-cache
		Write-Host "PERFORMING .NET BUILD.."
		dotnet build --configuration Debug --no-restore
		Set-Location $backupPwd
	}
}

function ProcessForDotnetResources($path)
{
	# DELETES ALL THE 'bin' & 'obj' DIRECTORIES AND THEIR CONTENT AND PERFORMS DOTNET CLEAN OPERATION FOR A C# PROJECT
	if (Test-Path -Path $path\*.csproj -PathType Leaf)
	{
		$backupPwd = $path
		Write-Host "CURRENTLY INSIDE DIRECTORY:" $path
		Write-Host "C# PROJECT FILE (.csproj) FOUND"
		Set-Location $path
		RemovePreviousBuildArtifacts(@("bin", "obj"))
		Write-Host "PERFORMING .NET CLEAN.."
		dotnet clean
		Set-Location $backupPwd
	}
}

function RemovePreviousBuildArtifacts($artifactDirectories)
{
	# DELETES ALL THE 'bin' & 'obj' DIRECTORIES AND THEIR CONTENT FOR EACH C# PROJECT
	forEach($artifactDirectory in $artifactDirectories)
	{
		Write-Host $pwd\$artifactDirectory
		if (Test-Path -Path $pwd\$artifactDirectory -PathType Container)
		{
			Write-Host $artifactDirectory "DIRECTORY FOUND, DELETING.."
			rm -r -fo $pwd\$artifactDirectory
		}
		else
		{
			Write-Host $pwd\$artifactDirectory "Not found."
		}
	}
}
# FUNCTIONS END

# MAIN SCRIPT START
Write-Host ""
Write-Host "++++ ASP.NET CORE WEB API TEMPLATE | COMPONENT RENAMING SCRIPT +++++" -ForegroundColor Yellow
Write-Host "SCRIPT EXECUTION STARTED"
Write-Host "STAGE 1" -ForegroundColor Red
Write-Host "PRE-PROCESSING STAGE STARTED"
Set-Location -Path ..	# MOVE TO PARENT DIRECTORY (PARENT OF 'Scripts')
PreprocessForDotnetResources($pwd)
Write-Host "PRE-PROCESSING STAGE FINISHED"
Write-Host "STAGE 2" -ForegroundColor Red
Write-Host "RENAMING STAGE STARTED"
ProcessDirectoryNames($pwd)
ProcessFileNames($pwd)
ProcessFileContents($pwd)
RenameSolutionFile($pwd)
Write-Host "RENAMING STAGE FINISHED"
Write-Host "STAGE 3" -ForegroundColor Red
Write-Host "BUILD STAGE STARTED"
PostprocessForDotnetResources($pwd)
Write-Host "BUILD STAGE FINISHED"
Write-Host "SCRIPT EXECUTION FINISHED" -ForegroundColor Yellow
# MAIN SCRIPT END