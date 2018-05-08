CREATE PROCEDURE [dbo].[e_Service_Select_ServiceName]
@ServiceName varchar(100)
AS
	SELECT *
	FROM Service With(NoLock)
	WHERE ServiceName = @ServiceName