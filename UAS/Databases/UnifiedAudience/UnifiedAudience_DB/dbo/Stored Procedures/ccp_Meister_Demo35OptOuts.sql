create procedure ccp_Meister_Demo35OptOuts
@xml xml,
@ProcessCode varchar(50)
as
BEGIN
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		[Email] varchar(100)
	)  
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(
		 Email
	)  
	SELECT 
		 Email
	FROM OPENXML(@docHandle, N'/XML/Emails')   
	WITH   
	(  
		Email varchar(100) 'Email'
	)  
	EXEC sp_xml_removedocument @docHandle  
	
	update sf
	set ThirdPartyPermission = 0
	from SubscriberFinal sf
	join @import i on sf.Email = i.Email
	where sf.ProcessCode = @ProcessCode

END
go