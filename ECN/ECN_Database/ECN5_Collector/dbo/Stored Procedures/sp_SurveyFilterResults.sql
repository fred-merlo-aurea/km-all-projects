CREATE proc [dbo].[sp_SurveyFilterResults] 
(
	@surveyID int, 
	@Filterstr varchar(8000),
	@OnlyCounts int
)   
AS       
  
BEGIN       
	set nocount on
	declare @groupID int,	
			@Col1 varchar(8000),
			@Col2 varchar(8000),
			@Col3 varchar(8000),
			@gdfID_completiondt int,
			@tmpfilter varchar(100),
			@statementID int,
			@optionID int,
			@tempstr varchar(100)

	set @col1 = ''
	set @col2 = ''
	set @col3 = ''

	select	@groupID = s.GroupID, @gdfID_completiondt = gdf.groupdatafieldsID	
	from	Survey s join 
			ecn5_communicator.dbo.groupdatafields gdf on s.SurveyID = gdf.surveyID and s.GroupID = gdf.groupID
	where	s.SurveyID = @surveyID and 
			shortname = Convert(varchar,@surveyID) + '_completionDt'


	if len(ltrim(rtrim(@Filterstr))) > 0
	Begin

		declare @filter TABLE (questionID int, statementID int, OptionID int, gdfID int, answer varchar(255))

		DECLARE cFilter CURSOR FOR select * from dbo.fn_split(@Filterstr, ',')

		OPEN cFilter
		FETCH NEXT FROM cFilter INTO @tmpfilter

		WHILE @@FETCH_STATUS = 0
		BEGIN
			set	@tempstr = substring(@tmpfilter,charindex('|',@tmpfilter)+1, len(@tmpfilter))
			set @statementID = 0
			set @optionID = 0
			if (charindex('|',@tempstr) > 0)
			Begin
				set @statementID = substring(@tempstr,1,charindex('|',@tempstr)-1)
				set @optionID = substring(@tempstr,charindex('|',@tempstr)+1, len(@tempstr))
			End
			else
			Begin
				set @optionID = @tempstr
			end

			insert into @filter (questionID, statementID, OptionID)
			select substring(@tmpfilter,1,charindex('|',@tmpfilter)-1), isnull(@statementID,0), @optionID 

			FETCH NEXT FROM cFilter INTO @tmpfilter

		end

		CLOSE cFilter
		DEALLOCATE cFilter

		update @filter
		set gdfID = gdf.groupdatafieldsID, 
			answer = REPLACE(isnull(OptionValue ,''),CHAR(39),CHAR(39) + CHAR(39))
		from 
				@filter fn left outer join 
				response_options ro on fn.OptionID = ro.OptionID join
				question q on q.QuestionID = fn.questionID left outer join 
				grid_statements g on g.GridStatementID = fn.statementID join
				ECN5_Communicator..groupdatafields gdf on gdf.surveyID = @surveyID and groupID = @groupID and shortname = convert(varchar, @surveyID) + '_' + convert(varchar, q.number)

		select  @Col1 = @Col1 + coalesce('(groupdatafieldsID = ' + convert(varchar,gdfID) + (case when answer='' and OptionID=0 and statementID=0 then ' and isnull(datavalue,'''')<>''' else ' and datavalue=''' end) + answer + ''' and isnull(SurveyGridID,0) = ' + convert(varchar,statementID) + ') or ', ''),
				@Col2 = @Col2 + coalesce('max(case when gdfID = ' + convert(varchar,gdfID) + ' then isnull(datavalue,'''') end) as [' + convert(varchar,gdfID) +'],', '') ,
				@Col3 = @Col3 + coalesce(' [' + convert(varchar,gdfID) + ']' + (case when answer='' and OptionID=0 and statementID=0 then '<>''' else '=''' end) +  + answer + ''' and ', '') 
		from @filter

		set @Col1 = substring(@Col1, 0, len(@Col1)-2)   
		set @Col2 = substring(@Col2, 0, len(@Col2))   
		set @Col3 = substring(@Col3, 0, len(@Col3)-2) 

		if @OnlyCounts = 1
			exec ('select count(emailID) from (select emailID, ' + @Col2 + ' from (select emailID, groupdatafieldsID as gdfID, datavalue from ECN5_Communicator..emaildatavalues ' +
			' where (' + @Col1 + ') and EmailID in (SELECT DISTINCT(EmailID) FROM ecn5_communicator.dbo.EmailDataValues WHERE GroupDatafieldsID = ' + @gdfID_completiondt + ')) as edv group by emailID) inn where ' + @col3)
		else
			exec ('select emailID from (select emailID, ' + @Col2 + ' from (select emailID, groupdatafieldsID as gdfID, datavalue from ECN5_Communicator..emaildatavalues ' +
			' where (' + @Col1 + ') and EmailID in (SELECT DISTINCT(EmailID) FROM ecn5_communicator.dbo.EmailDataValues WHERE GroupDatafieldsID = ' + @gdfID_completiondt + ')) as edv group by emailID) inn where ' + @col3)

		--	print ('select emailID from (select emailID, ' + @Col2 + ' from (select emailID, groupdatafieldsID as gdfID, datavalue from ECN5_Communicator..emaildatavalues ' +
		--	' where (' + @Col1 + ') and EmailID in (SELECT DISTINCT(EmailID) FROM ecn5_communicator.dbo.EmailDataValues WHERE GroupDatafieldsID = ' + Convert(varchar,@gdfID_completiondt) + ')) as edv group by emailID) inn where ' + @col3)


	end
	else
	Begin
		if @OnlyCounts = 1
			SELECT count(DISTINCT(EmailID)) FROM ecn5_communicator.dbo.EmailDataValues WHERE GroupDatafieldsID = @gdfID_completiondt
		else
			SELECT DISTINCT(EmailID) FROM ecn5_communicator.dbo.EmailDataValues WHERE GroupDatafieldsID = @gdfID_completiondt
	end
	
END
