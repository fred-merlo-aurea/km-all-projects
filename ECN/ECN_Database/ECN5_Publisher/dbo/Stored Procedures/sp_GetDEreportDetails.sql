CREATE proc [dbo].[sp_GetDEreportDetails]
(
	@EditionID varchar(10),     
	@BlastID int,   
	@ReportType  varchar(25),    
	@PageNo int,    
	@PageSize int 
)
as   
Begin    

    declare @RecordNoStart int,    
			@RecordNoEnd int    

	Set @RecordNoStart = (@PageNo * @PageSize) + 1    
	Set @RecordNoEnd = (@PageNo * @PageSize) + @PageSize    
    
	Declare @reportdata TABLE (id int identity(1,1), EAID int)    
    
    if @ReportType = 'visit' 
	Begin
		SET ROWCOUNT @RecordNoEnd    

		Insert into @reportdata (EAID)
		select  EAID 
		from	editionactivitylog eal 
		where    
				eal.EditionID = @EditionID and 
				ActionTypecode='Visit'   and 
				blastID = (case when @blastID <= 0 then blastID else @blastID end)  
		order by ActionDate desc    

		SET ROWCOUNT 0

		select   Count(EAID) as total
		from 
				editionactivitylog eal 
		where    
				eal.EditionID = @EditionID and 
				ActionTypecode='Visit'   and 
				blastID = (case when @blastID <= 0 then blastID else @blastID end)  

		select   
			(case when isnull(emailaddress,'') = '' then 'Anonymous' else Emailaddress end) as EmailAddress,    
			PageNumber,  
			ActionDate,    
			IPAddress as IP    
		from 
				editionactivitylog eal join 
				@reportdata r on eal.eaid = r.eaid join
				Page p on p.pageID = eal.pageID left outer join 
				ecn5_communicator..emails e on e.emailID = eal.emailID    
		order by ActionDate desc    
	end    
End
