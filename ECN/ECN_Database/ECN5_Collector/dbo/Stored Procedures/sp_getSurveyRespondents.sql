CREATE proc [dbo].[sp_getSurveyRespondents]
(
	@surveyID int,
	@emailID int,
	@filterstr varchar(8000)
)
as
Begin
/*
	select	1000 surveyID,
			100 emailID, 
			'abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz' as EmailAddress, 
			'abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz' as groupname, 
			100 as BlastID, 
			'abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz' as BlastSubject,
			Convert(datetime,'10/10/2005') completedDate 
*/
	declare @gdf_BlastID int,
			@gdf_completedDate int

	select @gdf_BlastID = GroupDataFieldsID from ecn5_communicator..groupdatafields where surveyID = @surveyID and shortname = Convert(varchar, @surveyID) + '_blastID'
	select @gdf_completedDate = GroupDataFieldsID from ecn5_communicator..groupdatafields where surveyID = @surveyID and shortname = Convert(varchar, @surveyID) + '_completionDt'

	declare @respondents TABLE (emailID int, BlastID int , CompletedDate datetime)
	create TABLE #Response (EmailID int)

	insert into #Response
	exec sp_SurveyFilterResults @surveyID, @filterstr, 0

	insert into @respondents
	select	emailID, 
			case when groupdatafieldsID = @gdf_BlastID then datavalue end,
			case when groupdatafieldsID = @gdf_completedDate then datavalue end
	from	ecn5_communicator..emaildatavalues 
	where	groupdatafieldsID in (@gdf_BlastID,@gdf_completedDate) and 
			EmailID = case when @emailID = 0 then EmailID else @emailID end and
			EmailID in (select EmailID from #Response)

	drop table #Response			

	select	@surveyID surveyID,
			e.emailID, 
			case when charindex('@survey_' + Convert(varchar, @surveyID) + '.com',e.emailaddress) > 0 then 'Anonymous' else e.emailaddress end as EmailAddress, 
			isnull(g.groupname,'') as groupname, 
			isnull(r.blastID,0) as BlastID, 
			case when len(isnull(EmailSubject,'')) > 45 then substring(EmailSubject,1,45) + '...' else isnull(EmailSubject,'') end as BlastSubject,
			completedDate 
	from	ecn5_communicator..emails e join 
			(select emailID, max(blastID) as blastID, max(CompletedDate) CompletedDate from @respondents r group by emailID) r on e.emailID = r.emailID
			left outer join ecn5_communicator..[BLAST] b on b.blastID = r.blastID 
			left outer join ecn5_communicator..groups g on g.groupID = b.groupID

end
