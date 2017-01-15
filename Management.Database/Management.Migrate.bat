set MSB="%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"


%MSB% Management.Migrate.proj /t:Migrate /p:DatabaseProvider=SqlServer2012 /p:ConnectionStringConfigPath=ConnectionStrings.config /p:ConnectionStringName=ManagementConnectionString /p:DataMigrationProjectName=Management.Database /p:DataMigrationProjectRootPath=. /p:MigratorTasksDirectory=..\packages\FluentMigrator.1.3.1.0\tools\