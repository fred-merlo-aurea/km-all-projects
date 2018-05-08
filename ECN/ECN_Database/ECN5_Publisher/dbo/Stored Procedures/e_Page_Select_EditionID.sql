create PROCEDURE [dbo].[e_Page_Select_EditionID]   
@EditionID int

AS
	select p.*, pb.CustomerID 
	from 
			Page p  join
			Edition e on e.EditionID = p.EditionID join
			Publication pb on pb.PublicationID = e.PublicationID
	where 
		p.EditionID = @EditionID  and p.IsDeleted=0