CREATE PROCEDURE [dbo].[AP_Maintenance_RebuildIndexes] @MaxLogicalfrag DECIMAL = 15.0,  @MaxScanDensity DECIMAL = 90.0
AS
-- Declare variables
SET NOCOUNT ON
DECLARE @tablename VARCHAR (128)
DECLARE @tableschema varchar(128)
DECLARE @execstr   VARCHAR (255)
DECLARE @objectid  INT
DECLARE @indexid   INT
DECLARE @indexname   VARCHAR (255)
DECLARE @LogicalFrag    DECIMAL
DECLARE @ScanDensity    DECIMAL

-- Declare cursor
DECLARE tables CURSOR FOR
   SELECT TABLE_NAME, TABLE_SCHEMA
   FROM INFORMATION_SCHEMA.TABLES
   WHERE TABLE_TYPE = 'BASE TABLE' --AND TABLE_NAME  NOT IN ('BLAST', 'EMAILACTIVITYLOG', 'EMAILDATAVALUES', 'EMAILs', 'EMAILGROUPS')

-- Create the table
CREATE TABLE #fraglist (
   OwnerName varchar(128),
   ObjectName CHAR (255),
   ObjectId INT,
   IndexName CHAR (255),
   IndexId INT,
   Lvl INT,
   CountPages INT,
   CountRows INT,
   MinRecSize INT,
   MaxRecSize INT,
   AvgRecSize INT,
   ForRecCount INT,
   Extents INT,
   ExtentSwitches INT,
   AvgFreeBytes INT,
   AvgPageDensity INT,
   ScanDensity DECIMAL,
   BestCount INT,
   ActualCount INT,
   LogicalFrag DECIMAL,
   ExtentFrag DECIMAL)

-- Open the cursor

OPEN tables

-- Loop through all the tables in the database

FETCH NEXT
   FROM tables
   INTO @tablename, @tableschema 

WHILE @@FETCH_STATUS = 0
BEGIN

-- Do the showcontig of all indexes of the table
   INSERT INTO #fraglist (
	ObjectName, ObjectId, IndexName, IndexId, Lvl, CountPages, CountRows, MinRecSize, MaxRecSize, AvgRecSize, ForRecCount, Extents, ExtentSwitches, AvgFreeBytes, AvgPageDensity, ScanDensity, BestCount, ActualCount, LogicalFrag, ExtentFrag
	)
   EXEC ('DBCC SHOWCONTIG (''' + @tableschema + '.' + @tablename + ''') 
      WITH FAST, TABLERESULTS, ALL_INDEXES, NO_INFOMSGS')

   UPDATE #fraglist
   SET OwnerName = @tableschema
   WHERE OwnerName IS NULL

   FETCH NEXT
      FROM tables
      INTO @tablename, @tableschema

END

-- Close and deallocate the cursor
CLOSE tables
DEALLOCATE tables

  SELECT ObjectName, OwnerName, ObjectId, IndexId, IndexName, LogicalFrag, ScanDensity
        FROM #fraglist f    
        WHERE (LogicalFrag >= @MaxLogicalfrag
                OR ScanDensity < @MaxScanDensity)
                AND INDEXPROPERTY (ObjectId, IndexName, 'IndexDepth') > 0
                AND NOT EXISTS(SELECT ObjectID 
                                FROM #fraglist i1 
                                WHERE IndexId = 1 
                                AND (LogicalFrag >= @MaxLogicalfrag
                                OR ScanDensity < @MaxScanDensity)
				AND i1.ObjectID = f.ObjectID)
        UNION 
        SELECT ObjectName, OwnerName, ObjectId, IndexId, IndexName, LogicalFrag, ScanDensity
        FROM #fraglist f    
        WHERE (LogicalFrag >= @MaxLogicalfrag
                OR ScanDensity < @MaxScanDensity)
                AND INDEXPROPERTY (ObjectId, IndexName, 'IndexDepth') > 0
                AND IndexId = 1 
                AND (LogicalFrag >= @MaxLogicalfrag
                OR ScanDensity < @MaxScanDensity)
        ORDER BY ObjectName, IndexId
        

-- Declare cursor for list of indexes to be defragged
-- If clustered index meets the criteria, just reindex the entire table
-- If clustered index does not require a reindex, only return the non-clustered
-- which meet the criteria.
DECLARE indexes CURSOR FOR
        SELECT ObjectName, OwnerName, ObjectId, IndexId, IndexName, LogicalFrag, ScanDensity
        FROM #fraglist f    
        WHERE (LogicalFrag >= @MaxLogicalfrag
                OR ScanDensity < @MaxScanDensity)
                AND INDEXPROPERTY (ObjectId, IndexName, 'IndexDepth') > 0
                AND NOT EXISTS(SELECT ObjectID 
                                FROM #fraglist i1 
                                WHERE IndexId = 1 
                                AND (LogicalFrag >= @MaxLogicalfrag
                                OR ScanDensity < @MaxScanDensity)
				AND i1.ObjectID = f.ObjectID)
        UNION 
        SELECT ObjectName, OwnerName, ObjectId, IndexId, IndexName, LogicalFrag, ScanDensity
        FROM #fraglist f    
        WHERE (LogicalFrag >= @MaxLogicalfrag
                OR ScanDensity < @MaxScanDensity)
                AND INDEXPROPERTY (ObjectId, IndexName, 'IndexDepth') > 0
                AND IndexId = 1 
                AND (LogicalFrag >= @MaxLogicalfrag
                OR ScanDensity < @MaxScanDensity)
        ORDER BY ObjectName, IndexId

-- Open the cursor

OPEN indexes

-- loop through the indexes
FETCH NEXT
   FROM indexes
   INTO @tablename, @tableschema, @objectid, @indexid, @indexname, @LogicalFrag, @ScanDensity

WHILE @@FETCH_STATUS = 0
BEGIN
		
        IF @indexid = 1
        BEGIN

			PRINT 'print(''Executing DBCC DBREINDEX (['+ RTRIM(@tableschema) + '.' + RTRIM(@tablename) + ']) - fragmentation currently '
				+ RTRIM(CONVERT(varchar(15),@LogicalFrag)) + '% and Scan Density currently '
				+ RTRIM(CONVERT(VARCHAR(15), @ScanDensity) + ''')')

			SELECT @execstr = 'DBCC DBREINDEX (['+ RTRIM(@tableschema) + '.' + RTRIM(@tablename) + '])'
			print @execstr
			--EXEC (@execstr)
			print 'go'

        END

        IF @indexid > 1

        BEGIN
			PRINT 'print(''Executing DBCC DBREINDEX (['+ RTRIM(@tableschema) + '.' + RTRIM(@tablename) + '],' 
				+ RTRIM(@indexid) + ') - fragmentation currently '
				+ RTRIM(CONVERT(VARCHAR(15),@LogicalFrag)) + '% and Scan Density currently '
				+ RTRIM(CONVERT(VARCHAR(15), @ScanDensity)+ ''')')

			SELECT @execstr = 'DBCC DBREINDEX (['+ RTRIM(@tableschema) + '.'  + RTRIM(@tablename) + '],' + RTRIM(@indexname) + ')'
			print @execstr
			--EXEC (@execstr)
			print 'go'

        END

   FETCH NEXT
      FROM indexes
      INTO @tablename, @tableschema, @objectid, @indexid, @indexname, @LogicalFrag, @ScanDensity

END

-- Close and deallocate the cursor

CLOSE indexes
DEALLOCATE indexes
