CREATE proc [dbo].[sp_DeletePage] 
(
	@PageID int
)
as
Begin
	
	declare @SurveyID int, 
			@totalQuestions int,
			@maxPagenumber int,
			@newPageNumber int,
			@currentPageNumber int

	set @totalQuestions = 0
	set @maxPagenumber = 0

	select @totalQuestions = count(QuestionID), @maxPagenumber = max(number) from question 
	where PageID = @PageID

	select @SurveyID = SurveyID, @currentPageNumber = number from page where PageID = @PageID 
	
	update page 
	set number = number - 1
	where SurveyID = @SurveyID and number > @currentPageNumber


	set @newPageNumber = @maxPagenumber - @totalQuestions 
		
	if @newPageNumber > 0
	Begin
		update question
		set		number = number - @newPageNumber
		where
				SurveyID = @SurveyID and number > @newPageNumber
	end

	delete from [SURVEYBRANCHING] where QuestionID in (select QuestionID from question where PageID = @PageID)
	delete from response_options where QuestionID in (select QuestionID from question where PageID = @PageID)
	delete from grid_statements where QuestionID in (select QuestionID from question where PageID = @PageID)
	delete from question where QuestionID in (select QuestionID from question where PageID = @PageID)
	delete from page where PageID =@PageID
End
