CREATE PROCEDURE [dbo].[TableSpaceUsed]
AS
Begin
	-- Create the temporary table...
	CREATE TABLE #tblResults
	(
	   [name]  nvarchar(40),
	   [rows]   int,
	   [reserved_kb]   varchar(18),
	   [reserved_mb]   int default(0),
	   [reserved_gb]   int default(0),
	   [data_kb]   varchar(18),
	   [data_mb]   int default(0),
	   [data_gb]   int default(0),
	   [index_size_kb]   varchar(18),
	   [index_size_mb]   int default(0),
	   [index_size_gb]   int default(0),
	   [unused_kb]   varchar(18),
	   [unused_mb]   int default(0),
	   [unused_gb]   int default(0),
	   row_size decimal(18,2)

	)

	-- Populate the temp table...
	EXEC sp_MSforeachtable @command1=
			 "INSERT INTO #tblResults
			   ([name],[rows],[reserved_kb],[data_kb],[index_size_kb],[unused_kb])
			  EXEC sp_spaceused '?'"
	   

	-- Strip out the " KB" portion from the fields
	UPDATE #tblResults SET
	   [reserved_mb] = (CAST(SUBSTRING([reserved_kb], 1, 
								 CHARINDEX(' ', [reserved_kb])) AS int)/1024),
	   [data_mb] = (CAST(SUBSTRING([data_kb], 1, 
								 CHARINDEX(' ', [data_kb])) AS int)/1024),
	   [index_size_mb] = (CAST(SUBSTRING([index_size_kb], 1, 
								 CHARINDEX(' ', [index_size_kb])) AS int)/1024),
	   [unused_mb] = (CAST(SUBSTRING([unused_kb], 1, 
								 CHARINDEX(' ', [unused_kb])) AS int)/1024),
	  [reserved_gb] = ((CAST(SUBSTRING([reserved_kb], 1, 
								 CHARINDEX(' ', [reserved_kb])) AS int)/1024)/1024),
	   [data_gb] = ((CAST(SUBSTRING([data_kb], 1, 
								 CHARINDEX(' ', [data_kb])) AS int)/1024)/1024),
	   [index_size_gb] = ((CAST(SUBSTRING([index_size_kb], 1, 
								 CHARINDEX(' ', [index_size_kb])) AS int)/1024)/1024),
	   [unused_gb] = ((CAST(SUBSTRING([unused_kb], 1, 
								 CHARINDEX(' ', [unused_kb])) AS int)/1024)/1024)

		UPDATE #tblResults SET
		row_size = case when rows > 0 then convert(decimal(18,2),(data_mb + [index_size_mb]) * 1024)/rows else 0 end


	-- Return the results...
	SELECT	SUM([reserved_mb]) reserved_mb, 
			SUM([reserved_gb])reserved_gb, 
			SUM([data_mb]) data_mb,
			SUM([data_gb]) data_gb,
			SUM([index_size_mb]) index_size_mb,
			SUM([index_size_gb]) index_size_gb,
			SUM([unused_mb]) unused_mb,
			SUM([unused_gb]) unused_gb 
	 FROM #tblResults  
	SELECT * FROM #tblResults order by 2 desc

drop table #tblResults

end
