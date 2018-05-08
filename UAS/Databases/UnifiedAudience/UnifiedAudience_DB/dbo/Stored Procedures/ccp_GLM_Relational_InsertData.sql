CREATE PROCEDURE [dbo].[ccp_GLM_Relational_InsertData]
@Xml xml
AS
BEGIN
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		[EMAIL] varchar(150) null, 
		[LEADSSENT] int null,
		[LIKES] int null,
		[BOARDFOLLOWS] int null,
		[EXHIBITORFOLLOWS] int null			
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(		 
		[EMAIL],[LEADSSENT],[LIKES],[BOARDFOLLOWS],[EXHIBITORFOLLOWS]
	)  
	
	SELECT 
		[EMAIL],[LEADSSENT],[LIKES],[BOARDFOLLOWS],[EXHIBITORFOLLOWS]
	FROM OPENXML(@docHandle, N'/XML/GLM')  
	WITH   
	(
		[EMAIL] varchar(150) 'EMAIL', 
		[LEADSSENT] int 'LEADSSENT',
		[LIKES] int 'LIKES',
		[BOARDFOLLOWS] int 'BOARDFOLLOWS',
		[EXHIBITORFOLLOWS] int 'EXHIBITORFOLLOWS'
	)  
	EXEC sp_xml_removedocument @docHandle    

	INSERT INTO tempGLMRelational ([EMAIL],[LEADSSENT],[LIKES],[BOARDFOLLOWS],[EXHIBITORFOLLOWS])	
	SELECT EMAIL,
		SUM(LEADSSENT) as LEADSSENT,
		SUM(LIKES) as LIKES,
		SUM(BOARDFOLLOWS) as BOARDFOLLOWS,
		SUM(EXHIBITORFOLLOWS) as EXHIBITORFOLLOWS  
	FROM @import
	GROUP BY EMAIL
	
END
GO