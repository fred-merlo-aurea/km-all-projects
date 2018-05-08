CREATE PROCEDURE [dbo].[e_UserLog_Bulk_Save]
@xml xml
AS

	SET NOCOUNT ON  	

	DECLARE @docHandle int
    
	CREATE TABLE #import
	(  
      [ApplicationID] int
      ,[UserLogTypeID] int
      ,[UserID] int
      ,[Object] nvarchar(256)
      ,[FromObjectValues] varchar(max)
      ,[ToObjectValues] varchar(max)
      ,[DateCreated] datetime
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	INSERT INTO #import 
	(
		 [ApplicationID], [UserLogTypeID], [UserID], [Object],[FromObjectValues],[ToObjectValues],[DateCreated]
	)  
	
	SELECT [ApplicationID], [UserLogTypeID], [UserID], [Object],[FromObjectValues],[ToObjectValues],[DateCreated]
	FROM OPENXML(@docHandle,N'/XML/UserLog')
	WITH
	(
		[ApplicationID] int 'ApplicationID',
		[UserLogTypeID] int 'UserLogTypeID',
		[UserID] int 'UserID',
		[Object] nvarchar(265) 'Object',
		[FromObjectValues] varchar(max) 'FromObjectValues',
		[ToObjectValues] varchar(max) 'ToObjectValues',
		[DateCreated] datetime 'DateCreated'
	)
	
	EXEC sp_xml_removedocument @docHandle
	
	-- Start insert here and pull userLogIds out into @ulIDs
	
	DECLARE @ulIDs table (UserLogID int)
	
	INSERT INTO UserLog ([ApplicationID], [UserLogTypeID], [UserID], [Object],[FromObjectValues],[ToObjectValues],[DateCreated])
	OUTPUT Inserted.UserLogID INTO @ulIDs
	SELECT [ApplicationID], [UserLogTypeID], [UserID], [Object],[FromObjectValues],[ToObjectValues],[DateCreated]
	FROM #import i 

	DROP TABLE #import

	SELECT ul.*	FROM UserLog ul INNER JOIN @ulIDs s on ul.UserLogID = s.UserLogID