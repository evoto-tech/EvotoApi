set MSB="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
set BASE=%dp0%
set CONFIG=%BASE%..\EvotoApi\Web.config

%MSB% %BASE%Registrar.Migrate.proj /t:Migrate /p:DatabaseProvider=SqlServer2012 /p:ConnectionStringConfigPath=%CONFIG% /p:ConnectionStringName=RegistrarConnectionString /p:DataMigrationProjectName=Registrar.Database /p:DataMigrationProjectRootPath=%BASE% /p:MigratorTasksDirectory=%BASE%..\packages\FluentMigrator.1.6.2\tools\