CREATE PROCEDURE [dbo].[e_SourceFile_Paging_Count_FileType_Product_FileName]
	@ClientID int,
	@ServiceID int, 
	@Type varchar(10),
	@PubID int = NULL, 
	@FileTypeID int = NULL, 
	@FileName varchar(250) = NULL
AS			
BEGIN		
	----Start Test Data--------------------------------------
	--DECLARE @ClientID int = 25, @ServiceID int = 14, @Type varchar(10) = 'CIRC', @PubID int = NULL, @FileTypeID int = 2725, @FileName varchar(250) = NULL
	----End Test Data----------------------------------------

	DECLARE @SQL varchar(max) = ''
	set @SQL = '
		SELECT Count(SourceFileID)		
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

	--print(@SQL)
	exec(@SQL)
END