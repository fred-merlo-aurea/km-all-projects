CREATE PROCEDURE [dbo].[sp_ConvertXmlFilterTop]
	-- Add the parameters for the stored procedure here
	@strFilter text,
	@docHandle int,
	@newFilter varchar(4000) output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
	declare @build_column varchar(800),
	@build_value varchar(4000) 

--	EXEC sp_xml_preparedocument @docHandle OUTPUT, @strFilter

	 
	set @newFilter = '<filters>'

	DECLARE xml_Filter CURSOR FOR
	select [column], [values]
	FROM	OPENXML(@docHandle, N'/xml/GlobalFilters/Field')   
	WITH   
	(  
		[column] varchar(800) '@column', 
		[values] varchar(4000) '@values'
	) 

	OPEN xml_Filter  

	FETCH NEXT FROM xml_Filter INTO @build_column , @build_value

	WHILE @@FETCH_STATUS = 0  
	BEGIN  

		IF @build_column = 's.CategoryID'  BEGIN
			set @newFilter = @newFilter + '<CategoryCodes>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</CategoryCodes>';
		END
		ELSE IF @build_column = 'CategoryGroupID'  BEGIN
			set @newFilter = @newFilter + '<CategoryIDs>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</CategoryIDs>';
		END
		ELSE IF @build_column = 'TransactionGroupID'  BEGIN
			set @newFilter = @newFilter + '<TransactionIDs>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</TransactionIDs>';
		END
		ELSE IF @build_column = 'QsourceGroupID'  BEGIN
			set @newFilter = @newFilter + '<QsourceIDs>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</QsourceIDs>';
		END
		ELSE IF @build_column = 'state' BEGIN
			set @newFilter = @newFilter + '<StateIDs>';
			set @newFilter = @newFilter + REPLACE(@build_value,'''','');
			set @newFilter = @newFilter + '</StateIDs>';
		END
		ELSE IF @build_column = 's.CountryID'  BEGIN
			set @newFilter = @newFilter + '<CountryIDs>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</CountryIDs>';
		END
		ELSE IF @build_column = 's.EmailExists'  BEGIN
			set @newFilter = @newFilter + '<Email>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</Email>';
		END
		ELSE IF @build_column = 's.PhoneExists'  BEGIN
			set @newFilter = @newFilter + '<Phone>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</Phone>';
		END
		ELSE IF @build_column = 's.FaxExists'  BEGIN
			set @newFilter = @newFilter + '<Fax>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</Fax>';
		END
		ELSE IF @build_column = 'Demo31'  BEGIN
			set @newFilter = @newFilter + '<Demo31>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</Demo31>';
		END
		ELSE IF @build_column = 'Demo32'  BEGIN
			set @newFilter = @newFilter + '<Demo32>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</Demo32>';
		END
		ELSE IF @build_column = 'Demo33'  BEGIN
			set @newFilter = @newFilter + '<Demo33>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</Demo33>';
		END
		ELSE IF @build_column = 'Demo34'  BEGIN
			set @newFilter = @newFilter + '<Demo34>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</Demo34>';
		END
		ELSE IF @build_column = 'Demo35'  BEGIN
			set @newFilter = @newFilter + '<Demo35>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</Demo35>';
		END
		ELSE IF @build_column = 'Demo36'  BEGIN
			set @newFilter = @newFilter + '<Demo36>';
			set @newFilter = @newFilter + @build_value;
			set @newFilter = @newFilter + '</Demo36>';
		END


		FETCH NEXT FROM xml_Filter INTO @build_column , @build_value
	End  

	close xml_Filter
	DEALLOCATE xml_Filter

--	EXEC sp_xml_removedocument @docHandle

END