CREATE PROCEDURE [dbo].[Dashboard_getProductCounts_By_BrandID]
(
	@BrandID int
)
AS
Begin
	declare @totalcounts bigint

	select @totalcounts = ISNULL(SUM([Counts]), 0)
	FROM BrandDetails bd WITH (NOLOCK)
		JOIN Pubs P
		  ON P.PubID = bd.PubID
		JOIN dbo.[Summary_Data] s WITH (NOLOCK)
		  ON s.PubID = bd.PubID
		  AND entityName = 'Pubsubscriptions'
		  AND [Type] = 'new'
	WHERE bd.BrandID = @BrandID
	
	SELECT
	  P.PubID,
	  P.PubName,
	  P.Pubcode,
	  ISNULL(SUM([Counts]), 0) AS [Counts],
	  convert(decimal(18,2),(convert(decimal(18,2),ISNULL(SUM([Counts]), 0)) * 100) / convert(decimal(18,2),@totalcounts)) as PubPercentage
	FROM Brand b WITH (NOLOCK)
	JOIN BrandDetails bd WITH (NOLOCK)
	  ON b.BrandID = bd.BrandID
	JOIN Pubs P
	  ON P.PubID = bd.PubID
	JOIN dbo.[Summary_Data] s WITH (NOLOCK)
	  ON s.PubID = bd.PubID
	  AND entityName = 'Pubsubscriptions'
	  AND [Type] = 'new'
	WHERE bd.BrandID = @BrandID
	GROUP BY P.PubID,
			 P.PubName,
			 P.Pubcode
	ORDER BY Pubcode
End