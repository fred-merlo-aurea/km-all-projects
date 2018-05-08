CREATE PROCEDURE [dbo].[ccp_BriefMedia_BMWU_Insert_Access]
@Xml xml
AS
BEGIN
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		DrupalID int,
		AccessID varchar(8000)		
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(		 
		 DrupalID,AccessID
	)  
	
	SELECT 
		DrupalID,AccessID
	FROM OPENXML(@docHandle, N'/XML/User')  
	WITH   
	(
		DrupalID int 'DrupalID',
		AccessID varchar(8000) 'AccessID'
	)  
	EXEC sp_xml_removedocument @docHandle    

	INSERT INTO tempBriefMediaBMWU (DrupalID, AccessID)
	SELECT DrupalID, AccessID FROM @import

END
GO