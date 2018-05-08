CREATE PROCEDURE [dbo].[sp_GetBlastReport1] 
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
    declare @RecordNoStart int,  
			@RecordNoEnd int  
  
	Set @RecordNoStart = (@PageNo * @PageSize) + 1  
	Set @RecordNoEnd = (@PageNo * @PageSize) + 50  
  
	Declare @reportdata TABLE (id int identity(1,1), emailID int, emailaddress varchar(255), ActionDate datetime)  
  
 
	if @ReportType = 'send'  
	Begin  

		SET ROWCOUNT @RecordNoEnd 

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

		SET ROWCOUNT 0 

	END  
	else if @ReportType = 'noclick'  
	Begin  
		if len(rtrim(ltrim(@ISP))) > 0
		Begin
			SELECT	eal.ActionDate, e.EmailAddress
			from	EmailActivityLog eal join Emails e  ON e.EmailID = eal.EmailID 
			WHERE BlastID=@blastID AND 
				ActionTypeCode in ('send','testsend') and 
				e.emailaddress like '%' + @ISP  and
				e.emailID not in (SELECT eal.EmailID FROM EmailActivityLog eal WHERE	BlastID=@blastID AND ActionTypeCode ='click')
			order by ActionDate desc, EmailAddress  
		
		end
		else
		Begin
			SELECT	eal.ActionDate, e.EmailAddress
			from	EmailActivityLog eal join Emails e  ON e.EmailID = eal.EmailID 
			WHERE BlastID=@blastID AND 
				ActionTypeCode in ('send','testsend') and 
				e.emailID not in (SELECT eal.EmailID FROM EmailActivityLog eal WHERE	BlastID=@blastID AND ActionTypeCode ='click')
			order by ActionDate desc, EmailAddress  
		end
	end
	else if @ReportType = 'noopen'  
	Begin 
		if len(rtrim(ltrim(@ISP))) > 0
		Begin
			SELECT	eal.ActionDate, e.EmailAddress
			from	EmailActivityLog eal join Emails e  ON e.EmailID = eal.EmailID 
			WHERE BlastID=@blastID AND 
				ActionTypeCode in ('send','testsend') and 
				e.emailaddress like '%' + @ISP  and
				e.emailID not in (SELECT eal.EmailID FROM EmailActivityLog eal WHERE	BlastID=@blastID AND ActionTypeCode ='open')
			order by ActionDate desc, EmailAddress  
		
		end
		else
		Begin

			SELECT	count(eal.EAID)
			from	EmailActivityLog eal with (NOLOCK)
			WHERE 	BlastID=@blastID AND 
					ActionTypeCode in ('send','testsend')
					and emailID not in (SELECT EmailID FROM EmailActivityLog with (NOLOCK) WHERE BlastID=@blastID AND ActionTypeCode ='open')
			--order by ActionDate desc, EmailAddress  

		end
	end
End    
  
--exec sp_GetBlastReport1 90536, 'noopen', '', '', 0, 50
