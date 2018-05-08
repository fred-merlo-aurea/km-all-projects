CREATE PROCEDURE [dbo].[e_ProductSubscription_SelectForUpdate]
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

	Select * 
	from PubSubscriptions ps WITH(NOLOCK)
	join #Subs s on ps.PubSubscriptionID = s.SubID
		

END