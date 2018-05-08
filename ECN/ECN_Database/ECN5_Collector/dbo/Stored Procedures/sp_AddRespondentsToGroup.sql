CREATE proc [dbo].[sp_AddRespondentsToGroup]
(
	@surveyID int,
	@filterstr varchar(1000),
	@groupID int
)
as
Begin
	set nocount on
	create table #resp (emailID int)

	insert into #resp
	exec sp_SurveyFilterResults @surveyID,@filterstr, 0

	insert into ecn5_communicator..emailgroups (EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn)
	select	distinct e.emailID, @groupID,  'html', 'S', getdate() 
	from 
			#resp r join ecn5_communicator..emails e on r.emailID = e.emailID
	where 
			emailaddress not like '%@survey_' + Convert(varchar, @surveyID) + '.com' and
			e.emailID not in (select emailID from ecn5_communicator..emailgroups where groupID = @groupID)

	select @@ROWCount

	drop table #resp
End
