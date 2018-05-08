CREATE PROCEDURE [dbo].[spGetBlastReport]   
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
  
			
 	SET ROWCOUNT @RecordNoEnd  
 
	if @ReportType = 'send'  
	Begin  
		if len(rtrim(ltrim(@ISP))) > 0
		Begin
			insert into @reportdata (emailID, emailaddress, actiondate)  
			SELECT 
				e.emailID, e.EmailAddress, bas.SendTime as ActionDate
			from 
				BlastActivitySends bas with (nolock)
				join ecn5_communicator..Emails e  with (nolock) ON e.EmailID = bas.EmailID  
			WHERE 
				BlastID=@blastID and e.emailaddress like '%' + @ISP   
			order by 
				ActionDate desc, EmailAddress  
		  
	
			SELECT 
				count(SendID) as 'Total'  
			from 
				BlastActivitySends bas with (nolock) 
				join ecn5_communicator..Emails e  with (nolock) ON e.EmailID = bas.EmailID  
			WHERE 
				BlastID=@blastID AND e.emailaddress like '%' + @ISP  

			SELECT 
				r.ActionDate As SendTime, r.EmailAddress   
			FROM  
				@reportdata r 

		end
		else
		Begin
			insert into @reportdata (emailID, actiondate)  
			SELECT 
				bas.emailID, bas.SendTime as ActionDate  
			from 
				BlastActivitySends bas with (nolock)
			WHERE 
				BlastID=@blastID
			order by 
				ActionDate desc
			
			SELECT 
				count(SendID) as 'Total'  
			from 
				BlastActivitySends bas  with (nolock)
			WHERE 
				BlastID=@blastID

			SELECT 
				r.ActionDate As SendTime, e.EmailAddress   
			FROM  
				@reportdata r 
				join ecn5_communicator..emails e on e.emailID = r.emailID 
			order by 
				ActionDate desc, e.EmailAddress  
		end
	END  
	else if @ReportType = 'noclick'  
	Begin  
		if len(rtrim(ltrim(@ISP))) > 0
		Begin
			insert into @reportdata (emailID, emailaddress, actiondate)  
			SELECT	
				e.emailID, e.EmailAddress, bas.SendTime as ActionDate  
			from	
				BlastActivitySends bas  with (nolock)
				join ecn5_communicator..Emails e with (nolock) ON e.EmailID = bas.EmailID 
			WHERE 
				BlastID=@blastID AND 
				e.emailaddress like '%' + @ISP  and
				e.emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac with (nolock) WHERE BlastID=@blastID)
			order by 
				ActionDate desc, EmailAddress  

			SELECT	
				count(SendID) as 'Total'  
			from	
				BlastActivitySends bas with (nolock) 
				join ecn5_communicator..Emails e with (nolock) ON e.EmailID = bas.EmailID 
			WHERE	
				BlastID=@blastID AND 
				e.emailaddress like '%' + @ISP  and
				e.emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac with (nolock) WHERE	BlastID=@blastID)

			SELECT 
				r.ActionDate, r.EmailAddress   
			FROM  
				@reportdata r  
		end
		else
		Begin
			insert into @reportdata (emailID, actiondate)  
			SELECT	
				emailID, ActionDate  
			from	
				EmailActivityLog eal with (nolock) 
			WHERE	
				BlastID=@blastID AND 
				ActionTypeCode in ('send','testsend') and 
				emailID not in (SELECT distinct eal.EmailID FROM BlastActivityClicks bac with (nolock) WHERE	BlastID=@blastID)
			order by 
				ActionDate desc

			SELECT	
				count(SendID) as 'Total'  
			from	
				BlastActivitySends bas with (nolock) 
			WHERE	
				BlastID=@blastID AND 
				emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac with (nolock) WHERE	BlastID=@blastID)

			SELECT 
				r.ActionDate, e.EmailAddress   
			FROM  
				@reportdata r 
				join ecn5_communicator..emails e with (nolock) on e.emailID = r.emailID 
			order by 
				ActionDate desc, e.EmailAddress 
		end
	end
	else if @ReportType = 'noopen'  
	Begin 
		if len(@ISP) > 0
		Begin

			insert into @reportdata (emailID, emailaddress, actiondate)  
			SELECT	
				e.emailID, e.EmailAddress, bas.SendTime as ActionDate  
			from	
				BlastActivitySends bas with (nolock) 
				join ecn5_communicator..Emails e with (nolock)  ON e.EmailID = bas.EmailID 
			WHERE	
				BlastID=@blastID AND 
				e.emailaddress like '%' + @ISP  and
				e.emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao with (nolock) WHERE	BlastID=@blastID)
			order by 
				ActionDate desc, EmailAddress  

			SELECT	
				count(SendID) as 'Total'  
			from	
				BlastActivitySends bas with (nolock) 
				join ecn5_communicator..Emails e with (nolock)  ON e.EmailID = bas.EmailID 
			WHERE	
				BlastID=@blastID AND 
				e.emailaddress like '%' + @ISP  and
				e.emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao with (nolock) WHERE	BlastID=@blastID)

			SELECT 
				r.ActionDate, r.EmailAddress   
			FROM  
				@reportdata r  
		end
		else
		begin

			insert into @reportdata (emailID, actiondate)  
			SELECT	
				emailID, bas.SendTime as ActionDate  
			from	
				BlastActivitySends bas with (nolock)
			WHERE	
				BlastID=@blastID AND 
				emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao with (nolock) WHERE	BlastID=@blastID)
			order by 
				ActionDate desc

			SELECT	
				count(SendID) as 'Total'	
			from 
				BlastActivitySends bas with (nolock) 
			WHERE	
				BlastID=@blastID AND 
				emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao with (nolock) WHERE	BlastID=@blastID)

			SELECT 
				r.ActionDate, e.EmailAddress   
			FROM  
				@reportdata r 
				join ecn5_communicator..Emails e with (nolock) on e.emailID = r.emailID 
			order by 
				ActionDate desc, e.EmailAddress 
		end
	end

	SET ROWCOUNT 0 

End
