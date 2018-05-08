--exec sp_getBlastReportData 1568907, '', '', 1
CREATE proc [dbo].[sp_getBlastReportData]
(
	@blastID int, 
	@UDFname varchar(100) = '',
	@UDFdata    varchar(100) = '',
	@CustomerID int     
)
as
    
Begin        

	if (len(@UDFname) > 0 and len(@UDFdata) > 0)
	Begin
		select 'add logic'
	end
	Else
	Begin
		select 
			'Send' as ActionTypeCode, COUNT(distinct bas.emailID) as DistinctCount, COUNT(bas.SendID) as Total 
		from 
			BlastActivitySends bas with (NOLOCK) 
			join ecn5_communicator..Blast b with (NOLOCK) on bas.BlastID = b.BlastID
		where 
			bas.BlastID = @blastID and
			b.CustomerID = @CustomerID
		union all
		select 
			'Open', COUNT(distinct bao.emailID), COUNT(bao.OpenID) 
		from 
			BlastActivityOpens bao with (NOLOCK) 
			join ecn5_communicator..Blast b with (NOLOCK) on bao.BlastID = b.BlastID
		where 
			bao.BlastID = @blastID and
			b.CustomerID = @CustomerID
		union all
		select 
			'Bounce', COUNT(distinct bab.emailID), COUNT(bab.BounceID) 
		from 
			BlastActivityBounces bab with (NOLOCK) 
			join ecn5_communicator..Blast b with (NOLOCK) on bab.BlastID = b.BlastID
		where 
			bab.BlastID = @blastID and
			b.CustomerID = @CustomerID
		union all
		select 
			'Suppressed', COUNT(distinct basupp.emailID), COUNT(basupp.SuppressID) 
		from 
			BlastActivitySuppressed basupp with (NOLOCK) 
			join ecn5_communicator..Blast b with (NOLOCK) on basupp.BlastID = b.BlastID
		where 
			basupp.BlastID = @blastID and
			b.CustomerID = @CustomerID
		union all
		select 
			'UnSubscribes', COUNT(distinct baunsub.emailID), COUNT(baunsub.UnsubscribeID) 
		from 
			BlastActivityUnSubscribes baunsub with (NOLOCK) 
			join ecn5_communicator..Blast b with (NOLOCK) on baunsub.BlastID = b.BlastID
		where 
			baunsub.BlastID = @blastID and
			b.CustomerID = @CustomerID
		union all
		select 
			'Conversion', COUNT(distinct baconv.emailID), COUNT(baconv.ConversionID) 
		from 
			BlastActivityConversion baconv with (NOLOCK) 
			join ecn5_communicator..Blast b with (NOLOCK) on baconv.BlastID = b.BlastID
		where 
			baconv.BlastID = @blastID and
			b.CustomerID = @CustomerID 
		union all
		select 
			'ActivityRefer', COUNT(distinct barefer.emailID), COUNT(barefer.ReferID) 
		from 
			BlastActivityRefer barefer with (NOLOCK) 
			join ecn5_communicator..Blast b with (NOLOCK) on barefer.BlastID = b.BlastID
		where 
			barefer.BlastID = @blastID and
			b.CustomerID = @CustomerID 
		union all
		select 
			'Resend', COUNT(distinct bar.emailID), COUNT(bar.ResendID) 
		from 
			BlastActivityResends bar with (NOLOCK) 
			join ecn5_communicator..Blast b with (NOLOCK) on bar.BlastID = b.BlastID
		where 
			bar.BlastID = @blastID and
			b.CustomerID = @CustomerID 
		union all
		SELECT  
				'click'   ,
				ISNULL(SUM(DistinctCount),0) AS DistinctCount, 
				ISNULL(SUM(total),0) AS total
			     
		FROM (        
				SELECT  COUNT(distinct bac.URL) AS DistinctCount, COUNT(bac.EmailID) AS total         
				FROM   BlastActivityClicks bac  with (NOLOCK) join ecn5_communicator..Blast b with (NOLOCK) on bac.BlastID = b.BlastID
				WHERE  bac.BlastID = @blastID and b.CustomerID = @CustomerID   
				GROUP BY  bac.URL,  bac.EmailID        
			) AS inn     
	End 
End