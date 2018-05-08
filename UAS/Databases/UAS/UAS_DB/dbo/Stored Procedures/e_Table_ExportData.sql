CREATE PROCEDURE [dbo].[e_Table_ExportData]
	@Table varchar(50),
	@ClientID int,
	@SourceFileID int
AS	
BEGIN

	set nocount on

    IF EXISTS(SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS With(NoLock) WHERE TABLE_NAME = @Table and COLUMN_NAME like '%ClientID%')
        BEGIN
	        IF EXISTS(SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS With(NoLock) WHERE TABLE_NAME = @Table and COLUMN_NAME like '%SourceFileID%')
				BEGIN
					IF (@SourceFileID > 0)
						BEGIN
							EXEC('SELECT * FROM ' + @Table + ' With(NoLock) WHERE ClientID = ' + @ClientID + ' AND SourceFileID = ' + @SourceFileID);
						END
					ELSE
						BEGIN
							EXEC('SELECT * FROM ' + @Table + ' With(NoLock) WHERE ClientID = ' + @ClientID);
						END
				END
	        ELSE
				BEGIN
					EXEC('SELECT * FROM ' + @Table + ' With(NoLock) WHERE ClientID = ' + @ClientID);
				END
        END
    ELSE
		BEGIN
			EXEC('SELECT * FROM ' + @Table + ' With(NoLock)');
		END

END