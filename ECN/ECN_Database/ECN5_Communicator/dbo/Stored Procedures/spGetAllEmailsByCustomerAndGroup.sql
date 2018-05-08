CREATE PROCEDURE [dbo].[spGetAllEmailsByCustomerAndGroup]
	@CustomerID int, 
	@GroupID int
AS
BEGIN
	select e.EmailAddress from Emails e join EmailGroups eg 
	on e.EmailID = eg.EmailID 
	where eg.GroupID = @GroupID and e.CustomerID = @CustomerID	
END
