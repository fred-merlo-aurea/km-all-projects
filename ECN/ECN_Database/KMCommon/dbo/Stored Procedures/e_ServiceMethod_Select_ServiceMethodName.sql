CREATE PROCEDURE [dbo].[e_ServiceMethod_Select_ServiceMethodName]
@ServiceMethodName varchar(50)
AS
	SELECT *
	FROM ServiceMethod With(NoLock)
	WHERE ServiceMethodName = @ServiceMethodName