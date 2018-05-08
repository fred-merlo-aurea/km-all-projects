CREATE PROCEDURE [dbo].[e_Subscription_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM Subscriptions With(NoLock)
	ORDER BY LNAME,FNAME

END
GO