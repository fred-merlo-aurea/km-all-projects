CREATE proc [dbo].[rpt_getSurveyQuestions]   
(    
 @surveyID int    
)    
as    
Begin    
 select QuestionID, QuestionText, number, format from question     
 where SurveyID = @surveyID
 order by number    
End
