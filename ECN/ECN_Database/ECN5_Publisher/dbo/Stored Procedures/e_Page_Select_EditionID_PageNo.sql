CREATE PROCEDURE [dbo].[e_Page_Select_EditionID_PageNo]   
@EditionID int,
@PageNo varchar(20)  

AS
	select p.*, pb.CustomerID 
	from 
			Page p  join
			Edition e on e.EditionID = p.EditionID join
			Publication pb on pb.PublicationID = e.PublicationID
	where 
		p.EditionID = @EditionID and  
		p.PageNumber  in (select items from dbo.fn_Split(@Pageno,',')) and p.IsDeleted=0