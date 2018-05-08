CREATE PROCEDURE [dbo].[Dashboard_getProductCounts_By_PubTypeID]
(
	@PubtypeID int,
	@BrandID int = 0
)
AS
Begin
	declare @totalcounts bigint

	if (@brandID = 0)
	Begin
		select @totalcounts = ISNULL(SUM([Counts]), 0)
		FROM Pubs P
		JOIN dbo.[Summary_Data] s WITH (NOLOCK)
		  ON p.PubID = s.PubID
		  AND entityName = 'Pubsubscriptions'
		  AND [Type] = 'new'
		WHERE p.PubtypeID = @PubtypeID

		SELECT
		  P.PubID,
		  P.PubName,
		  P.Pubcode,
		  ISNULL(SUM([Counts]), 0) AS [Counts],
		  convert(decimal(18,2),(convert(decimal(18,2),ISNULL(SUM([Counts]), 0)) * 100) / convert(decimal(18,2),@totalcounts)) as PubPercentage
		FROM Pubs P
		JOIN dbo.[Summary_Data] s WITH (NOLOCK)
		  ON p.PubID = s.PubID
		  AND entityName = 'Pubsubscriptions'
		  AND [Type] = 'new'
		WHERE p.PubtypeID = @PubtypeID
		GROUP BY P.PubID,
				 P.PubName,
				 P.Pubcode
		ORDER BY Pubcode
	end
	Else
	Begin
		select @totalcounts = ISNULL(SUM([Counts]), 0)
		FROM dbo.[Summary_Data] s WITH (NOLOCK)
		JOIN Pubs P WITH (NOLOCK) ON p.PubID = s.PubID
		join brandDetails bd on p.pubID = bd.pubID 
		WHERE p.PubtypeID = @PubtypeID AND bd.brandID = @BrandID and entityName = 'Pubsubscriptions' AND [Type] = 'new'

		SELECT
		  P.PubID,
		  P.PubName,
		  P.Pubcode,
		  ISNULL(SUM([Counts]), 0) AS [Counts],
		  convert(decimal(18,2),(convert(decimal(18,2),ISNULL(SUM([Counts]), 0)) * 100) / convert(decimal(18,2),@totalcounts)) as PubPercentage
		FROM dbo.[Summary_Data] s WITH (NOLOCK) 
		JOIN Pubs P WITH (NOLOCK) ON p.PubID = s.PubID
		join brandDetails bd on p.pubID = bd.pubID 
		
		WHERE		
				p.PubtypeID = @PubtypeID and
				bd.brandID = @BrandID and
				entityName = 'Pubsubscriptions'
				AND [Type] = 'new'
		GROUP BY P.PubID,
				 P.PubName,
				 P.Pubcode
		ORDER BY Pubcode
	End
End
GO