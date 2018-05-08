CREATE PROCEDURE [dbo].[e_Layout_Select_Search] 
(
@LayoutName varchar(250) = NULL,
@FolderID int = NULL,
@CustomerID int = NULL,
@UserID int = NULL,
@UpdatedDateFrom datetime = NULL,
@UpdatedDateTo datetime = NULL,
@Archived bit = NULL
)
AS

DECLARE @SelectSQL varchar(4000)
SET @SelectSQL = '	SELECT * FROM Layout l WITH(NOLOCK) 
                            left outer join Content c with(nolock) on l.ContentSlot1 = c.ContentID 
                            or l.ContentSlot2 = c.ContentID 
                            or l.ContentSlot3 = c.ContentID 
                            or l.ContentSlot4 = c.ContentID
                            or l.ContentSlot5 = c.ContentID 
                            or l.ContentSlot6 = c.ContentID
                            or l.ContentSlot7 = c.ContentID
                            or l.ContentSlot8 = c.ContentID
                            or l.ContentSlot9 = c.ContentID
							WHERE
							l.CustomerID = ' + convert(varchar,@CustomerID) + ' AND l.IsDeleted = 0  
							and ISNULL(l.ContentSlot1,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot1,0) ELSE null END   
							and ISNULL(l.ContentSlot2,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot2,0) ELSE null END 
							and ISNULL(l.ContentSlot3,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot3,0) ELSE null END
							and ISNULL(l.ContentSlot4,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot4,0) ELSE null END 
							and ISNULL(l.ContentSlot5,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot5,0) ELSE null END 
							and ISNULL(l.ContentSlot6,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot6,0) ELSE null END
							and ISNULL(l.ContentSlot7,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot7,0) ELSE null END 
							and ISNULL(l.ContentSlot8,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot8,0) ELSE null END
							and ISNULL(l.ContentSlot9,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot9,0) ELSE null END '

IF @FolderID IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND l.FolderID = ' + convert(varchar,@FolderID) + ' '
END
IF @LayoutName IS NOT NULL
BEGIN 
	SET @LayoutName = '''%' + UPPER(@LayoutName) + '%'''
	SET @SelectSQL = @SelectSQL + 'AND UPPER(l.LayoutName) LIKE ' + @LayoutName + ' '
END
IF @UserID IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND l.UpdatedUserID = ' + convert(varchar,@UserID) + ' '
END
IF @Archived is not null and @Archived = 1
BEGIN
       SET @SelectSQL = @SelectSQL + 'AND IsNull(l.Archived, 0) = 1 ' 
END
ELSE IF @Archived is not null and @Archived = 0
BEGIN
       SET @SelectSQL = @SelectSQL + 'AND IsNull(l.Archived, 0) = 0 ' 
END
IF @UpdatedDateFrom IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND l.UpdatedDate >= ''' + convert(varchar,@UpdatedDateFrom) + ''' '
END
IF @UpdatedDateTo IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND l.UpdatedDate <= ''' + convert(varchar,@UpdatedDateTo) + ''''
END

EXEC(@SelectSQL)
