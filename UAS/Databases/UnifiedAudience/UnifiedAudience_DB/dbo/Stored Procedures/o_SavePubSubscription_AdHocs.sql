CREATE PROCEDURE [dbo].[o_SavePubSubscription_AdHocs] 
@PubSubscriptionID int,
@PubID int,
@AdHocs TEXT
AS
BEGIN

	SET NOCOUNT ON

	--DECLARE @AdHocs varchar(max) = '<XML><AdHoc><Name>excl_inv</Name><Value>0011</Value></AdHoc></XML>'
	--DECLARE @PubSubscriptionID int, @PubID int

	BEGIN --Parse AdHocs
		CREATE TABLE #AdHoc
		(  
		  [Name] nvarchar(256)
		  ,[Value] nvarchar(256)
		)

		DECLARE @docHandle int

		EXEC sp_xml_preparedocument @docHandle OUTPUT, @AdHocs  
		INSERT INTO #AdHoc 
		(
			 [Name], [Value]
		)  
		SELECT [Name], [Value]
		FROM OPENXML(@docHandle,N'/XML/AdHoc')
		WITH
		(
			[Name] nvarchar(256) 'Name',
			[Value] nvarchar(1500) 'Value'
		)
		EXEC sp_xml_removedocument @docHandle
	END
	IF NOT EXISTS (SELECT TOP 1 * FROM PubSubscriptionsExtension WHERE PubSubscriptionID = @PubSubscriptionID)
		BEGIN
			INSERT INTO PubSubscriptionsExtension (PubSubscriptionID, DateCreated, CreatedByUserID)
			VALUES(@PubSubscriptionID, GETDATE(), 1)
		END
	DECLARE @PubCodeID int,
	@PubStandardField varchar(255),
	@PubCustomField varchar(255),
	@PubValue varchar(255)
	DECLARE @updateClause varchar(max) = 'UPDATE PubSubscriptionsExtension SET '

	--SET @PubID = 22
	--SET @PubSubscriptionID = 23525646

	CREATE TABLE #tblPubAdhoc (PubSubscriptionID int, AdhocValue varchar(max))

	CREATE INDEX IDX_tblPubAdhoc_SubscriptionID ON #tblPubAdhoc(PubSubscriptionID)
	CREATE TABLE #colNames (col varchar(255))
	CREATE TABLE #tempPubAdhocFields (PubID int, CustomField varchar(255), StandardField varchar(255), Value varchar(255))

	INSERT INTO #tempPubAdhocFields (PubID,CustomField,StandardField, Value)
	SELECT DISTINCT pem.PubID, pem.CustomField, pem.StandardField, a.Value		
	FROM PubSubscriptionsExtensionMapper pem WITH(NOLOCK)
		JOIN #AdHoc a ON a.Name = pem.CustomField
	WHERE pem.Active = 1 AND pem.PubID = @PubID

	DECLARE curPubAdhoc CURSOR FOR SELECT PubID, CustomField, StandardField, Value from #tempPubAdhocFields
	OPEN curPubAdhoc
	FETCH NEXT FROM curPubAdhoc INTO @PubCodeID, @PubCustomField, @PubStandardField, @PubValue
	WHILE @@FETCH_STATUS = 0  
	BEGIN 	
		SET @updateClause += @PubStandardField + ' = ''' + @PubValue + ''','
		FETCH NEXT FROM curPubAdhoc INTO @PubCodeID, @PubCustomField, @PubStandardField, @PubValue
	END 
	CLOSE curPubAdhoc
	DEALLOCATE curPubAdhoc		

	WHILE RIGHT(@updateClause, 1) = ','
		SET @updateClause = SUBSTRING(@updateClause, 1, LEN(@updateClause)-1)
	
	SET @updateClause += ' WHERE PubSubscriptionID = ' + CONVERT(varchar(25), @PubSubscriptionID)
	EXEC (@updateClause)

	DROP TABLE #tempPubAdhocFields
	DROP TABLE #colNames
	DROP TABLE #AdHoc
	DROP TABLE #tblPubAdhoc

END