CREATE PROCEDURE [dbo].[e_Transformation_Paging_IsTemplate_IsActive_TransformationTypeId]
	@ClientID int,
	@CurrentPage int,
	@PageSize int,
	@IsTemplate bit,
	@IsActive bit,
	@TransformationTypeId int,
	@IgnoreAdminTransformationTypes bit,
	@AdminTransformId int = 0,
	@SortField varchar(250) = 'isnull(dateupdated, datecreated)', 
	@SortDirection varchar(4) = 'DESC'
AS
BEGIN
	----Start Test Data--------------------------------------
	--DECLARE @ClientID int = 25, @CurrentPage int = 1, @PageSize int = 10, @IsTemplate bit = 1, @IsActive bit = 1, @TransformationTypeId int = 72, @IgnoreAdminTransformationTypes bit = 1, @AdminTransformId int = 73, @SortField varchar(250) = 'isnull(dateupdated, datecreated)', @SortDirection varchar(4) = 'DESC'
	----End Test Data----------------------------------------
	set @SortField = (Select CASE WHEN @SortField = 'TransformationName' THEN 'TransformationName' 
									WHEN @SortField = 'TransformationDescription'  THEN 'TransformationDescription'					
									WHEN @SortField = 'TransformationType'  THEN 'TransformationTypeID'
									ELSE @SortField END)

	DECLARE @SQL varchar(max) = ''
	set @SQL = 'WITH Results
	AS (
		SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortField + ' ' + @SortDirection + ' ) AS ROWNUM,
		*
		FROM Transformation t (nolock)		
		WHERE t.ClientID = ' + Cast(@ClientID as varchar(10)) + '
		and t.IsActive = ' + Cast(@IsActive as varchar(10)) + '
		and t.IsTemplate = ' + Cast(@IsTemplate as varchar(20))
	
		if (@IgnoreAdminTransformationTypes = 'true')
		BEGIN
		set @SQL = @SQL + ' and t.TransformationTypeID <> ' + Cast(@AdminTransformId as varchar(10))
		END 

		if (@TransformationTypeId > 0)
		BEGIN
			set @SQL = @SQL + ' and t.TransformationTypeID = ' + Cast(@TransformationTypeId as varchar(10))
		END

	set @SQL = @SQL + ' 
	)
	SELECT *
	FROM Results
	WHERE ROWNUM between ((' + Cast(@CurrentPage as varchar(10)) + ' - 1) * ' + Cast(@PageSize as varchar(10)) + ' + 1) and (' + Cast(@CurrentPage as varchar(10)) + ' * ' + Cast(@PageSize as varchar(10)) + ')'

	--print(@SQL)
	exec(@SQL)
END