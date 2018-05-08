CREATE proc [dbo].[sp_ReOrderPage]
(
	@PageID int,
	@Position char(1),
	@ToPageID int
)
as
Begin
	declare @SurveyID int, 
			@TotalPages int,
			@currentPagenumber int,
			@TargetPageNumber int,
			@newPageNumber int

	select @SurveyID = SurveyID , @currentPagenumber = number from page where PageID = @PageID
	select @TotalPages = isnull(max(number),1) from page where SurveyID = @SurveyID 
	select @TargetPageNumber= number from page where PageID = @ToPageID

	if @ToPageID > 0
	Begin

		if @currentPagenumber < @TargetPageNumber
		Begin
			if @Position='a'
				set @newPageNumber = @TargetPageNumber
			else 
				set @newPageNumber = @TargetPageNumber - 1
			
		end
		else if @currentPagenumber > @TargetPageNumber
		Begin
			if @Position='a' 
				set @newPageNumber = @TargetPageNumber + 1
			else
				set @newPageNumber = @TargetPageNumber

		end

		if @newPageNumber > @TotalPages
			set @newPageNumber = @TotalPages

		if @currentPagenumber > @newPageNumber
			update page 
			set number = number + 1
			where SurveyID = @SurveyID and number >= @newPageNumber and number <= @currentPagenumber
		else if @currentPagenumber < @newPageNumber
			update page 
			set number = number - 1
			where SurveyID = @SurveyID  and number <= @newPageNumber and number >= @currentPagenumber

		update page 
		set number = @newPageNumber
		where PageID = @PageID

		--reorder questions

		declare @QuestionID int, 
				@counter int

		set @counter = 1

		DECLARE ecursor CURSOR FOR 
			select QuestionID from question q join Page p on q.PageID = p.PageID 
			where p.SurveyID = @SurveyID
			order by p.number, q.number

			OPEN ecursor

			FETCH NEXT FROM ecursor INTO @QuestionID

			WHILE @@FETCH_STATUS = 0
			BEGIN

				update question
				set number = @counter
				where QuestionID = @QuestionID

				set @counter = @counter + 1

				FETCH NEXT FROM ecursor INTO @QuestionID

			end

			CLOSE ecursor
			DEALLOCATE ecursor

	end
End
