CREATE proc [dbo].[sp_getIncompleteSurveyCount]
(
	@surveyID int
)   
as  
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

	select @groupID = s.GroupID from Survey s where	s.SurveyID = @surveyID

	select	@gdfID_completiondt = gdf.groupdatafieldsID	
	from	ecn5_communicator.dbo.groupdatafields gdf 
	where	gdf.surveyID = @surveyID and 
			shortname = Convert(varchar,@surveyID) + '_completionDt'

			SELECT count(EmailID) FROM ecn5_communicator.dbo.EmailGroups
			where groupID =@groupID
			and emailID not in (SELECT EmailID FROM ecn5_communicator.dbo.EmailDataValues WHERE GroupDatafieldsID = @gdfID_completiondt)




--			SELECT count(DISTINCT(EmailID)) FROM ecn5_communicator.dbo.EmailDataValues 
--			where groupdatafieldsID in (select groupdatafieldsID from ecn5_communicator..groupdatafields where groupID =@groupID)
--			and emailID not in (SELECT DISTINCT(EmailID) FROM ecn5_communicator.dbo.EmailDataValues WHERE GroupDatafieldsID = @gdfID_completiondt)
--	
END
