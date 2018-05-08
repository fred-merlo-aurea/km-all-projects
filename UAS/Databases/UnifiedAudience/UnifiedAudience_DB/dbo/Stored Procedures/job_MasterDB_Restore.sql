/********************************************************************************

2014-01-01  MTK	Added code to Kill active connections prior to RESTORE. 

*********************************************************************************/

CREATE PROCEDURE [dbo].[job_MasterDB_Restore]
@DbName varchar(200),
@File varchar(500),
@BackupMDF varchar(500),
@BackupLDF varchar(500),
@SavePath varchar(500)

AS
BEGIN

	SET NOCOUNT ON


	DECLARE 
		@SPID INT,
		@KillSQL VARCHAR(MAX)


	/* Cursor for Killing active user connections to Target database for Restore */

	DECLARE cr_processes CURSOR FOR

	SELECT SPID 
	FROM Master.dbo.SysProcesses 
	WHERE Dbid = (SELECT Dbid FROM Master.dbo.Sysdatabases WHERE Name = @DbName AND Spid != @@SPID AND Spid >=50)

	OPEN cr_processes
	FETCH NEXT FROM cr_processes INTO @SPID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SELECT @KillSQL = N'kill ' + cast( @SPID AS VARCHAR(10))
		EXEC (@KillSQL)
		PRINT 'Killed SPID ' + cast( @SPID AS VARCHAR(10))
		FETCH NEXT FROM cr_processes
		INTO @SPID 
	END

	CLOSE cr_processes
	DEALLOCATE cr_processes

	EXEC ('ALTER DATABASE ' + @DbName + ' SET SINGLE_USER')
 
	DECLARE @RestoreMDF varchar(500) = (@SavePath + @DbName + '.mdf')
	DECLARE @RestoreLDF varchar(500) = (@SavePath + @DbName + '.ldf')
	RESTORE DATABASE @DbName 
	FROM DISK = @File
	WITH REPLACE,
	MOVE @BackupMDF TO @RestoreMDF,
	MOVE @BackupLDF TO @RestoreLDF,
	STATS = 1

	EXEC ('ALTER DATABASE ' + @DbName + ' SET MULTI_USER')

END