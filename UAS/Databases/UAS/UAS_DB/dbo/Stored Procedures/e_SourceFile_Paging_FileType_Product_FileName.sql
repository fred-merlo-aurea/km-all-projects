CREATE PROCEDURE [dbo].[e_SourceFile_Paging_FileType_Product_FileName]
	@ClientID int,
	@CurrentPage int = 1, 
	@PageSize int = 10, 
	@ServiceID int, 
	@Type varchar(10),
	@PubID int = NULL, 
	@FileTypeID int = NULL, 
	@FileName varchar(250) = NULL,
	@SortField varchar(250) = 'isnull(dateupdated, datecreated)', 
	@SortDirection varchar(4) = 'DESC'
AS			
BEGIN		
	----Start Test Data--------------------------------------
	--DECLARE @ClientID int = 25, @CurrentPage int = 1, @PageSize int = 10, @ServiceID int = 14, @Type varchar(10) = 'CIRC', @PubID int = NULL, @FileTypeID int = NULL, @FileName varchar(250) = NULL, @SortField varchar(250) = 'isnull(dateupdated, datecreated)', @SortDirection varchar(4) = 'DESC'
	----End Test Data----------------------------------------
	set @SortField = (Select CASE WHEN @SortField = 'PubCode' THEN 'PublicationID' 
									WHEN @SortField = 'FileType' and @Type = 'CIRC' THEN 'DatabaseFileTypeId'					
									WHEN @SortField = 'FileType' and @Type = 'UAD' THEN 'ServiceFeatureID'
									WHEN @SortField = 'CreatedByUserName' THEN 'CreatedByUserID'				
									WHEN @SortField = 'UpdatedByUserName' THEN 'UpdatedByUserID'
									ELSE @SortField END)

	DECLARE @SQL varchar(max) = ''
	set @SQL = 'WITH Results
	AS (
		SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortField + ' ' + @SortDirection + ' ) AS ROWNUM,
		*
		FROM SourceFile sf (nolock)		
		WHERE sf.ClientID = ' + Cast(@ClientID as varchar(10)) + '
		and sf.IsDeleted = 0 
		and sf.ServiceID = ' + Cast(@ServiceID as varchar(20))
		if (LEN(@FileName) > 0)
		BEGIN
			set @SQL = @SQL + ' and sf.FileName like ''%' + REPLACE(@FileName, '''', '''''') + '%'''
		END

		if (@PubID > 0)
		BEGIN
			set @SQL = @SQL + ' and sf.PublicationID = ' + Cast(@PubID as varchar(20))
		END		
		
		if (@Type = 'CIRC' and @FileTypeID > 0)
		BEGIN
			set @SQL = @SQL + ' and sf.DatabaseFileTypeId = ' + Cast(@FileTypeID as varchar(20))
		END
		else if (@Type = 'UAD' and @FileTypeID > 0)
		BEGIN
			set @SQL = @SQL + ' and sf.ServiceFeatureID = ' + Cast(@FileTypeID as varchar(20))
		END		

	set @SQL = @SQL + ' 
	)
	SELECT *
	FROM Results
	WHERE ROWNUM between ((' + Cast(@CurrentPage as varchar(10)) + ' - 1) * ' + Cast(@PageSize as varchar(10)) + ' + 1) and (' + Cast(@CurrentPage as varchar(10)) + ' * ' + Cast(@PageSize as varchar(10)) + ')'

	--print(@SQL)
	exec(@SQL)
END
