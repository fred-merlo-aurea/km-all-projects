CREATE PROCEDURE [dbo].[e_Layout_Select_FolderID_CustomerID]
@FolderID int,
@CustomerID int
AS
	SELECT * FROM Layout WITH (NOLOCK) WHERE FolderID = @FolderID and IsDeleted = 0 and CustomerID=@CustomerID
