CREATE PROCEDURE [dbo].[e_DynamicTag_Delete]   
@DynamicTagID int,
@UserID int
AS

update DynamicTag set IsDeleted=1, UpdatedUserID=@UserID, UpdatedDate=GETDATE() 
where DynamicTagID=@DynamicTagID