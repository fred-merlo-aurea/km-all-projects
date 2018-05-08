CREATE PROCEDURE [dbo].[e_EditionActivityLog_Select_EditionID]   
@EditionID int
AS
	select eal.*, p.CustomerID from EditionActivityLog  eal join 
	Edition e on eal.EditionID = e.EditionID join
	Publication p on p.PublicationID = e.PublicationID
	 where 
		eal.EditionID = @EditionID and  
		eal.IsDeleted = 0