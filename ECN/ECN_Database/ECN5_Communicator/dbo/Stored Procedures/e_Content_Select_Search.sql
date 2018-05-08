CREATE PROCEDURE [dbo].[e_Content_Select_Search] 
(
@ContentTitle varchar(250) = NULL,
@FolderID int = NULL,
@CustomerID int,
@UserID int = NULL,
@UpdatedDateFrom datetime = NULL,
@UpdatedDateTo datetime = NULL,
@Archived bit = NULL
)
AS

DECLARE @SelectSQL varchar(4000)
SET @SelectSQL = 'SELECT * FROM Content WITH(NOLOCK) WHERE CustomerID = ' + convert(varchar(10),@CustomerID) + ' AND IsDeleted = 0 '

IF @FolderID IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND FolderID = ' + convert(varchar(10),@FolderID) + ' '
END
IF @ContentTitle IS NOT NULL
BEGIN 
	SET @ContentTitle = '''%' + UPPER(@ContentTitle) + '%'''
	SET @SelectSQL = @SelectSQL + 'AND UPPER(ContentTitle) LIKE ' + @ContentTitle + ' '
END
IF @UserID IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND UpdatedUserID = ' + convert(varchar(10),@UserID) + ' '
END
IF @Archived is not null and @Archived = 1
BEGIN
       SET @SelectSQL = @SelectSQL + 'AND IsNull(Archived, 0) = 1 ' 
END
ELSE IF @Archived is not null and @Archived = 0
BEGIN
       SET @SelectSQL = @SelectSQL + 'AND IsNull(Archived, 0) = 0 ' 
END
IF @UpdatedDateFrom IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND UpdatedDate >= ''' + convert(varchar(30),@UpdatedDateFrom) + ''' '
END
IF @UpdatedDateTo IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND UpdatedDate <= ''' + convert(varchar(30),@UpdatedDateTo) + ''''
END

EXEC(@SelectSQL)