set MSB="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"

IF NOT EXIST %MSB% set MSB="%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe"

set BASE=%~dp0
set CONFIG=%BASE%..\Management.Api\Web.config

ECHO %BUILDTYPE%

IF "%BUILDTYPE%" == "" SET BUILDTYPE="Debug"

%MSB% "%BASE:\=\\%Management.Migrate.proj" /t:Migrate /p:DatabaseProvider=SqlServer2012 /p:ConnectionStringConfigPath="%CONFIG:\=\\%" /p:ConnectionStringName=ManagementConnectionString /p:DataMigrationProjectName=Management.Database /p:DataMigrationProjectRootPath="%BASE:\=\\%" /p:MigratorTasksDirectory="%BASE%..\packages\\FluentMigrator.1.6.2\\tools\\" /p:Configuration=%BUILDTYPE%