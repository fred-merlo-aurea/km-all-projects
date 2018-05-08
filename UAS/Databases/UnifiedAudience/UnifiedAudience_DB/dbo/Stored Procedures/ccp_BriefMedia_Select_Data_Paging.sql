CREATE PROCEDURE [dbo].[ccp_BriefMedia_Select_Data_Paging]
@CurrentPage int,
@PageSize int
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);

	WITH TempResult as
	(
		SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY DrupalID) as 'RowNum', *
			FROM tempBriefMediaBMWUFinal  WITH (NOLOCK)
	)
	
	SELECT top (@LastRec-1) *
	FROM TempResult  WITH (NOLOCK)
	WHERE RowNum > @FirstRec 
		AND RowNum < @LastRec
	SET NOCOUNT OFF
END
GO