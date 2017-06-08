nuget pack "..\MvcPagedList\MvcPagedList.csproj" -Prop Configuration=Release
copy "%~dp0*.nupkg" "%localappdata%\NuGet\Cache"
pause
