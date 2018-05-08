CREATE PROCEDURE [dbo].[e_DomainSuppression_Select_DomainSuppressionID] 
(
@DomainSuppressionID int = NULL
)
AS

SELECT * FROM DomainSuppression WITH(NOLOCK) WHERE DomainSuppressionID = @DomainSuppressionID AND IsDeleted = 0
