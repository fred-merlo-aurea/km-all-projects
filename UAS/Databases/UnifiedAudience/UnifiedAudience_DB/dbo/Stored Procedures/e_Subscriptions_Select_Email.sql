CREATE PROCEDURE [dbo].[e_Subscriptions_Select_Email]
@Email varchar(100)
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT * 
	FROM Subscriptions With(NoLock)
	WHERE EMAIL = @Email

END