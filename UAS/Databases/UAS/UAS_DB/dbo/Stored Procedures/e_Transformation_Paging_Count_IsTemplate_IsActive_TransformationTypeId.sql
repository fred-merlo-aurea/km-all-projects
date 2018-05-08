﻿CREATE PROCEDURE [dbo].[e_Transformation_Paging_Count_IsTemplate_IsActive_TransformationTypeId]
	@ClientID int,
    @IsTemplate bit,
    @IsActive bit,
    @TransformationTypeId int,
    @IgnoreAdminTransformationTypes bit,
	@AdminTransformId int = 0
AS
BEGIN
	----Start Test Data--------------------------------------
	--DECLARE @ClientID int = 25, @IsTemplate bit = 1, @IsActive bit = 1, @TransformationTypeId int = 72, @IgnoreAdminTransformationTypes bit = 1, @AdminTransformId int = 73, @SortField varchar(250) = 'isnull(dateupdated, datecreated)', @SortDirection varchar(4) = 'DESC'
	----End Test Data----------------------------------------

	DECLARE @SQL varchar(max) = ''
	set @SQL = 'SELECT COUNT(*)
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

	--print(@SQL)
	exec(@SQL)
END