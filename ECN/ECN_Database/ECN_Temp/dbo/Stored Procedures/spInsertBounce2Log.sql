

CREATE Procedure [dbo].[spInsertBounce2Log](  
 @xmlDocument Text   
)    
as   
BEGIN  
  
	set nocount on    
	  
	DECLARE @docHandle int    
	  
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlDocument    
	  
	insert into ecn_Temp..Bounce2Logs (EmailID, BlastID, BounceDate, Message)     
	SELECT EmailID, BlastID, LogDate, LogMessage FROM OPENXML(@docHandle, N'/ROOT/BOUNCE') 
		WITH (
			EmailID INT '@EmailID', 
			BlastID INT '@BlastID', 
			LogDate datetime '@LogDate',
			LogMessage varchar(255) '@LogMessage'
		)    

  
	EXEC sp_xml_removedocument @docHandle  
    
END

