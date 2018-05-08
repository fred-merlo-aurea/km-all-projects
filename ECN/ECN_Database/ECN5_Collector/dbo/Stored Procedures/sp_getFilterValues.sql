CREATE proc [dbo].[sp_getFilterValues]
(
	@filterstr varchar(8000)
)
as
Begin
/*	
select	100 as QuestionID, 100 as statementID, 100 as OptionID,
		'abcdefghijklmnopqrsturvwxyzabcdefghijklmnopqrsturvwxyz ' as question,
		'abcdefghijklmnopqrsturvwxyzabcdefghijklmnopqrsturvwxyzabcdefghijklmnopqrsturvwxyzabcdefghijklmnopqrsturvwxyz abcdefghijklmnopqrsturvwxyzabcdefghijklmnopqrsturvwxyz ' as QuestionText,	
		'abcdefghijklmnopqrsturvwxyzabcdefghijklmnopqrsturvwxyzabcdefghijklmnopqrsturvwxyzabcdefghijklmnopqrsturvwxyz abcdefghijklmnopqrsturvwxyzabcdefghijklmnopqrsturvwxyz ' as Filter

*/
Declare
		@tmpfilter varchar(100),
		@tempstr varchar(100),
		@statementID int,
		@optionID int

	if len(ltrim(rtrim(@Filterstr))) > 0
	Begin

		declare @filter TABLE (questionID int, statementID int, OptionID int)

		DECLARE cFilter CURSOR FOR select * from dbo.fn_split(@Filterstr, ',')

		OPEN cFilter
		FETCH NEXT FROM cFilter INTO @tmpfilter

		WHILE @@FETCH_STATUS = 0
		BEGIN
			set	@tempstr = substring(@tmpfilter,charindex('|',@tmpfilter)+1, len(@tmpfilter))
			set @statementID = 0
			set @optionID = 0

			if (charindex('|',@tempstr) > 0)
			Begin
				set @statementID = substring(@tempstr,1,charindex('|',@tempstr)-1)
				set @optionID = substring(@tempstr,charindex('|',@tempstr)+1, len(@tempstr))
			End
			else
			Begin
				set @optionID = @tempstr
			end

			insert into @filter 
			select substring(@tmpfilter,1,charindex('|',@tmpfilter)-1), isnull(@statementID,0), @optionID 

			FETCH NEXT FROM cFilter INTO @tmpfilter

		end

		CLOSE cFilter
		DEALLOCATE cFilter
			
	end

		select	f.*, 
				'Q. ' + Convert(varchar,q.number) as Question, 
				q.QuestionText, 
				case when isnull(gs.GridStatement,'') = '' then (case when isnull(ro.OptionValue,'') = '' then 'Answered' else ro.OptionValue end) else isnull(gs.GridStatement,'') + ' - ' + ro.OptionValue  end as Filter 
		from	@filter f join 
				question q on f.QuestionID = q.QuestionID left outer join
				response_options ro on ro.QuestionID = q.QuestionID and f.OptionID = ro.OptionID left outer join
				grid_statements gs on gs.QuestionID = q.QuestionID  and f.statementID = gs.GridStatementID

end
