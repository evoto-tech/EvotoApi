set MSB="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"

if "%1" == "" (
  set CONFIG=..\EvotoApi\Web.config
) ELSE (
  set CONFIG=..\EvotoApi\Web.%1.config
)


%MSB% Registrar.Migrate.proj /t:Migrate /p:DatabaseProvider=SqlServer2012 /p:ConnectionStringConfigPath=%CONFIG% /p:ConnectionStringName=RegistrarConnectionString /p:DataMigrationProjectName=Registrar.Database /p:DataMigrationProjectRootPath=. /p:MigratorTasksDirectory=..\packages\FluentMigrator.1.6.2\tools\