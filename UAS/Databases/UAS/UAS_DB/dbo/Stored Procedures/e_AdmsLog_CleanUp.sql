CREATE PROCEDURE [dbo].[e_AdmsLog_CleanUp]
	@ClientId int,
	@IsADMS bit = 'true'
AS
	DECLARE @FileStatusCodeTypeId int = (Select CodeTypeId from UAD_Lookup..CodeType WITH(NOLOCK) where CodeTypeName = 'File Status')
	DECLARE @AdmsStepCodeTypeId int = (Select CodeTypeId from UAD_Lookup..CodeType WITH(NOLOCK) where CodeTypeName = 'ADMS Step')
	DECLARE @ProcessingStatusCodeTypeId int = (Select CodeTypeId from UAD_Lookup..CodeType WITH(NOLOCK) where CodeTypeName = 'Processing Status')

	DECLARE @CodeIdQued int = (Select CodeId from UAD_Lookup..Code WITH(NOLOCK) where CodeTypeId = @ProcessingStatusCodeTypeId and CodeName = 'Qued')
	DECLARE @CodeIdComp int = (Select CodeId from UAD_Lookup..Code WITH(NOLOCK) where CodeTypeId = @ProcessingStatusCodeTypeId and CodeName = 'Completed')
	DECLARE @CodeIdWatch int = (Select CodeId from UAD_Lookup..Code WITH(NOLOCK) where CodeTypeId = @ProcessingStatusCodeTypeId and CodeName = 'Watching for File')
	DECLARE @CodeIdCust int = (Select CodeId from UAD_Lookup..Code WITH(NOLOCK) where CodeTypeId = @ProcessingStatusCodeTypeId and CodeName = 'Custom File Processed')

	DECLARE @FailedFileStatusCodeId int = (Select CodeId from UAD_Lookup..Code WITH(NOLOCK) where CodeTypeId = @FileStatusCodeTypeId and CodeName = 'Failed')
	DECLARE @ProcessedAdmsStepCodeId int = (Select CodeId from UAD_Lookup..Code WITH(NOLOCK) where CodeTypeId = @AdmsStepCodeTypeId and CodeName = 'Processed')
	DECLARE @CompletedProcessingStatusCodeId int = (Select CodeId from UAD_Lookup..Code WITH(NOLOCK) where CodeTypeId = @ProcessingStatusCodeTypeId and CodeName = 'Completed')

	CREATE table #Failed (AdmsLogId int)
	DECLARE @SQL varchar(max) = ''
	DECLARE @LiveLinkedServer varchar(max) = ''
	if (@@SERVERNAME = 'DBSERVER1')
		Set @LiveLinkedServer = '[10.10.41.198].'

	if (@IsADMS = 'true')
	BEGIN	
		set @SQL = 'Insert into #Failed(AdmsLogId)
						Select a.AdmsLogId 
						from UAS..ADMSLog a WITH(NOLOCK)
							join UAS..SourceFile sf WITH(NOLOCK) on a.SourceFileId = sf.SourceFileId
							join ' + @LiveLinkedServer + 'KMPlatform.dbo.Service s WITH(NOLOCK) on sf.ServiceID = s.ServiceID
							join ' + @LiveLinkedServer + 'KMPlatform.dbo.ServiceFeature f WITH(NOLOCK) on sf.ServiceFeatureID = f.ServiceFeatureID
						where a.ClientID = ' + Cast(@ClientId as varchar(10)) + '
							and ProcessingStatusId not in (' + Cast(@CodeIdQued as varchar(10)) + ',' + Cast(@CodeIdComp as varchar(10)) + ',' + Cast(@CodeIdWatch as varchar(10)) + ',' + Cast(@CodeIdCust as varchar(10)) + ')
							and (f.SFCode != ''UADAPI'')'
	END
	ELSE
	BEGIN
		set @SQL = 'Insert into #Failed(AdmsLogId)
						Select a.AdmsLogId 
						from UAS..ADMSLog a WITH(NOLOCK)
							join UAS..SourceFile sf WITH(NOLOCK) on a.SourceFileId = sf.SourceFileId
							join ' + @LiveLinkedServer + 'KMPlatform.dbo.Service s WITH(NOLOCK) on sf.ServiceID = s.ServiceID
							join ' + @LiveLinkedServer + 'KMPlatform.dbo.ServiceFeature f WITH(NOLOCK) on sf.ServiceFeatureID = f.ServiceFeatureID
						where a.ClientID = ' + Cast(@ClientId as varchar(10)) + '
							and ProcessingStatusId not in (' + Cast(@CodeIdQued as varchar(10)) + ',' + Cast(@CodeIdComp as varchar(10)) + ',' + Cast(@CodeIdWatch as varchar(10)) + ',' + Cast(@CodeIdCust as varchar(10)) + ')
							and (s.ServiceCode = ''UADFILEMAPPER'' and f.SFCode = ''UADAPI'')'
	END

	exec(@SQL)
	--print(@SQL)

	Update a 
	set FileEnd = a.DateUpdated,
		FileStatusId = @FailedFileStatusCodeId,
		AdmsStepId = @ProcessedAdmsStepCodeId,
		ProcessingStatusId = @CompletedProcessingStatusCodeId
	from UAS..ADMSLog a 
		join #Failed f on a.AdmsLogId = f.AdmsLogId

	drop table #Failed