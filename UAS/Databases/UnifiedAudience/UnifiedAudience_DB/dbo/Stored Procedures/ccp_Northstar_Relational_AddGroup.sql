CREATE Procedure [dbo].[ccp_Northstar_Relational_AddGroup]
@Xml xml
AS
BEGIN
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		[PubCode] varchar(500) null,
		[GlobalUserKey] int null,
		[GroupId] varchar(500) null,
		[IsRecent] bit null,
		[AddDate] DateTime null,
		[DropDate] DateTime null
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(		 
		PubCode,GlobalUserKey,GroupId,IsRecent,AddDate,DropDate		 	 
	)  
	
	SELECT 
		PubCode,GlobalUserKey,GroupId,IsRecent,AddDate,DropDate
	FROM OPENXML(@docHandle, N'/XML/Northstar')  
	WITH   
	(
		PubCode varchar(500) 'PubCode',
		GlobalUserKey int 'GlobalUserKey',
		GroupId varchar(500) 'GroupId',
		IsRecent bit 'IsRecent',
		AddDate DateTime 'AddDate',
		DropDate DateTime 'DropDate'
	)  
	EXEC sp_xml_removedocument @docHandle    
    
	UPDATE tempNorthstarWebPersonGroup
		SET PubCode = x.GroupId,
			GroupId = x.GroupId,
			IsRecent = x.IsRecent,
			AddDate = x.AddDate,
			DropDate = x.DropDate
	FROM @import x WHERE tempNorthstarWebPersonGroup.GlobalUserKey = x.GlobalUserKey

END
GO