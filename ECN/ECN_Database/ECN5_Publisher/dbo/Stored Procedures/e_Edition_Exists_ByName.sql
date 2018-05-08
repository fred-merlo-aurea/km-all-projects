CREATE PROCEDURE [dbo].[e_Edition_Exists_ByName]   
(
	@PublicationID int = NULL,
	@CustomerID int = NULL,
	@EditionName varchar(100)= NULL,
	@EditionID int = NULL  
)

AS
	if exists (
				select top 1 e.EditionID 
				from 
					Edition e with (nolock)
					join Publication p with (nolock) on e.PublicationID = p.PublicationID
				where 
					e.EditionID <> @EditionID AND
					e.EditionName = @EditionName and 
					p.CustomerID = @CustomerID AND
					e.PublicationID = @PublicationID AND 
					e.IsDeleted = 0
				) SELECT 1 ELSE SELECT 0