CREATE PROCEDURE [dbo].[Dashboard_getOriginatedPubcodeCounts_By_BrandID]
(
	@BrandID int,
	@startdate date = '',
	@endDate date = ''
)
AS
Begin

	if @startdate = null or @startdate = ''
	Begin
		set @startdate = DateAdd(YEAR, -1, getdate()) 
	End
	
	if @endDate = null or @endDate = ''
	Begin
		set @endDate = GETDATE()
	End
	
	declare @totalcounts bigint

	select @totalcounts = ISNULL(SUM([Counts]), 0)
	FROM BrandDetails bd WITH (NOLOCK)
		JOIN Pubs P
		  ON P.PubID = bd.PubID
		JOIN dbo.[Summary_Data] s WITH (NOLOCK)
		  ON s.PubID = bd.PubID
		  AND entityName = 'subscriptions'
		  AND [Type] = 'new'  and s.Date between @startdate and @endDate
	WHERE bd.BrandID = @BrandID
	
	SELECT
	  P.PubID,
	  P.PubName,
	  P.Pubcode,
	  ISNULL(SUM([Counts]), 0) AS [Counts],
	  convert(decimal(18,2),(convert(decimal(18,2),ISNULL(SUM([Counts]), 0)) * 100) / convert(decimal(18,2),@totalcounts)) as CountsPercentage
	FROM Brand b WITH (NOLOCK)
	JOIN BrandDetails bd WITH (NOLOCK)
	  ON b.BrandID = bd.BrandID
	JOIN Pubs P
	  ON P.PubID = bd.PubID
	JOIN dbo.[Summary_Data] s WITH (NOLOCK)
	  ON s.PubID = bd.PubID
	  AND entityName = 'subscriptions'
	  AND [Type] = 'new'  and s.Date between @startdate and @endDate
	WHERE bd.BrandID = @BrandID
	GROUP BY P.PubID,
			 P.PubName,
			 P.Pubcode
	ORDER BY Pubcode
End