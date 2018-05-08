CREATE VIEW vw_Emails

AS

SELECT 
	c.customername,
	e.EmailID,
	e.EmailAddress,
	e.Title,
	e.FirstName,
	e.LastName,
	e.FullName,
	e.Company,
	e.Occupation,
	e.Address,
	e.Address2,
	e.City,
	e.State,
	e.Zip,
	e.Country,
	e.Notes,
	e.DateAdded,
	e.DateUpdated
FROM 
	Emails e WITH(NOLOCK)
	INNER JOIN [$(ECN5_Accounts)].dbo.Customer c WITH (NOLOCK) ON c.Customerid = e.customerid