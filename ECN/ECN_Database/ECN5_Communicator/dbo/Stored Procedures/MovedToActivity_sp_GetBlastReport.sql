CREATE PROCEDURE [dbo].[MovedToActivity_sp_GetBlastReport]   
(  
	@blastID varchar(10),   
	@ReportType  varchar(25),  
	@FilterType varchar(25),   
	@ISP varchar(100),  
	@PageNo int,  
	@PageSize int  
)       
as        
  
Begin  
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_GetBlastReport', GETDATE())     
    declare @RecordNoStart int,  
			@RecordNoEnd int  
  
	Set @RecordNoStart = (@PageNo * @PageSize) + 1  
	Set @RecordNoEnd = (@PageNo * @PageSize) + 50  

  	Declare @reportdata TABLE (id int identity(1,1), emailID int, emailaddress varchar(255), ActionDate datetime)  
  
			
 	SET ROWCOUNT @RecordNoEnd  
 
	if @ReportType = 'send'  
	Begin  
		if len(rtrim(ltrim(@ISP))) > 0
		Begin
			insert into @reportdata (emailID, emailaddress, actiondate)  
			SELECT e.emailID, e.EmailAddress, eal.ActionDate  
			from EmailActivityLog eal join Emails e  ON e.EmailID = eal.EmailID  
			WHERE BlastID=@blastID AND ActionTypeCode in ('send','testsend')  and e.emailaddress like '%' + @ISP   
			order by ActionDate desc, EmailAddress  
		  
	
			SELECT count(EAID) as 'Total'  
			from EmailActivityLog eal join Emails e  ON e.EmailID = eal.EmailID  
			WHERE BlastID=@blastID AND ActionTypeCode in ('send','testsend') and e.emailaddress like '%' + @ISP  

			SELECT r.ActionDate As SendTime, r.EmailAddress   
			FROM  @reportdata r 

		end
		else
		Begin
			insert into @reportdata (emailID, actiondate)  
			SELECT eal.emailID, eal.ActionDate  
			from EmailActivityLog eal
			WHERE BlastID=@blastID AND ActionTypeCode in ('send','testsend')   
			order by ActionDate desc
			
			SELECT count(EAID) as 'Total'  
			from EmailActivityLog eal 
			WHERE BlastID=@blastID AND ActionTypeCode in ('send','testsend') 

			SELECT r.ActionDate As SendTime, e.EmailAddress   
			FROM  @reportdata r join emails e on e.emailID = r.emailID 
			order by ActionDate desc, e.EmailAddress  
		end
	END  
	else if @ReportType = 'noclick'  
	Begin  
		if len(rtrim(ltrim(@ISP))) > 0
		Begin
			insert into @reportdata (emailID, emailaddress, actiondate)  
			SELECT	e.emailID, e.EmailAddress, eal.ActionDate  
			from	EmailActivityLog eal join Emails e  ON e.EmailID = eal.EmailID 
			WHERE BlastID=@blastID AND 
				ActionTypeCode in ('send','testsend') and 
				e.emailaddress like '%' + @ISP  and
				e.emailID not in (SELECT distinct eal.EmailID FROM EmailActivityLog eal WHERE	BlastID=@blastID AND ActionTypeCode in ('click'))
			order by ActionDate desc, EmailAddress  

			SELECT	count(EAID) as 'Total'  
			from	EmailActivityLog eal join Emails e  ON e.EmailID = eal.EmailID 
			WHERE	
					BlastID=@blastID AND 
					ActionTypeCode in ('send','testsend') and 
					e.emailaddress like '%' + @ISP  and
					e.emailID not in (SELECT distinct eal.EmailID FROM EmailActivityLog eal WHERE	BlastID=@blastID AND ActionTypeCode in ('click'))

			SELECT r.ActionDate, r.EmailAddress   
			FROM  @reportdata r  
		end
		else
		Begin
			insert into @reportdata (emailID, actiondate)  
			SELECT	emailID, ActionDate  
			from	EmailActivityLog eal 
			WHERE	BlastID=@blastID AND 
					ActionTypeCode in ('send','testsend') and 
					emailID not in (SELECT distinct eal.EmailID FROM EmailActivityLog eal WHERE	BlastID=@blastID AND ActionTypeCode in ('click'))
			order by ActionDate desc

			SELECT	count(EAID) as 'Total'  
			from	EmailActivityLog eal 
			WHERE	
					BlastID=@blastID AND 
					ActionTypeCode in ('send','testsend') and 
					emailID not in (SELECT distinct eal.EmailID FROM EmailActivityLog eal WHERE	BlastID=@blastID AND ActionTypeCode in ('click'))

			SELECT r.ActionDate, e.EmailAddress   
			FROM  @reportdata r join emails e on e.emailID = r.emailID 
			order by ActionDate desc, e.EmailAddress 
		end
	end
	else if @ReportType = 'noopen'  
	Begin 
		if len(@ISP) > 0
		Begin

			insert into @reportdata (emailID, emailaddress, actiondate)  
			SELECT	e.emailID, e.EmailAddress, eal.ActionDate  
			from	EmailActivityLog eal join Emails e  ON e.EmailID = eal.EmailID 
			WHERE	BlastID=@blastID AND 
					ActionTypeCode in ('send','testsend') and 
					e.emailaddress like '%' + @ISP  and
					e.emailID not in (SELECT distinct eal.EmailID FROM EmailActivityLog eal WHERE	BlastID=@blastID AND ActionTypeCode in ('open'))
			order by ActionDate desc, EmailAddress  

			SELECT	count(EAID) as 'Total'  
			from	EmailActivityLog eal join Emails e  ON e.EmailID = eal.EmailID 
			WHERE	
					BlastID=@blastID AND 
					ActionTypeCode in ('send','testsend') and 
					e.emailaddress like '%' + @ISP  and
					e.emailID not in (SELECT distinct eal.EmailID FROM EmailActivityLog eal WHERE	BlastID=@blastID AND ActionTypeCode in ('open'))

			SELECT r.ActionDate, r.EmailAddress   
			FROM  @reportdata r  
		end
		else
		begin

			insert into @reportdata (emailID, actiondate)  
			SELECT	emailID,  
					ActionDate  
			from	
					EmailActivityLog eal 
			WHERE	
					BlastID=@blastID AND 
					ActionTypeCode in ('send','testsend') and 
					emailID not in (SELECT distinct eal.EmailID FROM EmailActivityLog eal WHERE	BlastID=@blastID AND ActionTypeCode in ('open'))
			order by 
					ActionDate desc

			SELECT	count(EAID) as 'Total'	from EmailActivityLog eal 
			WHERE	
					BlastID=@blastID AND 
					ActionTypeCode in ('send','testsend') and 
					emailID not in (SELECT distinct eal.EmailID FROM EmailActivityLog eal WHERE	BlastID=@blastID AND ActionTypeCode in ('open'))

			SELECT r.ActionDate, e.EmailAddress   
			FROM  @reportdata r join emails e on e.emailID = r.emailID 
			order by ActionDate desc, e.EmailAddress 
		end
	end

	SET ROWCOUNT 0 

End
--EXEC sp_GetBlastReport 218366, 'send', '', '', 0, 1000
