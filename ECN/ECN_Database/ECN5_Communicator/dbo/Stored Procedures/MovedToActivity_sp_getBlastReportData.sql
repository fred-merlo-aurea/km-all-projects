CREATE proc [dbo].[MovedToActivity_sp_getBlastReportData]
(
	@blastID int, 
	@UDFname varchar(100),
	@UDFdata    varchar(100)     
)
as
    
Begin        

	if (len(@UDFname) > 0 and len(@UDFdata) > 0)
	Begin
		INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getBlastReportData', GETDATE())  
		SELECT  
				COUNT(DISTINCT eal.EmailID) AS DistinctCount,
				COUNT(eal.EmailID) AS total, 
				CASE WHEN ActionTypeCode = 'click' THEN 'UNIQCLIQ' ELSE ActionTypeCode END as ActionTypeCode
		FROM  
				EmailActivityLog eal join dbo.[fn_Blast_Report_Filter_By_UDF](@blastID,@UDFname,@UDFdata ) t on eal.emailID = t.emailID
		WHERE 
				eal.BlastID = @blastID 
		GROUP BY  ActionTypeCode         
		UNION        
		SELECT  
				ISNULL(SUM(DistinctCount),0) AS DistinctCount, 
				ISNULL(SUM(total),0) AS total,
				'click'         
		FROM (        
				SELECT  COUNT(distinct ActionValue) AS DistinctCount, COUNT(eal.EmailID) AS total         
				FROM   EmailActivityLog eal join dbo.[fn_Blast_Report_Filter_By_UDF](@blastID,@UDFname,@UDFdata ) t on eal.emailID = t.emailID        
				WHERE  ActionTypeCode = 'click' AND BlastID = @blastID        
				GROUP BY  ActionValue, eal.EmailID        
			) AS inn     
	end
	Else
	Begin
		SELECT  
				COUNT(DISTINCT eal.EmailID) AS DistinctCount,
				COUNT(eal.EmailID) AS total, 
				CASE WHEN ActionTypeCode = 'click' THEN 'UNIQCLIQ' ELSE ActionTypeCode END  as ActionTypeCode
		FROM  
				EmailActivityLog eal 
		WHERE 
				eal.BlastID = @blastID 
		GROUP BY  ActionTypeCode         
		UNION        
		SELECT  
				ISNULL(SUM(DistinctCount),0) AS DistinctCount, 
				ISNULL(SUM(total),0) AS total,
				'click'         
		FROM (        
				SELECT  COUNT(distinct ActionValue) AS DistinctCount, COUNT(eal.EmailID) AS total         
				FROM   EmailActivityLog eal 
				WHERE  ActionTypeCode = 'click' AND BlastID = @blastID        
				GROUP BY  ActionValue, eal.EmailID        
			) AS inn     
	End 
End
