CREATE PROCEDURE sp_Blast_IsCreater
    @UserID int
AS 
--DECLARE @UserID int = -21
    SET NOCOUNT ON;    
	
	IF EXISTS  (SELECT TOP 1 @UserID as UserID FROM [ECN5_COMMUNICATOR].[dbo].[Blast] b (nolock) 	
				join [ECN5_ACCOUNTS].[dbo].[Users] u (nolock) on u.UserID = b.CreatedUserID OR u.UserID = b.UpdatedUserID
				where (b.CreatedUserID = @UserID OR b.UpdatedUserID = @UserID) )
		BEGIN			
			SELECT  'True' as Result
		END
	ELSE
		BEGIN			
			SELECT  'False' as Result
		END
GO