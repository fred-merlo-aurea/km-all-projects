CREATE PROCEDURE [dbo].[e_GroupConfig_Delete]
@GroupConfigID  int = null,
@CustomerID  int,
@UserID int
AS

update GroupConfig set IsDeleted=1, UpdatedUserID=@UserID, UpdatedDate=GETDATE()
where GroupConfigID=@GroupConfigID