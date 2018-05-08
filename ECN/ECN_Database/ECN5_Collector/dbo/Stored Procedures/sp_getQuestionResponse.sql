--sp_helptext sp_getQuestionResponse 21238, 0, '', 1200
CREATE proc [dbo].[sp_getQuestionResponse] 
(
	@QuestionID int,
	@EmailID int,
	@filterstr varchar(8000),
	@count int
)
as
Begin
	declare @number int,
			@surveyID int,
			@Total_response int,
			@groupdatafieldsID int,
			@groupID int,
			@Col1 varchar(8000),
			@format varchar(25),
			@ShowTextControl bit

/*
SELECT	1 as QuestionID, 
		10 AS OptionID, 
		'ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ' AS 'OptionValue', 
		100 AS responsecount, 
		100.00 AS 'ratio' 
*/

	create TABLE #Response (EmailID int)

	set @Col1 = ''
	set @Total_response = 0
	
	select	@surveyID = s.SurveyID, 
			@number = q.number, 
			@groupID = GroupID ,
			@format = format ,
			@ShowTextControl = ShowTextControl
	from	question q join survey s on q.SurveyID = s.SurveyID 
	where q.QuestionID = @QuestionID

	select @groupdatafieldsID=groupdatafieldsID	from ecn5_communicator.dbo.groupdatafields where groupID = @groupID and surveyID = @surveyID and shortname = Convert(varchar,@surveyID) + '_' + Convert(varchar,@number)

	insert into #Response
	exec sp_SurveyFilterResults @surveyID, @filterstr, 0

	--SELECT @Total_response = COUNT(EmailID) FROM #response
	SELECT @Total_response = @count

	if @format = 'textbox'
	Begin
		declare @response_count int

		SELECT	@response_count = COUNT(edv.DataValue)
		FROM 
				ecn5_communicator.dbo.EmailDataValues edv 
		where 
				edv.GroupDatafieldsID = @groupdatafieldsID and 
				edv.EmailID in (select EmailID from #response)
		
		SELECT	@QuestionID as QuestionID, 0 as OptionID, 'Answered' AS 'OptionValue', 
				@response_count AS 'responsecount', 0  as HasOtherResponse,
				CAST((CAST(@response_count AS float)/(case when @Total_response=0 then 1 else @Total_response end)*100) AS decimal(8,1)) AS 'ratio' 
		/*union
		SELECT	@QuestionID as QuestionID, 1 as OptionID, 'Not Answered' AS 'OptionValue', 
				@Total_response - @response_count AS 'responsecount', 
				CAST((CAST((@Total_response -@response_count) AS float)/@Total_response*100) AS decimal(8,1)) AS 'ratio' */

	end
	else if @format = 'grid'
	Begin

		select  @Col1 = @Col1 + coalesce(' max(Case when OptionValue=''' + replace(OptionValue,'''','''''')  + ''' then OptionID end) [' + Convert(varchar,OptionID)  + '], count(Case when OptionValue=''' + replace(OptionValue,'''','''''')  + ''' then edv.emaildatavaluesID end) [' + RTRIM(OptionValue)  + '],', '')        
		from	response_options 
		where	QuestionID =  @QuestionID 
	         
		set @Col1 = substring(@Col1, 0, len(@Col1))   

		exec ('select ' + @QuestionID + ' as QuestionID,GridStatementID, GridStatement, ' + @Col1 + '
		from	
				grid_statements gs cross join 
				response_options ro left outer join
				ecn5_communicator..emaildatavalues edv on edv.surveygridID = gs.GridStatementID and ro.OptionValue = edv.datavalue and edv.groupdatafieldsID = ' + @groupdatafieldsID + ' and 
				(EmailID = (case when ' + @EmailID + ' = 0 then EmailID else ' + @emailID + ' end)  and
				EmailID in (select EmailID from #response))
		where gs.QuestionID = ' + @QuestionID + ' and ro.QuestionID = ' + @QuestionID + '
		group by GridStatementID, GridStatement')
	end
	else if (@format = 'checkbox' or @format = 'dropdown' or @format = 'radio' )
	Begin
		declare @lastOptionID int
		
		set @lastOptionID =0 
		
		if @ShowTextControl =1
			select  @lastOptionID = MAX(OptionID) from  response_options where QuestionID = @QuestionID
			
	
		SELECT	@QuestionID as QuestionID, ro.OptionID,ro.OptionValue AS 'OptionValue', 
				COUNT(r.DataValue) AS 'responsecount', 
				case when ro.OptionID = @lastOptionID then 1 else 0 end as HasOtherResponse,
				case when @Total_response = 0 then 0 else CAST((CAST(COUNT(r.DataValue)AS float)/@Total_response*100) AS decimal(8,1)) end AS 'ratio'
		FROM 
				response_options ro left outer join
				ecn5_communicator.dbo.EmailDataValues r on  ro.OptionValue = r.DataValue and r.GroupDatafieldsID = @groupdatafieldsID and 
				--(EmailID = (case when @EmailID = 0 then EmailID else @emailID end)) and
				EmailID in (select EmailID from #response)
		where 
				ro.QuestionID = @QuestionID
		GROUP BY ro.OptionID,ro.OptionValue
	end
	drop table #response
end
