CREATE PROCEDURE [dbo].[ccp_BriefMedia_BMWU_Update_Users]
@Xml xml
AS
BEGIN
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		DrupalID int,
		FirstName varchar(75),
		LastName varchar(75),
		Email varchar(150)
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(		 
		 DrupalID,FirstName,LastName,Email		 
	)  
	
	SELECT 
		DrupalID,FirstName,LastName,Email
	FROM OPENXML(@docHandle, N'/XML/User')  
	WITH   
	(
		DrupalID int 'DrupalID',
		FirstName varchar(75) 'FirstName',
		LastName varchar(75) 'LastName',
		Email varchar(150) 'Email'
	)  
	EXEC sp_xml_removedocument @docHandle    

	UPDATE tempBriefMediaBMWU
	SET FirstName = x.FirstName,
		LastName = x.LastName,
		Email = x.Email
	FROM @import x WHERE tempBriefMediaBMWU.DrupalID = x.DrupalID

END
GO