CREATE Proc [dbo].[sp_DeleteSurvey]           
(              
	@SurveyID int
)    
as    
Begin    
Delete from [SURVEYBRANCHING] where SurveyID = @SurveyID

Delete from grid_statements where QuestionID In (select QuestionID from question where SurveyID = @SurveyID)
Delete from response_options where QuestionID In (select QuestionID from question where SurveyID = @SurveyID)
Delete from question where SurveyID = @SurveyID
Delete from page where SurveyID = @SurveyID
Delete from [SURVEYSTYLES] where SurveyID = @SurveyID
Delete from survey where SurveyID = @SurveyID
Delete from [SURVEYBRANCHING] where SurveyID = @SurveyID
End
