CREATE PROCEDURE [dbo].[o_GetPubSubscription_Adhocs_PubSubscriptionID] 
@PubSubscriptionID int,
@PubID int
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @PubCodeID int,
	@PubStandardField varchar(255),
	@PubCustomField varchar(255)
	DECLARE @selectClause varchar(max) = 'SELECT '

	--SET @PubID = 22
	--SET @PubSubscriptionID = 23525646
	CREATE TABLE #tblPubAdhoc (PubSubscriptionID int, AdhocValue varchar(max))

	CREATE INDEX IDX_tblPubAdhoc_SubscriptionID ON #tblPubAdhoc(PubSubscriptionID)
	CREATE TABLE #colNames (col varchar(255))
	CREATE TABLE #tempPubAdhocFields (PubID int, CustomField varchar(255), StandardField varchar(255))

	INSERT INTO #tempPubAdhocFields (PubID,CustomField,StandardField)
	SELECT DISTINCT pem.PubID, pem.CustomField, pem.StandardField		
	FROM PubSubscriptionsExtensionMapper pem WITH(NOLOCK)
	WHERE pem.Active = 1 AND pem.PubID = @PubID

	DECLARE curPubAdhoc CURSOR LOCAL FAST_FORWARD FOR SELECT PubID, CustomField, StandardField from #tempPubAdhocFields
	OPEN curPubAdhoc
	FETCH NEXT FROM curPubAdhoc INTO @PubCodeID, @PubCustomField, @PubStandardField
	WHILE @@FETCH_STATUS = 0  
		BEGIN 	
			SET @selectClause += ' ' + @PubStandardField + ' as ''' + @PubCustomField + ''','

			FETCH NEXT FROM curPubAdhoc INTO @PubCodeID, @PubCustomField, @PubStandardField
		END 
	CLOSE curPubAdhoc
	DEALLOCATE curPubAdhoc		

	WHILE RIGHT(@selectClause, 1) = ','
		SET @selectClause = SUBSTRING(@selectClause, 1, LEN(@selectClause)-1)
	
	IF(LEN(@selectClause) = 6)
		BEGIN
			SET @selectClause += '*'
		END
	
	SET @selectClause += ' FROM PubSubscriptionsExtension pe WHERE pe.PubSubscriptionID = ' + CONVERT(varchar(25), @PubSubscriptionID)
	EXEC (@selectClause)

	DROP TABLE #tempPubAdhocFields
	DROP TABLE #tblPubAdhoc
	DROP TABLE #colNames

END