CREATE PROCEDURE [dbo].[e_Rule_Exists_ByID]   
(
	@RuleID int = NULL,
	@CustomerID int = NULL
)

AS
	if exists (
				select top 1 r.RuleID
				from 
					Rules r  join
					Edition e with (nolock) on r.EditionID = e.EditionID
					join Publication pb with (nolock) on e.PublicationID = pb.PublicationID
				where 
					pb.CustomerID = @CustomerID AND
					r.RuleID = @RuleID AND 
					r.IsDeleted = 0
				) SELECT 1 ELSE SELECT 0