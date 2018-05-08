CREATE PROC [dbo].[spGetBlastReportDataWithSuppressed]
(
	@blastID int, 
	@UDFname varchar(100),
	@UDFdata varchar(100)     
)
as    
Begin        
	if (len(@UDFname) > 0 and len(@UDFdata) > 0) -- ? 7 day rule
	Begin
		SELECT  
				ISNULL(COUNT(DISTINCT basupp.EmailID),0) AS DistinctCount,
				ISNULL(COUNT(basupp.EmailID),0) AS total, 
				sc.SupressedCode  as ActionTypeCode
		FROM  
				BlastActivitySuppressed basupp WITH (NOLOCK) JOIN SuppressedCodes sc WITH (NOLOCK) ON basupp.SuppressedCodeID = sc.SuppressedCodeID 
				JOIN fnBlast_Report_Filter_By_UDF(@blastID,@UDFname,@UDFdata ) t on basupp.emailID = t.emailID 
		WHERE 
				basupp.BlastID = @blastID
		GROUP BY
				sc.SupressedCode 
	end
	Else
	Begin
		SELECT  
				ISNULL(COUNT(DISTINCT basupp.EmailID),0) AS DistinctCount,
				ISNULL(COUNT(basupp.EmailID),0) AS total, 
				sc.SupressedCode  as ActionTypeCode
		FROM  
				BlastActivitySuppressed basupp WITH (NOLOCK) JOIN SuppressedCodes sc WITH (NOLOCK) ON basupp.SuppressedCodeID = sc.SuppressedCodeID 
		WHERE 
				basupp.BlastID = @blastID 
		GROUP BY  
				sc.SupressedCode 
	End 
End
