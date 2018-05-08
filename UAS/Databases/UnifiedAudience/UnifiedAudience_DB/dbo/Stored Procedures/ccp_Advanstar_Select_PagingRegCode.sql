﻿CREATE PROCEDURE [dbo].[ccp_Advanstar_Select_PagingRegCode]
@CurrentPage int,
@PageSize int
AS
BEGIN

	set nocount on
	
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);
	
	WITH TempResult as (
	
	SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY person_id) as 'RowNum', * FROM
	tempAdvanstarRegCodeFinal  WITH (NOLOCK)
	)
	SELECT top (@LastRec-1) PubCode, Person_ID, RegCode 
	FROM TempResult  WITH (NOLOCK)
	WHERE RowNum > @FirstRec 
	AND RowNum < @LastRec

END
GO