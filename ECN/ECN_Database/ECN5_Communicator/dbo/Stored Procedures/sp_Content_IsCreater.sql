CREATE PROCEDURE sp_Content_IsCreater
    @UserID int
AS 
--DECLARE @UserID int = 7636
    SET NOCOUNT ON;    
	
	IF EXISTS  (SELECT TOP 1 @UserID as UserID FROM [ECN5_COMMUNICATOR].[dbo].[Content] c (nolock)
				join [ECN5_ACCOUNTS].[dbo].[Users] u (nolock) on u.UserID = c.CreatedUserID OR u.UserID = c.UpdatedUserID
				where c.CreatedUserID = @UserID OR c.UpdatedUserID = @UserID
				AND c.IsDeleted = 0 )
		BEGIN			
			SELECT  'True' as Result
		END
	ELSE
		BEGIN			
			SELECT  'False' as Result
		END
GO