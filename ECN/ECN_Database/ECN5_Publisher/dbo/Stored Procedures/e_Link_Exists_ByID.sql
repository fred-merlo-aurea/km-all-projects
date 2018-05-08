create PROCEDURE [dbo].[e_Link_Exists_ByID]   
(
	@LinkID int = NULL,
	@CustomerID int = NULL
)

AS
	if exists (
				select top 1 l.LinkID 
				from 
					Link l join 
					Page p on l.pageID = p.PageID join
					Edition e with (nolock) on p.EditionID = e.EditionID
					join Publication pb with (nolock) on e.PublicationID = pb.PublicationID
				where 
					pb.CustomerID = @CustomerID AND
					l.LinkID = @LinkID AND 
					l.IsDeleted = 0
				) SELECT 1 ELSE SELECT 0