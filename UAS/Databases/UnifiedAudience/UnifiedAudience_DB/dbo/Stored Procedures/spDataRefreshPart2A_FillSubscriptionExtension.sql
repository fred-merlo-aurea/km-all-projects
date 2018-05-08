CREATE proc [dbo].[spDataRefreshPart2A_FillSubscriptionExtension]
as
BEGIN

	SET NOCOUNT ON    

	DECLARE @ColumnName VARCHAR(255)
	DECLARE @FieldName VARCHAR(10)
	DECLARE @ColumnNamesCsv AS VARCHAR(MAX)
	DECLARE @FieldNamesCsv AS VARCHAR(MAX)
	DECLARE @WhereOneIsNotNull AS VARCHAR(MAX)
	
	--copy data out of the old table into the new extenstion table
	DECLARE c CURSOR LOCAL FAST_FORWARD FOR SELECT CustomField, StandardField FROM SubscriptionsExtensionMapper
	OPEN c
	FETCH NEXT FROM c INTO @ColumnName, @FieldName
	WHILE @@FETCH_STATUS = 0  
		BEGIN 
	
			SET @ColumnNamesCsv = ISNULL(@ColumnNamesCsv + ', [' + replace(@ColumnName,' ', '') + ']', '[' + replace(@ColumnName,' ', '')+ ']')
			SET @FieldNamesCsv = ISNULL(@FieldNamesCsv + ', ' + @FieldName, @FieldName)
		
			IF(@WhereOneIsNotNull IS NULL)
			BEGIN
				SET @WhereOneIsNotNull = '[' + replace(@ColumnName,' ', '') + '] IS NOT NULL'
			END
			ELSE
			BEGIN
				SET @WhereOneIsNotNull = @WhereOneIsNotNull + ' OR [' + replace(@ColumnName,' ', '') + '] IS NOT NULL'
			END
	
		FETCH NEXT FROM c INTO @ColumnName, @FieldName 
		END 
	CLOSE c
	DEALLOCATE c
	
	IF(@ColumnNamesCsv IS NOT NULL 
		AND @FieldNamesCsv IS NOT NULL 
		AND @WhereOneIsNotNull IS NOT NULL)
		BEGIN
	
			EXEC ('INSERT INTO SubscriptionsExtension (SubscriptionID, ' + @FieldNamesCsv + ') 
			SELECT SubscriptionID, ' + @ColumnNamesCsv + '
			FROM IncomingData I
			JOIN Subscriptions s ON I.IGRP_NO = s.IGRP_NO
			WHERE I.IGRP_RANK = ''M'' 
			AND (' + @WhereOneIsNotNull + ')')
		
		END
END