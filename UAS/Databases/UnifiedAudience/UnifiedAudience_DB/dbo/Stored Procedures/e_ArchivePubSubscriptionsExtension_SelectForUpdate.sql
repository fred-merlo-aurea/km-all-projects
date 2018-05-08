CREATE PROCEDURE [dbo].[e_ArchivePubSubscriptionsExtension_SelectForUpdate]
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

	Select apse.* 
	from #Subs s 
	join IssueArchiveProductSubscription iaps WITH(NOLOCK) on s.SubID = iaps.PubSubscriptionID
	join IssueArchivePubSubscriptionsExtension apse WITH(NOLOCK) on iaps.IssueArchiveSubscriptionId = apse.IssueArchiveSubscriptionId
	where iaps.IssueID = @IssueID and iaps.PubID = @ProductID

END
