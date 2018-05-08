CREATE PROCEDURE [dbo].[e_SubscriberDemographicOriginal_SelectForSORecordIdentifier]
	@SORecordIdentifierList varchar(max)
AS
BEGIN

	SET NOCOUNT ON

	BEGIN
		CREATE TABLE #SORecordIdentifier
		(  
			RowID int IDENTITY(1, 1)
		  ,[SORecordIdentifier] varchar(100)
		)
		CREATE NONCLUSTERED INDEX IDX_SORecordIdentifier_SORecordIdentifier ON #SORecordIdentifier(SORecordIdentifier)
		DECLARE @docHandle int
		EXEC sp_xml_preparedocument @docHandle OUTPUT, @SORecordIdentifierList  
		INSERT INTO #SORecordIdentifier 
		SELECT [SORecordIdentifier]
		FROM OPENXML(@docHandle,N'/XML/SORecordIdentifier')
		WITH
		(
			[SORecordIdentifier] nvarchar(256) 'ID'
		)
		EXEC sp_xml_removedocument @docHandle
	END

	Select sdo.* 
	from SubscriberDemographicOriginal sdo WITH(NOLOCK)
		join #SORecordIdentifier sori on sdo.SORecordIdentifier = sori.SORecordIdentifier

	Drop table #SORecordIdentifier

END