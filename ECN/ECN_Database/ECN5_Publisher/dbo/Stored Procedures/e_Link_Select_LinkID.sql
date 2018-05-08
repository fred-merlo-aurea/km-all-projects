create PROCEDURE [dbo].[e_Link_Select_LinkID]   
@LinkID int

AS
	select l.*, pb.CustomerID 
	from	Link l join 
			Page p on l.pageID = p.PageID join
			Edition e on e.EditionID = p.EditionID join
			Publication pb on pb.PublicationID = e.PublicationID
	where	l.linkID = @LinkID and 
			l.IsDeleted=0