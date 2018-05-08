create PROCEDURE [dbo].[e_Edition_Select_PublicationID]   
@PublicationID int

AS
	select e.*, p.CustomerID 
	from 
		Edition e join 
		Publication p on e.PublicationID = P.PublicationID 
	where 
		e.PublicationID = @PublicationID and 
		e.IsDeleted=0 and 
		p.IsDeleted=0