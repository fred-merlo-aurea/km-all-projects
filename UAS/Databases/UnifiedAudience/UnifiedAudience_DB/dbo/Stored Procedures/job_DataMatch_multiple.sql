create procedure job_DataMatch_multiple
@matchFields varchar(500),--comma seperated list of fields to match on
@processCode varchar(50),
@sourceFileId int
as
	begin
		set nocount on
		declare @matchTable table
		(
			matchColumn varchar(50)
		)
		insert into @matchTable (matchColumn)
		select *
		from fn_Split(@matchFields,',')


		--fill in rest of logic from job_DataMatching

		exec job_DataMatching @processCode, @sourceFileId

	end
go

