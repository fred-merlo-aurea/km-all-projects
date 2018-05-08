create procedure e_ProductSubscription_Select_SequenceIdWhereClause
@SequenceIdWhereClause varchar(max)
as
BEGIN
	
	SET NOCOUNT ON

	select *
	from PubSubscriptions with(nolock)
	where SequenceID in (@SequenceIdWhereClause)

END
go