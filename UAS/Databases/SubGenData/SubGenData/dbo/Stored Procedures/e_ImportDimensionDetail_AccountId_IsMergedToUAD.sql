create procedure e_ImportDimensionDetail_AccountId_IsMergedToUAD
@accountId int, 
@IsMergedToUAD bit
as
BEGIN

	set nocount on

	select idd.*
	from ImportDimensionDetail idd with(nolock)
	join ImportDimension id with(nolock) on idd.ImportDimensionId = id.ImportDimensionId
	where id.account_id = @accountId
	and id.IsMergedToUAD = @IsMergedToUAD

END