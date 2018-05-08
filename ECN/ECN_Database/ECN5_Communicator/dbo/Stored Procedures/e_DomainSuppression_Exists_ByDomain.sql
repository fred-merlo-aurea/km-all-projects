CREATE PROCEDURE [dbo].[e_DomainSuppression_Exists_ByDomain] 
	@CustomerID int = NULL,
	@BaseChannelID int = NULL,
	@DomainSuppressionID int = NULL,
	@Domain varchar(50) = NULL
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 DomainSuppressionID 
				FROM DomainSuppression WITH (NOLOCK) 
				WHERE (CustomerID = @CustomerID OR BaseChannelID = @BaseChannelID) AND 
					DomainSuppressionID != ISNULL(@DomainSuppressionID, -1) AND 
					Domain = @Domain AND 
					IsDeleted = 0 ) SELECT 1 ELSE SELECT 0
END