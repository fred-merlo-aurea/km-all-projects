CREATE PROCEDURE [dbo].[e_Group_Select_Search]
@GroupName varchar(50) = NULL,
@FolderID int = NULL,
@CustomerID int,
@UserID int,
@Archived bit = NULL
AS
	DECLARE @SelectSQL varchar(4000)
	SET @SelectSQL = '	SELECT g.*, case when ISNULL(g.FolderID,0) = 0 then ''Root'' else f.FolderName end as ''FolderName'' FROM Groups g WITH(NOLOCK) 
						left outer join Folder f with(nolock) on g.FolderID = f.FolderID
						WHERE g.CustomerID = ' + convert(varchar,@CustomerID) + ' AND IsNull(MasterSupression, 0) = 0
						AND GroupID in (select GroupID from dbo.fn_getGroupsforUser( ' + convert(varchar,@CustomerID) + ' , ' + convert(varchar,@UserID) + ' )) '

	IF @FolderID IS NOT NULL
	BEGIN
		SET @SelectSQL = @SelectSQL + 'AND g.FolderID = ' + convert(varchar,@FolderID) + ' '
	END
	IF @GroupName IS NOT NULL
	BEGIN 
		SET @GroupName = '''%' + UPPER(@GroupName) + '%'''
		SET @SelectSQL = @SelectSQL + 'AND UPPER(GroupName) LIKE ' + convert(varchar,@GroupName) + ' '
	END
	IF @Archived is not null and @Archived = 1
	BEGIN
		   SET @SelectSQL = @SelectSQL + 'AND IsNull(g.Archived, 0) = 1 ' 
	END
	ELSE IF @Archived is not null and @Archived = 0
	BEGIN
		   SET @SelectSQL = @SelectSQL + 'AND IsNull(g.Archived, 0) = 0 ' 
	END

	EXEC(@SelectSQL)