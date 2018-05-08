CREATE PROCEDURE [dbo].[v_EmailPreview_GetUsage_BaseChannelID]  
(
	@BaseChannelID int,
	@Month int,
	@Year int
) AS 
BEGIN
	SELECT c.CustomerName, c.CustomerID, COUNT(EmailTestID) as counts
	FROM [EmailPreview] e WITH (NOLOCK) 
		join ECN5_ACCOUNTS.dbo.Customer c on c.CustomerID = e.CustomerID
	WHERE 
		c.BaseChannelID = @BaseChannelID and  
		month(DateCreated) = @Month and 
		year(DateCreated) = @Year
		
    group by c.CustomerName, c.CustomerID
END