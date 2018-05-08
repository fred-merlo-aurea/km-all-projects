CREATE PROCEDURE dc_DataCompareUserMatchCriteria_Count
@dcResultQueId int
AS
BEGIN

	set nocount on

	select COUNT(DataCompareUserMatchCriteriaId) from DataCompareUserMatchCriteria with(nolock) where DataCompareResultQueId = @dcResultQueId

END
go