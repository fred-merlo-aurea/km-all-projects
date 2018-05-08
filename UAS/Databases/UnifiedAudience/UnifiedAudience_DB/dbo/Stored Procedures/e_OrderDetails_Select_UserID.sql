CREATE PROCEDURE [dbo].[e_OrderDetails_Select_UserID]
@UserID uniqueidentifier = null
AS
BEGIN

	set nocount on

	select od.*, 
		fname, 
		lname, 
		company, 
		address, 
		city, 
		state, 
		c.ShortName as 'Country', 
		QDate, 
		EmailExists, 
		PhoneExists, 
		FaxExists, 
		zip, 
		PLUS4, 
		TITLE, 
		FORZIP, 
		PHONE, 
		FAX, 
		EMAIL
	from OrderDetails od with(nolock) join 
		Subscriptions s with(nolock)  on od.SubscriptionID = s.SubscriptionID join 
		UAD_Lookup..Country c with(nolock) on s.CountryID = c.countryID 		
	WHERE UserID = @UserID or @UserID is null

END