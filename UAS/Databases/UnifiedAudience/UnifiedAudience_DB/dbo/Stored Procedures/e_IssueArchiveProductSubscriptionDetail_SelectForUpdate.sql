CREATE PROCEDURE [dbo].[e_IssueArchiveProductSubscriptionDetail_SelectForUpdate]
	@ProductID int,
	@IssueID int,
	@PubSubs TEXT
AS
BEGIN

	SET NOCOUNT ON

	CREATE TABLE #Subs
	(  
		RowID int IDENTITY(1, 1)
	  ,[SubID] int
	)
	CREATE NONCLUSTERED INDEX IDX_Subs_SubID ON #Subs(SubID)
	DECLARE @docHandle int
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @PubSubs  
	INSERT INTO #Subs 
	SELECT [SubID]
	FROM OPENXML(@docHandle,N'/XML/S')
	WITH
	(
		[SubID] nvarchar(256) 'ID'
	)
	EXEC sp_xml_removedocument @docHandle		

	Select iapsd.* 
	from #Subs s 
	join IssueArchiveProductSubscription iaps WITH(NOLOCK) on s.SubID = iaps.PubSubscriptionID
	join IssueArchiveProductSubscriptionDetail iapsd WITH(NOLOCK) on iaps.IssueArchiveSubscriptionId = iapsd.IssueArchiveSubscriptionId
	where iaps.IssueID = @IssueID and iaps.PubID = @ProductID

END
