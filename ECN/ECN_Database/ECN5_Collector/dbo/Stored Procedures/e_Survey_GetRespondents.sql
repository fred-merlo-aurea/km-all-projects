CREATE proc [dbo].[e_Survey_GetRespondents]
(
	@surveyID int,
	@filterstr varchar(1000)
)
as
Begin
	set nocount on
	create table #resp (emailID int)
	declare @GroupID int
	select @GroupID=GroupID from Survey where SurveyID=@surveyID
	
	insert into #resp
	exec sp_SurveyFilterResults @surveyID,@filterstr, 0
	select e.*, eg.FormatTypeCode, eg.SubscribeTypeCode, eg.CreatedOn, eg.LastChanged from ecn5_communicator..Emails e
	 with (nolock) join ecn5_communicator..EmailGroups eg with (nolock) 
	 on e.EmailID = eg.EmailID 
	 where eg.emailID In (select distinct EmailID from #resp)
	 and eg.GroupID =@GroupID
	drop table #resp
End