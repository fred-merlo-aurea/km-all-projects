CREATE PROCEDURE [dbo].[sp_GetPaymentDetails]
	@startdate varchar(25),
	@enddate varchar(25)

AS
BEGIN

	SET NOCOUNT ON

	set @startdate = @startdate + ' 00:00:00'
	set @enddate = @enddate + ' 23:59:59'
	
	select u.UserID, 
		   PaymentTransactionID, 
		   CONVERT(VARCHAR(10),DateAdded,101) as DateAdded, 
		   'CDM Registration' as RegType, 
		   ('xxx' + CardNo) as CardNo,  
		   SubscriptionFee as Amount, 
		   (Fname + ' ' + Lname) as FullName, 
		   Phone, 
		   BillingAddress1, 
		   BillingAddress2, 
		   BillingCity, 
		   BillingState, 
		   BillingZip, 
		   c.Country, 
		   au.Email, 
		   au.CompanyName,
		   au.SalesForceID
	from UsersRegistration u WITH(NOLOCK)
		join Country c WITH(NOLOCK) on u.CountryID = c.CountryID 
		join ApplicationUsers au WITH(NOLOCK) on au.UserID = u.UserID
	where u.DateAdded between @startdate and @enddate 
	union all
		select b.UserID, 
			   PaymentTransactionID, 
			   CONVERT(VARCHAR(10),DateAdded,101) as DateAdded, 
			   'Subscription Fee' as RegType, 
			   ('xxx' + CardNo) as CardNo, 
			   SubscriptionFee as Amount, 
			   CardHolderName as FullName, 
			   Phone, 
			   BillingAddress1, 
			   BillingAddress2, 
			   BillingCity, 
			   BillingState, 
			   BillingZip, 
			   c.Country, 
			   au.Email, 
			   au.CompanyName,
			   au.SalesForceID
		from BillingHistory b WITH(NOLOCK)
			join Country c WITH(NOLOCK) on b.BillingCountryID = c.CountryID 
			join ApplicationUsers au WITH(NOLOCK) on au.UserID = b.UserID
		where b.DateAdded between @startdate and @enddate 
	union all
		select o.UserID, 
			   PaymentTransactionID, 
			   CONVERT(VARCHAR(10),OrderDate,101) as DateAdded, 
			   'Download Charges' as RegType, 
			   ('xxx' + CardNo) as CardNo, 
			   OrderTotal as Amount, 
			   CardHolderName as FullName, 
			   CardHolderPhone, 
			   CardHolderAddress1, 
			   CardHolderAddress2, 
			   CardHolderCity, 
			   CardHolderState, 
			   CardHolderZip, 
			   c.Country, 
			   au.Email, 
			   au.CompanyName,
			   au.SalesForceID
		from orders o WITH(NOLOCK)
			join  Country c WITH(NOLOCK) on o.CardHolderCountryID = c.CountryID 
			join ApplicationUsers au WITH(NOLOCK) on au.UserID = o.UserID
		where o.OrderDate between @startdate and @enddate
		ORDER BY 3;

END