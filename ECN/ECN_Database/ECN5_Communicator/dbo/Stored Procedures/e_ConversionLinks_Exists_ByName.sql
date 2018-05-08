CREATE PROCEDURE [dbo].[e_ConversionLinks_Exists_ByName] 
	@LayoutID int = NULL,
	@CustomerID int = NULL,
	@LinkID int = NULL,
	@LinkName varchar(50) = NULL
AS     
BEGIN     		
	IF EXISTS (
				SELECT TOP 1 cl.LinkID 
				FROM 
					ConversionLinks cl with (nolock)
					join Layout l with (nolock) on cl.LayoutID = l.LayoutID 
				WHERE 
					l.CustomerID = @CustomerID AND 
					cl.LinkID != ISNULL(@LinkID, -1) AND 
					cl.LayoutID = @LayoutID AND 
					cl.LinkName = @LinkName AND 
					cl.IsDeleted = 0 AND
					l.IsDeleted = 0
				) SELECT 1 ELSE SELECT 0
END
