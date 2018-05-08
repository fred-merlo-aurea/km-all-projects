CREATE proc [dbo].[spGetBlastReportData]
(
	@blastID int, 
	@UDFname varchar(100),
	@UDFdata    varchar(100)     
)
as
    
Begin        

	if (len(@UDFname) > 0 and len(@UDFdata) > 0)
	Begin
		SELECT  
				ISNULL(COUNT(DISTINCT bac.EmailID),0) AS DistinctCount,
				ISNULL(COUNT(bac.EmailID),0) AS total, 
				'UNIQCLIQ' as ActionTypeCode
		FROM  
				BlastActivityClicks bac WITH (NOLOCK) JOIN fnBlast_Report_Filter_By_UDF(@blastID,@UDFname,@UDFdata ) t on bac.emailID = t.emailID 
		WHERE 
				bac.BlastID = @blastID 
		UNION        
		SELECT  
				ISNULL(SUM(DistinctCount),0) AS DistinctCount, 
				ISNULL(SUM(total),0) AS total,
				'click'  as ActionTypeCode        
		FROM (        
				SELECT  COUNT(distinct URL) AS DistinctCount, COUNT(bac.EmailID) AS total         
				FROM   BlastActivityClicks bac WITH (NOLOCK) JOIN fnBlast_Report_Filter_By_UDF(@blastID,@UDFname,@UDFdata ) t on bac.emailID = t.emailID 
				WHERE  BlastID = @blastID        
				GROUP BY  URL, bac.EmailID        
			) AS inn   
		UNION  
			SELECT ISNULL(COUNT(DISTINCT bac.EmailID),0) AS DistinctCount,
				   ISNULL(COUNT(bac.EmailID),0) as total,
				   'clickthrough' as ActionTypeCode,
				   BlastID
			FROM BlastActivityClicks bac with(nolock)
				JOIN joinfnBlast_Report_Filter_By_UDF(@blastID,@UDFname,@UDFdata ) t on bas.emailID = t.emailID 
			GROUP BY BlastID  
		UNION  
		
		SELECT ISNULL(COUNT( DISTINCT bas.EmailID),0) AS DistinctCount, ISNULL(COUNT(bas.EmailID),0) AS total, 'send' AS ActionTypeCode 
		FROM BlastActivitySends bas WITH (NOLOCK) JOIN fnBlast_Report_Filter_By_UDF(@blastID,@UDFname,@UDFdata ) t on bas.emailID = t.emailID 
		WHERE BlastID = @blastID 
		UNION  
		SELECT ISNULL(COUNT( DISTINCT bars.EmailID),0) AS DistinctCount, ISNULL(COUNT(bars.EmailID),0) AS total, 'resend' AS ActionTypeCode 
		FROM BlastActivityResends bars WITH (NOLOCK) JOIN fnBlast_Report_Filter_By_UDF(@blastID,@UDFname,@UDFdata ) t on bars.emailID = t.emailID 
		WHERE BlastID = @blastID
		UNION  
		SELECT ISNULL(COUNT( DISTINCT bao.EmailID),0) AS DistinctCount, ISNULL(COUNT(bao.EmailID),0) AS total, 'open' AS ActionTypeCode 
		FROM BlastActivityOpens bao WITH (NOLOCK) JOIN fnBlast_Report_Filter_By_UDF(@blastID,@UDFname,@UDFdata ) t on bao.emailID = t.emailID 
		WHERE BlastID = @blastID       
		UNION 
		SELECT ISNULL(COUNT( DISTINCT bab.EmailID),0) AS DistinctCount, ISNULL(COUNT(bab.EmailID),0) AS total, 'bounce' AS ActionTypeCode 
		FROM BlastActivityBounces bab WITH (NOLOCK) JOIN fnBlast_Report_Filter_By_UDF(@blastID,@UDFname,@UDFdata ) t on bab.emailID = t.emailID 
		WHERE BlastID = @blastID          
		UNION
		SELECT  
				ISNULL(COUNT(DISTINCT bau.EmailID),0) AS DistinctCount,
				ISNULL(COUNT(bau.EmailID),0) AS total, 
				uc.UnsubscribeCode  as ActionTypeCode
		FROM  
				BlastActivityUnSubscribes bau WITH (NOLOCK) JOIN UnsubscribeCodes uc WITH (NOLOCK) ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID JOIN fnBlast_Report_Filter_By_UDF(@blastID,@UDFname,@UDFdata ) t on bau.emailID = t.emailID 
		WHERE 
				bau.BlastID = @blastID 
		GROUP BY  uc.UnsubscribeCode
		UNION
		SELECT  
				ISNULL(COUNT(DISTINCT basupp.EmailID),0) AS DistinctCount,
				ISNULL(COUNT(basupp.EmailID),0) AS total, 
				sc.SupressedCode  as ActionTypeCode
		FROM  
				BlastActivitySuppressed basupp WITH (NOLOCK) JOIN SuppressedCodes sc WITH (NOLOCK) ON basupp.SuppressedCodeID = sc.SuppressedCodeID JOIN fnBlast_Report_Filter_By_UDF(@blastID,@UDFname,@UDFdata ) t on basupp.emailID = t.emailID 
		WHERE 
				basupp.BlastID = @blastID 
		GROUP BY  sc.SupressedCode
		UNION        
		SELECT ISNULL(COUNT( DISTINCT baconv.EmailID),0) AS DistinctCount, ISNULL(COUNT(baconv.EmailID),0) AS total, 'conversion' AS ActionTypeCode FROM BlastActivityConversion baconv WITH (NOLOCK) JOIN fnBlast_Report_Filter_By_UDF(@blastID,@UDFname,@UDFdata ) t on baconv.emailID = t.emailID WHERE BlastID = @blastID                        
		UNION 
		SELECT ISNULL(COUNT( DISTINCT bar.EmailID),0) AS DistinctCount, ISNULL(COUNT(bar.EmailID),0) AS total, 'refer' AS ActionTypeCode FROM BlastActivityRefer bar WITH (NOLOCK) JOIN fnBlast_Report_Filter_By_UDF(@blastID,@UDFname,@UDFdata ) t on bar.emailID = t.emailID WHERE BlastID = @blastID  
	end
	Else
	Begin
		SELECT  
				ISNULL(COUNT(DISTINCT bac.EmailID),0) AS DistinctCount,
				ISNULL(COUNT(bac.EmailID),0) AS total, 
				'UNIQCLIQ' as ActionTypeCode
		FROM  
				BlastActivityClicks bac WITH (NOLOCK)
		WHERE 
				bac.BlastID = @blastID 
		UNION        
		SELECT  
				ISNULL(SUM(DistinctCount),0) AS DistinctCount, 
				ISNULL(SUM(total),0) AS total,
				'click'  as ActionTypeCode        
		FROM (        
				SELECT  COUNT(distinct URL) AS DistinctCount, COUNT(bac.EmailID) AS total         
				FROM   BlastActivityClicks bac WITH (NOLOCK)
				WHERE  BlastID = @blastID        
				GROUP BY  URL, bac.EmailID        
			) AS inn     
		UNION  
			SELECT ISNULL(COUNT(DISTINCT bac.EmailID),0) AS DistinctCount,
				   ISNULL(COUNT(bac.EmailID),0) as total,
				   'clickthrough' as ActionTypeCode
			FROM BlastActivityClicks bac with(nolock)
			WHERE bac.BlastID = @blastID
			GROUP BY BlastID  
		UNION
		SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'send' AS ActionTypeCode FROM BlastActivitySends WITH (NOLOCK) WHERE BlastID = @blastID  
		UNION 
		SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'resend' AS ActionTypeCode FROM BlastActivityResends WITH (NOLOCK) WHERE BlastID = @blastID   
		UNION 
		SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'open' AS ActionTypeCode FROM BlastActivityOpens WITH (NOLOCK) WHERE BlastID = @blastID       
		UNION 
		SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'bounce' AS ActionTypeCode FROM BlastActivityBounces WITH (NOLOCK) WHERE BlastID = @blastID          
		UNION
		SELECT  
				ISNULL(COUNT(DISTINCT bau.EmailID),0) AS DistinctCount,
				ISNULL(COUNT(bau.EmailID),0) AS total, 
				uc.UnsubscribeCode  as ActionTypeCode
		FROM  
				BlastActivityUnSubscribes bau WITH (NOLOCK) JOIN UnsubscribeCodes uc WITH (NOLOCK) ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID 
		WHERE 
				bau.BlastID = @blastID 
		GROUP BY  uc.UnsubscribeCode
		UNION
		SELECT  
				ISNULL(COUNT(DISTINCT basupp.EmailID),0) AS DistinctCount,
				ISNULL(COUNT(basupp.EmailID),0) AS total, 
				sc.SupressedCode  as ActionTypeCode
		FROM  
				BlastActivitySuppressed basupp WITH (NOLOCK) JOIN SuppressedCodes sc WITH (NOLOCK) ON basupp.SuppressedCodeID = sc.SuppressedCodeID 
		WHERE 
				basupp.BlastID = @blastID 
		GROUP BY  sc.SupressedCode
		UNION 
		SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'conversion' AS ActionTypeCode FROM BlastActivityConversion WITH (NOLOCK) WHERE BlastID = @blastID                        
		UNION 
		SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'refer' AS ActionTypeCode FROM BlastActivityRefer WITH (NOLOCK) WHERE BlastID = @blastID                 
	End 
End
