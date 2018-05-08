CREATE PROCEDURE [dbo].[e_ShoppingCarts_Select_UserID]
@UserID uniqueidentifier = null
AS
BEGIN

	SET NOCOUNT ON

	select sc.*, fname, lname, company, address, city, state, title, ZIP, QDate, EmailExists, PhoneExists, FaxExists, c.FullName  as 'country' 
	from ShoppingCarts sc with(nolock) 
		join Subscriptions s with(nolock)  on sc.SubscriptionID = s.SubscriptionID 
		join UAD_Lookup..Country c with(nolock) on s.CountryID = c.countryID 		
	WHERE UserID = @UserID or @UserID is null

END