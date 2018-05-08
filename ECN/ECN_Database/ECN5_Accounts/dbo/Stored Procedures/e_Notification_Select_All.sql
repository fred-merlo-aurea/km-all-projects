CREATE PROCEDURE [dbo].[e_Notification_Select_All]
AS
BEGIN   
	SET NOCOUNT ON;  
  
	SELECT  *
		FROM Notification WITH(NOLOCK)  
	WHERE IsDeleted = 0
END
