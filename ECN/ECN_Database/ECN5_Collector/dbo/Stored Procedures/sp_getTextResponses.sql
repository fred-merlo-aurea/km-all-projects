CREATE proc [dbo].[sp_getTextResponses] 
(
	@QuestionID int,
	@OtherText  bit,
	@filterstr varchar(8000)
)
as
Begin	


/*
select 'abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz' as EmailAddress, 
	   Convert(varchar(8000),'abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz') as	DataValue

*/

	create TABLE #Responses (EmailID int)

	declare @number int,
			@surveyID int,
			@groupdatafieldsID int,
			@groupID int
	
	select	@surveyID = s.SurveyID, 
			@number = q.number, 
			@groupID = GroupID 
	from	question q join survey s on q.SurveyID = s.SurveyID 
	where q.QuestionID = @QuestionID 

	select @groupdatafieldsID=groupdatafieldsID	from ecn5_communicator.dbo.groupdatafields where groupID = @groupID and surveyID = @surveyID and shortname = Convert(varchar,@surveyID) + '_' + Convert(varchar,@number) + (case when @OtherText = 1 then '_TEXT' else '' end)

	insert into #Responses
	exec sp_SurveyFilterResults @surveyID, @filterstr, 0

	SELECT	case when charindex('@survey_' + Convert(varchar, @surveyID) + '.com',e.emailaddress) > 0 then 'Anonymous' else e.emailaddress end as EmailAddress, 
			 replace(replace(edv.DataValue,char(10),' '), char(13),' ') as DataValue
	FROM 
			ecn5_communicator.dbo.EmailDataValues edv join ecn5_communicator.dbo.emails e on edv.emailID = e.emailID
	where 
			edv.GroupDatafieldsID = @groupdatafieldsID and 
			edv.EmailID in (select EmailID from #responses)
	order by 1 asc

	drop table #Responses

End
