CREATE PROCEDURE [dbo].[sp_GetRecordsDownload]
	@SalesRepID uniqueidentifier,
	@startdate varchar(25),
	@enddate varchar(25)
AS
	set @startdate = @startdate + ' 00:00:00'
	set @enddate = @enddate + ' 23:59:59'
	
	select o.OrderID, 
		CONVERT(VARCHAR(10),o.OrderDate,101) as OrderDate,
		au.CompanyName,
		au.SalesForceID,
		au.FullName as MemberName,
		s.Company,
		s.FNAME + ' ' + s.LNAME as FullName, 
		s.Title, 
		s.State,			
		--o.OrderTotal,  
		(case when od.IsFreeDownload = 1 then '0.00' else od.Price end) as Price,
		--(case when od.Price = 0  then 'Contact Verification' else 'Opportunity Search' end) as SearchType
		(case when od.SearchTypeID = 1  then 'Contact Verification' else 'Opportunity Search' end) as SearchType
	from Orders o 
		join orderdetails od on o.OrderID =  od.OrderID 
		join ApplicationUsers au on od.UserID = au.UserId 
		join Subscriptions s on od.SubscriptionID =  s.SubscriptionID 
	where o.orderdate between @startdate and @enddate and
			(au.SalesRepID =  @SalesRepID or @SalesRepID = cast(cast(0 as binary) as uniqueidentifier))
	order by o.OrderID

