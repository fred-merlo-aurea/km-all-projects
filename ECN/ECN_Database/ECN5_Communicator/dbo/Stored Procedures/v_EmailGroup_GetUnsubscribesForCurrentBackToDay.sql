CREATE PROCEDURE [dbo].[v_EmailGroup_GetUnsubscribesForCurrentBackToDay]  
(
	--declare 
	@CustomerID int,
	@DaysBack int
	--set @CustomerID = 1
	--set @FolderID = 0
	--set @DaysBack = -3
) AS 
BEGIN
	DECLARE @Start Date
	DECLARE @End Date
	
	SET @Start = CONVERT(DATE,DATEADD(DD, @DaysBack, GETDATE()))
	SET @END = CONVERT(DATE,GETDATE())

	SELECT 
		e.EmailID, e.EmailAddress
	FROM 
		[Emails] e WITH (NOLOCK)
		JOIN [EmailGroups] eg WITH (NOLOCK) ON e.EmailID = eg.EmailID
	WHERE 
		e.CustomerID = @CustomerID AND
		eg.SubscribeTypeCode = 'U' AND 
		( 
			(eg.LastChanged BETWEEN @Start AND @End) OR 
			(eg.CreatedOn BETWEEN @Start AND @End)
		) 
END