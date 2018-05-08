CREATE PROCEDURE [dbo].[sp_Content_IsCreater]
    @UserID int
AS 
--DECLARE @UserID int = 7996
    SET NOCOUNT ON;    
	
	IF EXISTS  (SELECT TOP 1 @UserID as UserID 
				FROM [ECN5_COMMUNICATOR].[dbo].[Content] 
				where (CreatedUserID = @UserID OR UpdatedUserID = @UserID)
				AND IsDeleted = 0)
		BEGIN			
			SELECT  'True' as Result
		END
	ELSE
		BEGIN			
			SELECT  'False' as Result
		END
GO