CREATE PROCEDURE [dbo].[e_Edition_Select_CustomerID]   
@CustomerID int

AS
	select e.*, p.CustomerID 
	from 
		Edition e join 
		Publication p on e.PublicationID = P.PublicationID 
	where 
		CustomerID = @CustomerID and 
		e.IsDeleted=0 and 
		p.IsDeleted=0