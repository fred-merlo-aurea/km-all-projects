CREATE PROCEDURE [dbo].[DeleteUserDashboards](@Username nvarchar(50))
AS
BEGIN
	SET NOCOUNT ON;
	delete from dashboard where dashboard.username = @Username;
    
END