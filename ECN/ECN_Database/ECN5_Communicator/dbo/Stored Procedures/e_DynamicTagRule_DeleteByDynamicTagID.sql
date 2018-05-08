CREATE PROCEDURE [dbo].[e_DynamicTagRule_DeleteByDynamicTagID]   
@DynamicTagID int,
@UserID int
AS

update DynamicTagRule set IsDeleted=1, UpdatedUserID=@UserID, UpdatedDate=GETDATE() 
where DynamicTagID=@DynamicTagID