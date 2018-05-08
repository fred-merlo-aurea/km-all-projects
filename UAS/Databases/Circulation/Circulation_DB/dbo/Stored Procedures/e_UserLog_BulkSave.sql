CREATE PROCEDURE [dbo].[e_UserLog_BulkSave]
@xml xml
AS

	SET NOCOUNT ON  	

	DECLARE @docHandle int
    DECLARE @insertcount int
    
	DECLARE @import TABLE    
	(  
		ApplicationID int,
		UserLogTypeID int,
		UserID int,
		[Object] varchar(50),
		FromObjectValues text,
		ToObjectValues text,
		DateCreated datetime
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  
	INSERT INTO @import 
	(
		 ApplicationID,UserLogTypeID,UserID,[Object],FromObjectValues,ToObjectValues,DateCreated
	)  
	
	SELECT ApplicationID,UserLogTypeID,UserID,[Object],FromObjectValues,ToObjectValues,DateCreated
	FROM OPENXML(@docHandle,N'/XML/UserLog')
	WITH
	(
		ApplicationID int 'ApplicationID' ,
		UserLogTypeID int 'UserLogTypeID',
		UserID int 'UserID',
		[Object] varchar(50) 'Object',
		FromObjectValues text 'FromObjectValues',
		ToObjectValues text 'ToObjectValues',
		DateCreated datetime 'DateCreated'
	)
	
	EXEC sp_xml_removedocument @docHandle
	
	DECLARE @userLogId TABLE (UserLogId int)
	
	INSERT INTO UserLog(ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated)
	OUTPUT inserted.UserLogID INTO @userLogId
	SELECT ApplicationID,UserLogTypeID,UserID,[Object],FromObjectValues,ToObjectValues,(CASE WHEN ISNULL(DateCreated,'')='' THEN GETDATE() ELSE DateCreated END) AS DateCreated
	FROM @import

	SELECT ul.UserLogId,ApplicationID,UserLogTypeID,UserID,[OBJECT],FromObjectValues,ToObjectValues,DateCreated FROM @userLogId uld INNER JOIN UserLog ul ON uld.UserLogId = ul.UserLogID



