CREATE Procedure ccp_Northstar_Get_WEB_Person_Group_Data_Paging
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
		SELECT	DISTINCT ROW_NUMBER() OVER (ORDER BY s.GlobalUserKey) as 'RowNum',
				*
		FROM tempNorthstarWebPersonGroup s With(NoLock)		
	)
	
	SELECT top (@LastRec-1) *
	FROM TempResult
	WHERE RowNum > @FirstRec 
	AND RowNum < @LastRec
	-- Turn NOCOUNT back OFF
	SET NOCOUNT OFF

END
GO