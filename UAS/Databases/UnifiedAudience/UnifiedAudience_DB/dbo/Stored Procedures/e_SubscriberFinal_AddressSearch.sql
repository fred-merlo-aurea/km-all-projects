CREATE PROCEDURE [dbo].[e_SubscriberFinal_AddressSearch]
@Address varchar(255),
@Mailstop varchar(255),
@City varchar(50),
@State varchar(50),
@Zip varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberFinal With(NoLock)
	WHERE Address = @Address
	AND Mailstop = @Mailstop
	AND City = @City
	AND State = @State
	AND Zip = @Zip

END
GO