create PROCEDURE [dbo].[e_EditionHistory_Exists_ByID]   
(
	@EditionHistoryID int = NULL
)

AS
	if exists (
				select top 1 EditionHistoryID
				from 
					EditionHistory 
				where 
					EditionHistoryID = @EditionHistoryID AND 
					IsDeleted = 0
				) SELECT 1 ELSE SELECT 0