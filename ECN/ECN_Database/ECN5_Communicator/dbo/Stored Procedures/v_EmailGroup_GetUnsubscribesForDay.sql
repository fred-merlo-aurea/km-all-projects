CREATE PROCEDURE [dbo].[v_EmailGroup_GetUnsubscribesForDay]  
(
	--declare 
	@CustomerID int,
	@FolderID int,
	@DaysBack int
	--set @CustomerID = 1
	--set @FolderID = 0
	--set @DaysBack = -3
) AS 
BEGIN
	DECLARE @Start Date
	DECLARE @End Date
	
	SET @Start = CONVERT(DATE,DATEADD(DD, @DaysBack, GETDATE()))
	SET @END = CONVERT(DATE,DATEADD(DD, @DaysBack + 1, GETDATE()))

	SELECT 
		DISTINCT e.EmailID 
	FROM 
		[Emails] e WITH (NOLOCK)
		JOIN [EmailGroups] eg WITH (NOLOCK) ON e.EmailID = eg.EmailID 
		JOIN [Groups] g on eg.GroupID = g.groupID 
	WHERE 
		g.FolderID = @FolderID AND 
		g.CustomerID = @CustomerID AND
		eg.SubscribeTypeCode = 'U' AND 
		( 
			(eg.LastChanged BETWEEN @Start AND @End) OR 
			(eg.CreatedOn BETWEEN @Start AND @End)
		) 
END