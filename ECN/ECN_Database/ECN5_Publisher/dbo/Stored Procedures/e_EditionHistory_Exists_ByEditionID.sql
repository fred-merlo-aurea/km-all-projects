create PROCEDURE [dbo].[e_EditionHistory_Exists_ByEditionID]   
(
	@EditionID int = NULL
)

AS
	if exists (
				select top 1 EditionHistoryID
				from 
					EditionHistory 
				where 
					editionID = @EditionID AND 
					isnull(ArchievedDate,'') <> '' AND 
					isnull(DeActivatedDate,'') = '' AND 
					IsDeleted = 0
				) SELECT 1 ELSE SELECT 0