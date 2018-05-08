CREATE proc [dbo].[e_SubscriberExtenstion_getbySubscriberID_forRecordView]
	@subscriptionID int
as
Begin
	set nocount on
	
	if exists (select top 1 subscriptionID from SubscriptionsExtension with(nolock) where  SubscriptionID = @subscriptionID)
	Begin
		declare @query varchar(max)
		DECLARE @SubExtMapperColNameInBraces VARCHAR(4000)
		DECLARE @SubExtMapperColsAliased varchar(4000)

		SELECT 
			@SubExtMapperColNameInBraces = ISNULL(@SubExtMapperColNameInBraces + ',', '') + '[' + sem.CustomField + ']',
			@SubExtMapperColsAliased = ISNULL(@SubExtMapperColsAliased + ',', '') + 'convert(varchar(2048), ISNULL( ' + sem.StandardField + ', '''')) AS [' + sem.CustomField+ ']'
		FROM 
			SubscriptionsExtensionMapper sem with(nolock) join 
			RecordViewField rvf with(nolock) on sem.SubscriptionsExtensionMapperID = rvf.SubscriptionsExtensionMapperID
		where 
			Active = 1
		order by RecordViewFieldID
			
		SET @SubExtMapperColsAliased = COALESCE( @SubExtMapperColsAliased, '')
		SET @SubExtMapperColNameInBraces = COALESCE( @SubExtMapperColNameInBraces, '')

		if(LEN(@SubExtMapperColsAliased) > 0)
		begin
		
			SET @query =	'select AdhocField, AdhocValue from ' +
							' (select ' + @SubExtMapperColsAliased + ' from SubscriptionsExtension with(nolock)  where  SubscriptionID = ' + convert(varchar(50), @subscriptionID) + ') p' +
							' UNPIVOT (AdhocValue FOR AdhocField IN (' + @SubExtMapperColNameInBraces + ') )AS unpvt;'	
		END
			
		--print @query
		exec (@query)
	End
	else
	Begin
		SELECT 
			sem.CustomField as AdhocField, '' as AdhocValue
		FROM 
			SubscriptionsExtensionMapper sem with(nolock) join 
			RecordViewField rvf with(nolock) on sem.SubscriptionsExtensionMapperID = rvf.SubscriptionsExtensionMapperID
		where 
			sem.Active = 1
		order by RecordViewFieldID	
	End
End
