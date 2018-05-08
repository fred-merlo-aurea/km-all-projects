create procedure e_ProductSubscription_Select_SFRecordIdentifier
@SFRecordIdentifier uniqueidentifier
as
BEGIN

	SET NOCOUNT ON

	select *
	from PubSubscriptions with(nolock)
	where SFRecordIdentifier = @SFRecordIdentifier
END
go