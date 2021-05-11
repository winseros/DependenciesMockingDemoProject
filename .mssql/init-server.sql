if (not exists (select 1 from sys.databases where name = 'DependenciesMockingDatabase'))
begin
    create database DependenciesMockingDatabase
end
go

--This is the application-level user; Must have minimal ammount of permissions the app needs to function
if (not exists (select * from sys.sql_logins where name = 'DependenciesMocking_App'))
begin
    use DependenciesMockingDatabase
    create login DependenciesMocking_App with password='1QAZ2wsx3EDC', default_database=DependenciesMockingDatabase
    create user DependenciesMocking_App from login DependenciesMocking_App
    exec sys.sp_addrolemember db_datareader, DependenciesMocking_App
    exec sys.sp_addrolemember db_datawriter, DependenciesMocking_App
end

--This is the administration-level user; User by integration tests engine
if (not exists (select 1 from sys.sql_logins where name = 'DependenciesMocking_Adm'))
begin
    use DependenciesMockingDatabase
    create login DependenciesMocking_Adm with password='1QAZ2wsx3EDC', default_database=DependenciesMockingDatabase
    create user DependenciesMocking_Adm from login DependenciesMocking_Adm
    exec sys.sp_addrolemember db_datareader, DependenciesMocking_Adm
    exec sys.sp_addrolemember db_datawriter, DependenciesMocking_Adm
    exec sys.sp_addrolemember db_ddladmin, DependenciesMocking_Adm
end