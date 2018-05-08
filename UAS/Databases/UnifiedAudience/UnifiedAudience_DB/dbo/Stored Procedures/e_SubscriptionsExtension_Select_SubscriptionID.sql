CREATE PROCEDURE [dbo].[e_SubscriptionsExtension_Select_SubscriptionID]
		@subscriptionID int	
AS
BEGIN
	
	SET NOCOUNT ON
	
	declare @query varchar(max)
	DECLARE @SubExtMapperColNameInBraces VARCHAR(4000)
	DECLARE @SubExtMapperColsAliased varchar(4000)

	SELECT @SubExtMapperColNameInBraces = ISNULL(@SubExtMapperColNameInBraces + ',', '') + '[' + CustomField + ']',
			@SubExtMapperColsAliased = ISNULL(@SubExtMapperColsAliased + ',', '') + 'convert(varchar(2048), ISNULL( ' + StandardField + ', '''')) AS [' + CustomField+ ']'
	FROM SubscriptionsExtensionMapper with (nolock)  
	where Active = 1
			
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