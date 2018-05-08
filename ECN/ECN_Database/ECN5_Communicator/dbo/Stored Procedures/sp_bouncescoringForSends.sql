CREATE PROCEDURE [dbo].[sp_bouncescoringForSends] 
as
Begin
	set nocount on
	declare @sends table  
	 (  
	   EAID int primary key,	    
	   EmailID  int,      
	   blastID  int  
	  )  

	declare @startdate datetime,
			@enddate datetime

	 insert into @sends  
	 select eal.EAID, eal.EmailID, eal.blastID from emailactivitylog eal with (NOLOCK) where actionTypecode = 'send' and processed='n' and ActionDate > DATEADD(dd, -5, getdate())

	if @@ROWCOUNT > 0
	Begin
		-- Update emails - decrement bouncescore with -1 for each send  
		update emails       
		set bouncescore = (case when (ISNULL(bouncescore,0) + (b.bscore * -1)) < 0 then -1 else (ISNULL(bouncescore,0) + (b.bscore * -1)) end)       
		from emails e join       
		(select emailID, count(emailID) as bscore from @sends group by emailID) b on e.emailID = b.emailID       


		/* Update the EAL Table & set the Processed Flag to Y.*/      
		update emailactivitylog set processed ='Y' from emailactivitylog eal join @sends s on eal.EAID = s.EAID     
	End
End
