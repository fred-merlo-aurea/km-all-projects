CREATE PROCEDURE [dbo].[e_UADTable_ExportData]
	@Table varchar(100),
	@PubCode varchar(50)
AS	
BEGIN
	
	SET NOCOUNT ON
	
    IF EXISTS(SELECT * FROM sys.columns WHERE (UPPER([name]) = 'PUBCODE' OR UPPER([name]) = 'PUBID') AND [object_id] = OBJECT_ID(@Table))
		BEGIN    
			IF (@PubCode != '')
				BEGIN
					DECLARE @PubID int = (SELECT PubID FROM Pubs With(NoLock) WHERE PubCode = @PubCode)
					IF EXISTS (SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS With(NoLock) WHERE TABLE_NAME = @Table and COLUMN_NAME like '%PubID%')
						BEGIN
							EXEC('SELECT * FROM ' + @Table + ' WHERE PubID = ' + @PubID);
						END
					ELSE
						BEGIN
							IF EXISTS (SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS With(NoLock) WHERE TABLE_NAME = @Table and COLUMN_NAME like '%PubCode%')
								BEGIN
									EXEC('SELECT * FROM ' + @Table + ' WHERE PubCode = ' + @PubCode);
								END
							ELSE
								BEGIN
									EXEC('SELECT * FROM ' + @Table);
								END
						END
				END
			ELSE
				BEGIN
					EXEC('SELECT * FROM ' + @Table);
				END
		END
    ELSE
		BEGIN
			BEGIN
				EXEC('SELECT * FROM ' + @Table);
			END
		END

END