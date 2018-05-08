CREATE proc [dbo].[sp_ReOrderQuestion]
(
	@PageID int,
	@QuestionID int,
	@Position char(1),
	@ToQuestionID int
)
as
Begin
	declare @SurveyID int,
			@TotalQuestions int,
			@currentQuestionNumber int,
			@TargetQuestionNumber int,
			@newQuestionNumber int

	select @SurveyID = SurveyID , @currentQuestionNumber=number from question where QuestionID=@QuestionID

	select @TotalQuestions=max(q.number) from question q where SurveyID = @SurveyID 	

	if @ToQuestionID = 0
	Begin
		select @TargetQuestionNumber= isnull(max(q.number),0) + 1
		from	question q join page p on q.PageID = p.PageID  
		where	p.SurveyID = @SurveyID and
				p.number <= (select number from page where PageID = @PageID)

		set @newQuestionNumber = @TargetQuestionNumber
	end
	else
		select @TargetQuestionNumber=number from question where QuestionID=@ToQuestionID


	if @currentQuestionnumber < @TargetQuestionNumber
		Begin
			if @Position='a'
				set @newQuestionNumber = @TargetQuestionNumber
			else 
				set @newQuestionNumber = @TargetQuestionNumber - 1
			
		end
	else if @currentQuestionnumber > @TargetQuestionNumber
	Begin
			if @Position='a' 
				set @newQuestionNumber = @TargetQuestionNumber + 1
			else
				set @newQuestionNumber = @TargetQuestionNumber

	end

	if @newQuestionNumber > @TotalQuestions
		set @newQuestionNumber = @TotalQuestions

	if @newQuestionNumber > @currentQuestionNumber
		update question
		set		number = number - 1 
		where	SurveyID = @SurveyID and number > @currentQuestionNumber and number <= @newQuestionNumber
	else if @newQuestionNumber <= @currentQuestionNumber
		update question
		set		number = number + 1 
		where	SurveyID = @SurveyID and number >= @newQuestionNumber and number < @currentQuestionNumber

	update question
	set		PageID = @PageID,
			number  = @newQuestionNumber
	where	QuestionID = @QuestionID
End
