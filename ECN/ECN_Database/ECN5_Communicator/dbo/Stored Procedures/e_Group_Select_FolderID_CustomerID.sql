CREATE PROCEDURE [dbo].[e_Group_Select_FolderID_CustomerID]
@FolderID int,
@CustomerID int,
@UserID int
AS
	SELECT * FROM Groups WITH (NOLOCK) WHERE ISNULL(FolderID,0) = @FolderID and CustomerID=@CustomerID and GroupID in (select GroupID from dbo.fn_getGroupsforUser(@CustomerID,@UserID))
