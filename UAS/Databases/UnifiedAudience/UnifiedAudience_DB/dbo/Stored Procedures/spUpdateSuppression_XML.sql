create Proc [dbo].[spUpdateSuppression_XML] 
(
	@XML xml
)
as
BEGIN
	
	SET NOCOUNT ON

	update  subscriptions
		set email = '', emailexists = 0
		where subscriptionID in 
			(
				SELECT subs.value('.', 'int' ) from @XML.nodes('XML/sID') AS x(subs) 
			)
	
	select @@ROWCOUNT

End