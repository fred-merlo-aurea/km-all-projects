CREATE PROCEDURE [dbo].[sp_GetEcnContactByEmailAddress] 
(
    @emailAddress varchar(255)
)
AS

SELECT DISTINCT e.EmailAddress,
				e.FirstName,
				e.LastName,
				e.Address,
				e.City,
				e.State,
				e.Zip,
				e.Country,
				e.Company,
				e.Title,
				e.Voice,
				e.Mobile,
				e.Fax
FROM Emails e
WHERE e.EmailAddress = @emailAddress
