function Generate-AssemblyInfo
{
param(
[string]$title,
[string]$description,
[string]$company,
[string]$product,
[string]$copyright,
[string]$version,
[string]$file = $(throw "file is a required parameter.")
)
 
	$asmInfo = "using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(""$title"")]
[assembly: AssemblyDescription(""$description"")]
[assembly: AssemblyCompany(""$company"")]
[assembly: AssemblyProduct(""$product"")]
[assembly: AssemblyCopyright(""$copyright"")]
[assembly: AssemblyVersion(""$version"")]
[assembly: AssemblyFileVersion(""$version"")]
[assembly: ComVisible(false)]"
 
	$dir = [System.IO.Path]::GetDirectoryName($file)
 	if ([System.IO.Directory]::Exists($dir) -eq $false)
 	{
		New-Item $dir -ItemType directory | Out-Null
	}
	Write-Host "Generating assembly info file: $file"
	Write-Output $asmInfo > $file
}