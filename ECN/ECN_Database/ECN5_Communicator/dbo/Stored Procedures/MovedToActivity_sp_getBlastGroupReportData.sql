CREATE PROCEDURE [dbo].[MovedToActivity_sp_getBlastGroupReportData] 
	-- Add the parameters for the stored procedure here
	@blastGroupID int
AS
BEGIN
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getBlastGroupReportData', GETDATE()) 
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @blastIDs varchar(4000)
    -- Insert statements for procedure here
	SELECT @blastIDs = BlastIDs from BlastGrouping WHERE BlastGroupID = @blastGroupID

	SELECT sum(DistinctCount) AS 'DistinctCount', sum(total) AS 'total' , ActionTypeCode from (
		SELECT  COUNT(DISTINCT eal.EmailID) AS DistinctCount,COUNT(eal.EmailID) AS total, 
			CASE WHEN ActionTypeCode = 'click' THEN 'UNIQCLIQ' ELSE ActionTypeCode END AS 'ActionTypeCode' , BlastID 
		FROM  EmailActivityLog eal JOIN (SELECT ITEMS FROM DBO.fn_split(@blastIDs, ',')  where items <> '0') ids ON ids.items = eal.blastID 
		GROUP BY  ActionTypeCode, blastID          
		UNION          
		SELECT  ISNULL(SUM(DistinctCount),0) AS DistinctCount, ISNULL(SUM(total),0) AS total,'click' , blastID        
		FROM (          
			SELECT  COUNT(distinct ActionValue) AS DistinctCount, COUNT(EmailID) AS total , BlastID         
			FROM   EmailActivityLog eal JOIN (SELECT ITEMS FROM DBO.fn_split(@blastIDs, ',')  where items <> '0') ids ON ids.items = eal.blastID           
			WHERE  ActionTypeCode = 'click' 
			GROUP BY  ActionValue, EmailID , BlastID        
		) AS inn      Group by BlastID
	) outer1 
	GROUP BY ActionTypeCode
END
