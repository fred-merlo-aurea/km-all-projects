CREATE  proc [dbo].[rpt_getGridResponse] 
(
	@QuestionID int,
	@EmailID int,
	@filterstr varchar(8000)
)
as
Begin
	declare @number int,
			@surveyID int,
			@groupdatafieldsID int,
			@groupID int,
			@format varchar(25)


	create TABLE #Response (EmailID int)

	
	select	@surveyID = s.SurveyID, 
			@number = q.number, 
			@groupID = GroupID ,
			@format = format 
	from	question q join survey s on q.SurveyID = s.SurveyID 
	where q.QuestionID = @QuestionID

	select @groupdatafieldsID=groupdatafieldsID	from ecn5_communicator.dbo.groupdatafields where groupID = @groupID and surveyID = @surveyID and shortname = Convert(varchar,@surveyID) + '_' + Convert(varchar,@number)

	insert into #Response
	exec sp_SurveyFilterResults @surveyID, @filterstr, 0


	if @format = 'grid'
	Begin

		exec('select GridStatementID, GridStatement, OptionID, OptionValue, count(edv.emaildatavaluesID) as TotalCount 
		from	
				grid_statements gs cross join 
				response_options ro left outer join
				ecn5_communicator..emaildatavalues edv on edv.surveygridID = gs.GridStatementID and ro.OptionValue = edv.datavalue and edv.groupdatafieldsID = ' + @groupdatafieldsID + ' and 
				(EmailID = (case when ' + @EmailID + ' = 0 then EmailID else ' + @EmailID + ' end)  and
				EmailID in (select EmailID from #response))
		where gs.QuestionID = ' + @QuestionID + ' and ro.QuestionID = ' + @QuestionID + '
		group by GridStatementID, GridStatement, OptionID, OptionValue')
	end
	
	drop table #response
end
