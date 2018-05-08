CREATE Proc dbo.e_SurveyBranching_Save           
(              
	@SurveyID int,
	@QuestionID int,    
	@OptionID int = NULL,    	
	@PageID int = NULL, 
	@EndSurvey bit = NULL
)    
as    
Begin    

	insert into SurveyBranching(SurveyID, QuestionID, OptionID,PageID, EndSurvey)
	values(@SurveyID, @QuestionID, @OptionID, @PageID, @EndSurvey)
End