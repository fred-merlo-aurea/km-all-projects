CREATE proc [dbo].[v_EmailGroup_Get_BounceScore] 
(
	@CustomerID int = NULL,
	@GroupID int = NULL,
	@BounceScore int = NULL,
	@BounceCondition varchar(50) = NULL
	
	--set @CustomerID = 1
	--set @GroupID = 49195
	--set @BounceScore = 1
	--set @BounceCondition = '>='
)
as
Begin
	Set nocount on
	declare @SelectSQL varchar(8000)	
	declare @From varchar(500)
	
	IF @GroupID IS NULL
	BEGIN
		SET @From = 'FROM [Emails] e WITH (NOLOCK)'
	END
	ELSE
	BEGIN
		SET @From = 'FROM [Emails] e WITH (NOLOCK) JOIN [EmailGroups] eg WITH (NOLOCK) ON e.EmailID = eg.EmailID AND eg.groupID = ' + CONVERT(varchar,@GroupID)
	END
	
	SET @SelectSQL =    'SELECT 
							COUNT(e.EmailID) AS EmailsCount, e.BounceScore ' +
						@From +
						' WHERE e.CustomerID = ' + CONVERT(varchar,@CustomerID) + ' AND e.BounceScore ' + @BounceCondition + ' ' + CONVERT(varchar,@BounceScore) +
						' GROUP BY e.BounceScore ORDER BY e.BounceScore DESC '

	EXEC(@SelectSQL)
End