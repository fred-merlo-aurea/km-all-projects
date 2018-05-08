CREATE PROCEDURE [dbo].[e_Edition_Select_EditionID]   
@EditionID int

AS
	select e.*, p.CustomerID 
	from 
		Edition e join 
		Publication p on e.PublicationID = P.PublicationID 
	where 
		EditionID = @EditionID and 
		e.IsDeleted=0 and 
		p.IsDeleted=0