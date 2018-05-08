CREATE  PROCEDURE [dbo].[sp_insertLog_Sends]
(
	@BlastID int,
	@ActionTypeCode varchar(25),
	@xmlDoc Text   
)
AS  
  
BEGIN   
	DECLARE @xmlHandle INT,
			@TestBlast varchar(1),
			@ActionValue varchar(10),
			@currentdt datetime

	set @ActionValue = 'Send'
	set @currentdt = GETDATE()
	
	select @TestBlast = TestBlast from blast where blastID = @blastID  
  
	--if @TestBlast='Y'
	--Begin
	--	set @ActionValue = 'testsend'
		
	--	if @ActionTypeCode='send'
	--		set @ActionTypeCode = 'testsend'
	--End
	
	EXEC sp_xml_preparedocument @xmlHandle OUTPUT, @xmlDoc  
  
	Insert into EmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionValue, ActionDate)  
	SELECT	EmailID, @BlastID, @ActionTypeCode, @ActionValue, @currentdt 
	FROM OPENXML (@xmlHandle, '//e', 2) WITH  
	(  
		EmailID  int '.'
	)  
  
	EXEC sp_xml_removedocument @xmlHandle  
END