create procedure o_SubscriberTransformed_GetPubCodes
@ProcessCode varchar(50)
as
BEGIN
	
	SET NOCOUNT ON

	select distinct PubCode
	from SubscriberTransformed with(nolock)
	where ProcessCode = @ProcessCode
		and PubCode is not null
		and LEN(PubCode) > 1

END
go