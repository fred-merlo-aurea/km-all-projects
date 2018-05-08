CREATE PROCEDURE [dbo].[e_Page_Exists_ByID]   
(
	@PageID int = NULL,
	@CustomerID int = NULL
)

AS
	if exists (
				select top 1 p.PageID
				from 
					Page p  join
					Edition e with (nolock) on p.EditionID = e.EditionID
					join Publication pb with (nolock) on e.PublicationID = pb.PublicationID
				where 
					pb.CustomerID = @CustomerID AND
					p.PageID = @PageID AND 
					p.IsDeleted = 0
				) SELECT 1 ELSE SELECT 0