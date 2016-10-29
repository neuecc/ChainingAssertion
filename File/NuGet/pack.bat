pushd %~dp0
"%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\csc.exe" pre-package.cs -nologo
pre-package.exe

nuget pack chainingassertion.nuspec
nuget pack chainingassertion-mbunit.nuspec
nuget pack chainingassertion-nunit.nuspec
nuget pack chainingassertion-xunit.nuspec