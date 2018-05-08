CREATE PROCEDURE [dbo].[sp_Layout_IsCreater]
    @UserID int
AS 
--DECLARE @UserID int = -21
    SET NOCOUNT ON;    
	
	IF EXISTS  (SELECT TOP 1 @UserID as UserID FROM [ECN5_COMMUNICATOR].[dbo].[Layout] l (nolock)
				join [ECN5_ACCOUNTS].[dbo].[Users] u (nolock) on u.UserID = l.CreatedUserID OR u.UserID = l.UpdatedUserID
				where (l.CreatedUserID = @UserID OR l.UpdatedUserID = @UserID)
				AND l.IsDeleted = 0 )				
		BEGIN			
			SELECT  'True' as Result
		END
	ELSE
		BEGIN			
			SELECT  'False' as Result
		END