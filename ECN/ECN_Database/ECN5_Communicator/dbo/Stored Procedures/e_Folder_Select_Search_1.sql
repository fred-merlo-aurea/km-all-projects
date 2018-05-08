CREATE PROCEDURE [dbo].[e_Folder_Select_Search] 
(
@FolderName varchar(250) = NULL,
@FolderID int = NULL,
@CustomerID int,
@UserID int = NULL,
@UpdatedDateFrom datetime = NULL,
@UpdatedDateTo datetime = NULL
)
AS

DECLARE @SelectSQL varchar(4000)
SET @SelectSQL = 'SELECT * FROM Folder WITH(NOLOCK) WHERE CustomerID = ' + convert(varchar,@CustomerID) + ' AND IsDeleted = 0 '

IF @FolderID IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND FolderID = ' + convert(varchar,@FolderID) + ' '
END
IF @FolderName IS NOT NULL
BEGIN 
	SET @FolderName = '''%' + UPPER(@FolderName) + '%'''
	SET @SelectSQL = @SelectSQL + 'AND UPPER(FolderName) LIKE ' + convert(varchar,@FolderName) + ' '
END
IF @UserID IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND UpdatedUserID = ' + convert(varchar,@UserID) + ' '
END
IF @UpdatedDateFrom IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND UpdatedDate >= ''' + convert(varchar,@UpdatedDateFrom) + ''' '
END
IF @UpdatedDateTo IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND UpdatedDate <= ''' + convert(varchar,@UpdatedDateTo) + ''''
END

EXEC(@SelectSQL)
GO