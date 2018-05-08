$currentdirectory = [System.IO.Path]::GetDirectoryName($myInvocation.MyCommand.Definition)

$files = Get-ChildItem $currentdirectory -Recurse -Include web.config, app.config, sharedconnections.config, sharedsettings.config

foreach ($file in $files) {
   $content = Get-Content -Path $file.fullname
   $content = $content -replace "uid=ecn5;pwd=EcN5AcCeSs;database=KMCommon", "uid=webuser;pwd=webuser#23#;database=KMCommon"
   $content = $content -replace "qa-ec2", "specialprojects" 
   $content = $content -replace "10.10.41.198", "10.161.1.47" 
   $content = $content -replace "10.161.1.100", "10.161.1.47" 
   $content = $content -replace "216.17.41.191", "10.161.1.47" 
   $content = $content -replace "216.17.41.241", "10.161.1.47"
   $content = $content -replace "216.17.41.225", "10.161.1.47"
   $content = $content -replace "216.17.41.198", "10.161.1.47"
   Set-Content -Path $file.fullname -Value $content
}
