CREATE proc [dbo].[sp_getBlastReportDataWithSuppressed]
(
	@blastID int, 
	@UDFname varchar(100),
	@UDFdata    varchar(100)     
)
as
    
Begin        
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getBlastReportDataWithSuppressed', GETDATE())
	if (len(@UDFname) > 0 and len(@UDFdata) > 0)
	Begin
		SELECT  
				COUNT(DISTINCT eal.EmailID) AS DistinctCount,
				COUNT(eal.EmailID) AS total, 
				ActionValue
		FROM  
				EmailActivityLog eal join dbo.[fn_Blast_Report_Filter_By_UDF](@blastID,@UDFname,@UDFdata ) t on eal.emailID = t.emailID
		WHERE 
				eal.BlastID = @blastID 
		GROUP BY  ActionValue   
	end
	Else
	Begin
		SELECT  
				COUNT(DISTINCT eal.EmailID) AS DistinctCount,
				COUNT(eal.EmailID) AS total, 
				ActionValue
		FROM  
				EmailActivityLog eal 
		WHERE 
				eal.BlastID = @blastID 
		GROUP BY  ActionValue    
	End 
End
