CREATE PROCEDURE [dbo].[v_EmailPreview_GetUsage_Automated]  
(
	@CustomerID varchar(5000), 
	@StartDate datetime,
	@EndDate datetime
) AS 
BEGIN
	SELECT c.CustomerName, c.CustomerID, COUNT(EmailTestID) as counts 
	FROM [EmailPreview] e WITH (NOLOCK) 
		join ECN5_ACCOUNTS.dbo.Customer c on c.CustomerID = e.CustomerID
		join (select items as customerID from dbo.fn_Split(@customerID, ',')) tmp on tmp.customerID = c.customerID 
	WHERE 
		DateCreated between @StartDate and @EndDate --and 
		--TimeCreated between CONVERT(time, @StartDate) and CONVERT(time, @EndDate)
	group by c.CustomerName, c.CustomerID
END