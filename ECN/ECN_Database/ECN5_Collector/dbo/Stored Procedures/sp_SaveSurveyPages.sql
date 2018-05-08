CREATE proc [dbo].[sp_SaveSurveyPages]
(
	@PageID int,
	@SurveyID int,
	@PageHeader varchar(255),
	@PageDesc	varchar(255),
	@PageNumber	int
)
as
Begin

	declare @maxPageNumber int
	
	If @PageID > 0
	Begin
		Update	Page 
		Set		PageHeader = @PageHeader,
				PageDesc = @PageDesc
		where 
				PageID = @PageID
				
				select @PageID
		
	end
	else
	Begin
		select @maxPageNumber = isnull(max(number),0) from page where SurveyID = @SurveyID

		if @PageNumber = 0
		Begin
			insert into Page (SurveyID, PageHeader, PageDesc, number) values
				(@SurveyID, @PageHeader, @PageDesc, @maxPageNumber+1)
			select @@IDENTITY
		end
		else
		Begin
			
			update Page
			Set number = number+1
			where SurveyID = @SurveyID and number >= @PageNumber

			insert into Page (SurveyID, PageHeader, PageDesc, number) values
				(@SurveyID, @PageHeader, @PageDesc, @PageNumber)
			select @@IDENTITY
		end
	end

end
