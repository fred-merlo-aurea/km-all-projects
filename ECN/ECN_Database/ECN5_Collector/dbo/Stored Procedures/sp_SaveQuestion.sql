-- Procedure
CREATE proc [dbo].[sp_SaveQuestion] 
(
	@QuestionID int,
	@SurveyID  int,
	@PageID  int,
	@format varchar(25),
	@grid_control_type char(1),
	@QuestionText text,
	@maxlength varchar(3),
	@Required char(1),
	@GridValidation int,
	@ShowTextControl char(1),
	@position char(1),
	@targetID int,
	@options text,
	@gridrow text

)
as
Begin

	declare @currentQnumber int,
			@targetQno int,
			@currentpagenumber int,
			@GroupID int,
			@docHandle int  

	select @GroupID = GroupID from survey where SurveyID = @SurveyID

	select @currentpagenumber = number from page where PageID = @PageID

	if @QuestionID > 0 --update
	Begin
		update	Question
		Set 
				QuestionText = @QuestionText,
				format = @format,
				grid_control_type = case when @format = 'grid' then @grid_control_type else NULL end,
				maxlength = case when @maxlength='' or @format <> 'textbox' then NULL else @maxlength end,
				Required = (case when @format = 'grid' and @GridValidation > 0 then 1 else @Required end),
				GridValidation = (case when @format = 'grid' then @GridValidation else 0 end),
				ShowTextControl = @ShowTextControl
		where
				QuestionID = @QuestionID		
				
		

		if not exists (select * from [SURVEYBRANCHING] where QuestionID = @QuestionID)
		Begin
			Delete from response_options where QuestionID = @QuestionID

			if @@ERROR = 0
			Begin
				EXEC sp_xml_preparedocument @docHandle OUTPUT, @options  

				insert into response_options
				SELECT  @QuestionID, Responseoption,  isnull(score,0)
				FROM OPENXML(@docHandle, N'/options/option')   
				WITH   
				(  
					Responseoption varchar(255) '.', 
					score  int '@score'
				) 
				EXEC sp_xml_removedocument @docHandle    
			end
			
		
			Delete from grid_statements where QuestionID = @QuestionID

			if @@ERROR = 0
			Begin

				EXEC sp_xml_preparedocument @docHandle OUTPUT, @gridrow  

				insert into grid_statements
				SELECT  @QuestionID, Responseoption
				FROM OPENXML(@docHandle, N'/options/option')   
				WITH   
				(  
					Responseoption varchar(255) '.'
				) 
				
				EXEC sp_xml_removedocument @docHandle    
			end

			
		end
	End
	else
	Begin

		--calcullate the position.
		if @targetID > 0 -- if the target Question is selected.
		Begin
			select @currentQnumber = case when @position = 'a' then number+1 else number end from question where QuestionID = @targetID
		end
		else
		Begin
			-- if target question is not selected or current page has no questions - get the last question number from previous page and add 1.
			set @currentQnumber	= 1

			if exists(select QuestionID from question where SurveyID = @SurveyID)
				select @currentQnumber = isnull(max(number),0) + 1 from question where PageID in (select PageID from page where SurveyID = @SurveyID and number <= (@currentpagenumber-1))
		End

		-- Reorder current questions.
		update question set number = number + 1 where SurveyID = @SurveyID and number >= @currentQnumber

		insert into question 
			(SurveyID, PageID, QuestionText, format, number, grid_control_type, maxlength, Required, ShowTextControl, GridValidation) 
		values
			(@SurveyID, @PageID, @QuestionText, @format, @currentQnumber, 
			case when @format = 'grid' then @grid_control_type else NULL end, 
			case when @maxlength='' or @format <> 'textbox' then NULL else @maxlength end, @Required , @ShowTextControl, @GridValidation)


		set @QuestionID = @@IDENTITY


		EXEC sp_xml_preparedocument @docHandle OUTPUT, @options  

		insert into response_options
		SELECT  @QuestionID, Responseoption,  isnull(score,0)
		FROM OPENXML(@docHandle, N'/options/option')   
		WITH   
		(  
			Responseoption varchar(255) '.', 
			score  int '@score'
		) 
		EXEC sp_xml_removedocument @docHandle  
		
		EXEC sp_xml_preparedocument @docHandle OUTPUT, @gridrow  

		insert into grid_statements
		SELECT  @QuestionID, Responseoption
		FROM OPENXML(@docHandle, N'/options/option')   
		WITH   
		(  
			Responseoption varchar(255) '.'
		) 
		
		EXEC sp_xml_removedocument @docHandle
	end
	select @QuestionID
End
