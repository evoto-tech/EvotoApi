set MSB="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
set BASE=%~dp0
set CONFIG=%BASE%..\EvotoApi\Web.config

%MSB% "%BASE:\=\\%Management.Migrate.proj" /t:Migrate /p:DatabaseProvider=SqlServer2012 /p:ConnectionStringConfigPath="%CONFIG:\=\\%" /p:ConnectionStringName=ManagementConnectionString /p:DataMigrationProjectName=Management.Database /p:DataMigrationProjectRootPath="%BASE:\=\\%" /p:MigratorTasksDirectory="%BASE%..\packages\\FluentMigrator.1.6.2\\tools\\"