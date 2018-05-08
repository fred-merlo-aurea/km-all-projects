CREATE PROCEDURE [dbo].[e_Link_Select_PageID]   
@PageID int

AS
	select l.*, pb.CustomerID 
	from	Link l join 
			Page p on l.pageID = p.PageID join
			Edition e on e.EditionID = p.EditionID join
			Publication pb on pb.PublicationID = e.PublicationID
	where	l.PageID = @PageID and 
			l.IsDeleted=0