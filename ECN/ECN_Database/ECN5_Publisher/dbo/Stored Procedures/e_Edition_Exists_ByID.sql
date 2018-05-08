create PROCEDURE [dbo].[e_Edition_Exists_ByID]   
(
	@EditionID int = NULL,
	@CustomerID int = NULL
)

AS
	if exists (
				select top 1 e.EditionID 
				from 
					Edition e with (nolock)
					join Publication p with (nolock) on e.PublicationID = p.PublicationID
				where 
					p.CustomerID = @CustomerID AND
					e.EditionID = @EditionID AND 
					e.IsDeleted = 0
				) SELECT 1 ELSE SELECT 0