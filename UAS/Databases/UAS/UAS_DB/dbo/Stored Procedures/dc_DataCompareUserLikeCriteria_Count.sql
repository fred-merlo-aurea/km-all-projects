CREATE PROCEDURE dc_DataCompareUserLikeCriteria_Count
@dcResultQueId int
AS
BEGIN

	set nocount on

	select COUNT(DataCompareUserLikeCriteriaId) 
	from DataCompareUserLikeCriteria with(nolock) 
	where DataCompareResultQueId = @dcResultQueId

END
go