CREATE PROCEDURE [dbo].[e_Service_Select_ServiceID]
@ServiceID int
AS
	SELECT *
	FROM Service With(NoLock)
	WHERE ServiceID = @ServiceID