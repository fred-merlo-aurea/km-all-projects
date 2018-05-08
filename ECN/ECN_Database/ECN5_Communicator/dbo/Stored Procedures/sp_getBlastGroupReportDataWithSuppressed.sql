CREATE proc [dbo].[sp_getBlastGroupReportDataWithSuppressed]
(
	@blastGroupID int   
)
as
    
Begin       
	SET NOCOUNT ON;
	declare @blastIDs varchar(4000)
    SELECT @blastIDs = BlastIDs from BlastGrouping WHERE BlastGroupID = @blastGroupID
	
	SELECT sum(DistinctCount) AS 'DistinctCount', sum(total) AS 'total' , ActionValue from (
		SELECT  
				COUNT(DISTINCT eal.EmailID) AS DistinctCount,
				COUNT(eal.EmailID) AS total, 
				ActionValue
		FROM  
				EmailActivityLog eal 
				JOIN (SELECT ITEMS FROM DBO.fn_split(@blastIDs, ',')  where items <> '0') ids ON ids.items = eal.blastID
		GROUP BY  ActionValue    
		) outer1 
	GROUP BY ActionValue
End
