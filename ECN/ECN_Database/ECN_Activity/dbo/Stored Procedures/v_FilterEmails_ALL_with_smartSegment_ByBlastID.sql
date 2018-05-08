
CREATE  PROCEDURE [dbo].[v_FilterEmails_ALL_with_smartSegment_ByBlastID]
(
	@BlastID int,
	@EmailID int
)
AS
BEGIN 
	DECLARE
	@GroupID int,
	@CustomerID int,
	@FilterID int,
	@Filter varchar(8000),
	@BlastID_and_BounceDomain varchar(250),
	@ActionType varchar(10),
	@refBlastID varchar(2000)	
	
 	set NOCOUNT ON 	
 	
 	SET @Filter = ''
 	if @EmailID is null
		set @EmailID = 0		
 	IF @EmailID > 0
 		SET @Filter = ' and Emails.EmailID = ' + CONVERT(varchar, @EmailID)
	Set @FilterID = NULL
	Set @ActionType = ''
 	Set @BlastID_and_BounceDomain = ''
 	
 	SELECT 
 		@GroupID = GroupID, @CustomerID = CustomerID, @refBlastID = Convert(varchar(2000),RefBlastID)
 	FROM 
 		ecn5_communicator..Blast with (nolock) 
 	WHERE 
 		BlastID=@BlastID and StatusCode <> 'Deleted'
 		
 	EXEC v_FilterEmails_ALL_with_smartSegment @GroupID, @CustomerID, @FilterID, @Filter, @BlastID, @BlastID_and_BounceDomain, @ActionType, @refBlastID, @EmailID

END
