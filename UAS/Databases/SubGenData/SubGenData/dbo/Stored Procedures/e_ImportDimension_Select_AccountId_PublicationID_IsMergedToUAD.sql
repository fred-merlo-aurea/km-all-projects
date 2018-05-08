create procedure e_ImportDimension_Select_AccountId_PublicationID_IsMergedToUAD
@accountId int, 
@PublicationID int, 
@IsMergedToUAD bit
as
BEGIN

	set nocount on

	select id.*
	from ImportDimension id with(nolock)
	where id.account_id = @accountId
	and id.PublicationID = @PublicationID
	and id.IsMergedToUAD = @IsMergedToUAD

END