CREATE PROCEDURE [dbo].[e_OrderDetails_Select_SalesRepID_Date]
	@SalesRepID uniqueidentifier,
	@startdate varchar(25),
	@enddate varchar(25)
AS
BEGIN

	set nocount on

	set @startdate = @startdate + ' 00:00:00'
	set @enddate = @enddate + ' 23:59:59'
	
	select	au.Email, o.OrderID, CONVERT(VARCHAR(10),OrderDate,101) as orderdate, ordertotal, 
			COUNT(case when SearchTypeName='contact' then OrderDetailsid END) as contactcount ,
			COUNT(case when SearchTypeName='opportunity' then OrderDetailsid END)  as OppCount,
			au1.FullName as SalesRepName
	from	Orders o with(nolock) join 
			OrderDetails od with(nolock) on o.OrderID=od.OrderID join 
			SearchType s with(nolock) on od.SearchTypeID = s.SearchTypeID join 
			ApplicationUsers au with(nolock) on au.userID = o.UserID left outer join
			ApplicationUsers au1 with(nolock) on au.userID = au1.UserID
	where	IsProcessed = 1 and 
			o.orderdate between @startdate and @enddate and 
			(au.SalesRepID =  @SalesRepID or @SalesRepID = cast(cast(0 as binary) as uniqueidentifier))
	group by o.OrderID, OrderDate, ordertotal, au.Email, au1.FullName

END