Create PROCEDURE [dbo].[DeleteAllDashboards]
AS
BEGIN
	SET NOCOUNT ON;
	delete from dashboard;
    
END