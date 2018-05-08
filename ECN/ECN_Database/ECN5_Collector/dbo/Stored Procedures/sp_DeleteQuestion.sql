CREATE proc [dbo].[sp_DeleteQuestion] 
(
	@QuestionID int
)
as
Begin
	declare @SurveyID int,
			@number int,
			@totalQcount int

	select @SurveyID = SurveyID, @number = number from question where QuestionID = @QuestionID
	select @totalQcount = count(QuestionID) from question where SurveyID = @SurveyID

	update question
	set		number = number -1
	where
			SurveyID = @SurveyID and number > @number

	if isnull(@SurveyID,0) <> 0
		delete from ecn5_communicator..GroupDatafields 
		where SurveyID = @SurveyID and shortname = convert(varchar,@SurveyID) + '_' + convert(varchar,@totalQcount)

	delete from [SURVEYBRANCHING] where QuestionID = @QuestionID
	delete from response_options where QuestionID = @QuestionID
	delete from grid_statements where QuestionID = @QuestionID
	delete from question where QuestionID = @QuestionID
End
