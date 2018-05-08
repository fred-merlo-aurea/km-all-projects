CREATE PROCEDURE [dbo].[e_Filter_Select_Search]
@CustomerID int,
@GroupID int= NULL,
@Archived bit = NULL

AS
	DECLARE @SelectSQL varchar(4000)
	SET @SelectSQL = 'SELECT * FROM Filter WITH(NOLOCK) WHERE CustomerID = ' + convert(varchar(10),@CustomerID) + ' AND IsDeleted = 0 '
	IF @GroupID IS NOT NULL
	BEGIN
		SET @SelectSQL = @SelectSQL + 'AND GroupID = ' + convert(varchar(10),@GroupID) + ' '
	END
	IF @Archived is not null and @Archived = 1
		BEGIN
			SET @SelectSQL = @SelectSQL + 'AND IsNull(Archived, 0) = 1 ' 
		END
	ELSE IF @Archived is not null and @Archived = 0
	BEGIN
	SET @SelectSQL = @SelectSQL + 'AND IsNull(Archived, 0) = 0 ' 
	END

	EXEC(@SelectSQL)

