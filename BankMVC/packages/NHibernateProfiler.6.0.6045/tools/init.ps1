param($installPath, $toolsPath, $package, $project)

# Run the profiler
&"$toolsPath\NHProf.exe"

$projectPath = $project.FullName
$lang, $appStartFileName, $regexMainMethod, $preStartCall
	
if ($projectPath.EndsWith("csproj")) {
		$lang = "cs"
		$appStartFileName = "Program.cs"
		$regexMainMethod = "static void Main.*\(.*\)[^{]+{"
		$preStartCall = [System.Environment]::NewLine + "            NHibernateProfilerBootstrapper.PreStart();" + [System.Environment]::NewLine
}

$appStartFilePath = Join-Path (Split-Path $projectPath -Parent) $appStartFileName

if (Test-Path $appStartFilePath) {
	# Inject a call to PreStart inside the application start method
	# Get program.cs/Module1.vb file content
	$programFileContent = ""
	(Get-Content $appStartFilePath) | Foreach-Object { $programFileContent += $_ + [System.Environment]::NewLine }
		
	# Get the main method signature
	$match = [Regex]::Match($programFileContent, $regexMainMethod)
	$mainSignature = ""
	if ($match.Success)
	{
		$mainSignature = $match.Value
	}
		
	if (!($programFileContent -match "NHibernateProfilerBootstrapper.PreStart\(\)|NHibernateProfiler.Initialize\("))
	{
		$programFileContent.Insert($programFileContent.IndexOf($mainSignature) + $mainSignature.Length, $preStartCall) | Set-Content $appStartFilePath
	}
}