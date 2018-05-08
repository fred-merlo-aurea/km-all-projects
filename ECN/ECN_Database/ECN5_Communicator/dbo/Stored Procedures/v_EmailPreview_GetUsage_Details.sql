create PROCEDURE [dbo].[v_EmailPreview_GetUsage_Details]  
(
	@CustomerID int,
	@Month int,
	@Year int
) AS 
BEGIN
	SELECT b.BlastID, b.EmailSubject, b.EmailFrom, b.EmailFromName, e.DateCreated, c.CustomerName, c.customerID
	FROM [EmailPreview] e WITH (NOLOCK) 
		join [Blast] b WITH (NOLOCK) on e.BlastID = b.BlastID 
		join ECN5_ACCOUNTS.dbo.Customer c on c.CustomerID = e.CustomerID
	WHERE 
		e.CustomerID = @CustomerID and 
		month(DateCreated) = @Month and 
		year(DateCreated) = @Year

END