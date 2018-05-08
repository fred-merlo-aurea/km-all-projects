CREATE  PROCEDURE [dbo].[sp_GetEcnContacts] 
(
    @startRowIndex int,
    @maximumRows int,
    @customerID int,
    @groupID int
)
AS

DECLARE @first_id int, @startRow int

-- Get the first employeeID for our page of records
SET ROWCOUNT @startRowIndex
SELECT @first_id = e.EmailID 
FROM Emails e 
INNER JOIN Groups g ON g.CustomerID  = e.CustomerID
WHERE e.CustomerID = @customerID
AND g.GroupID =@groupID 
ORDER BY e.EmailID

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @maximumRows

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
				e.Fax,
				g.GroupName
FROM Emails e
INNER JOIN Groups g ON g.CustomerID  = e.CustomerID
WHERE e.CustomerID = @customerID
AND g.GroupID =@groupID
AND e.EmailID >= @first_id 
            
SET ROWCOUNT 0
