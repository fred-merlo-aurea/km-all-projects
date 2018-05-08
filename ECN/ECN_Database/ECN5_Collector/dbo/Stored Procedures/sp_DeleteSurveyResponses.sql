CREATE proc [dbo].[sp_DeleteSurveyResponses]
(
	@surveyID int
)
as
Begin
	declare	@groupID int
	
	select @groupID = GroupID from ecn5_collector..survey where SurveyID = @surveyID

	delete from ecn5_communicator..emaildatavalues where groupdatafieldsID in (select groupdatafieldsID from ecn5_communicator..groupdatafields where groupID = @groupID and surveyID = @surveyID)
	delete from ecn5_communicator..emailgroups where groupID = @groupID 

End
