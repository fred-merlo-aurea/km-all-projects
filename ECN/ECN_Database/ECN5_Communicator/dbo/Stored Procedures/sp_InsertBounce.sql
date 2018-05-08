

CREATE Procedure [dbo].[sp_InsertBounce](  
 @xmlDocument Text   
)    
as   
BEGIN  
  
	set nocount on    
	  
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_InsertBounce', GETDATE())
  
	  
	DECLARE @docHandle int    
	  
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlDocument    
	  
	insert into Emailactivitylog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue, ActionNotes, Processed)     
	SELECT EmailID, BlastID, 'bounce', getDate(), ActionValue, ActionNotes, 'Y' FROM OPENXML(@docHandle, N'/ROOT/BOUNCE') 
		WITH (
			EmailID INT '@EmailID', 
			BlastID INT '@BlastID', 
			ActionValue varchar(255) '@BounceWeight',
			ActionNotes varchar(500) '@Signature'
		)    

	update emails
	set bouncescore = ISNULL(bouncescore,0) + (inn.bcount * 2)
	from emails join 
	(
		SELECT EmailID, count(EmailID) as bcount FROM OPENXML(@docHandle, N'/ROOT/BOUNCE') 
		WITH (
			EmailID INT '@EmailID', 
			BlastID INT '@BlastID', 
			ActionValue varchar(255) '@BounceWeight',
			ActionNotes varchar(500) '@Signature'
		)    
		where actionvalue in ('hard','hardbounce') 
		group by EmailID
	) inn on emails.emailID = inn.emailID

	  
	EXEC sp_xml_removedocument @docHandle  
    
END

