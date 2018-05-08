CREATE PROCEDURE [dbo].[e_EditionHistory_Select_EditionID]   
@EditionID int

AS
	select eh.*, p.CustomerID from EditionHistory eh join 
	Edition e on eh.EditionID = e.EditionID join
	Publication p on p.PublicationID = e.PublicationID
	where 
		eh.EditionID = @EditionID and 
		isnull(ArchievedDate,'') <> '' 
		and isnull(DeActivatedDate,'') = '' 
		and eh.IsDeleted=0