CREATE PROCEDURE [dbo].[spGetBlastGroupReportData] 
	-- Add the parameters for the stored procedure here
	@blastGroupID int
	--set @blastGroupID = 13697
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @blastIDs varchar(4000)
    -- Insert statements for procedure here
	SELECT @blastIDs = BlastIDs from ecn5_communicator..BlastGrouping WHERE BlastGroupID = @blastGroupID

	SELECT sum(DistinctCount) AS 'DistinctCount', sum(total) AS 'total' , ActionTypeCode from 
	(
		SELECT  
				ISNULL(COUNT(DISTINCT bac.EmailID),0) AS DistinctCount,
				ISNULL(COUNT(bac.EmailID),0) AS total, 
				'UNIQCLIQ' as ActionTypeCode,
				BlastID
		FROM  
				BlastActivityClicks bac WITH (NOLOCK) JOIN (SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',') where items <> '0') ids ON ids.items = bac.BlastID
		GROUP BY BlastID 
		UNION      
		SELECT  
				ISNULL(SUM(DistinctCount),0) AS DistinctCount, 
				ISNULL(SUM(total),0) AS total,
				'click'  as ActionTypeCode,
				BlastID       
		FROM (        
				SELECT  COUNT(distinct URL) AS DistinctCount, COUNT(bac.EmailID) AS total, BlastID        
				FROM   BlastActivityClicks bac WITH (NOLOCK) JOIN (SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')  where items <> '0') ids ON ids.items = bac.blastID
				GROUP BY  URL, bac.EmailID, BlastID        
			) AS inn Group by BlastID
		UNION  
		SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'send' AS ActionTypeCode, BlastID FROM BlastActivitySends bas WITH (NOLOCK) JOIN (SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')  where items <> '0') ids ON ids.items = bas.blastID GROUP BY BlastID
		UNION 
		SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'resend' AS ActionTypeCode, BlastID FROM BlastActivityResends bar WITH (NOLOCK) JOIN (SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')  where items <> '0') ids ON ids.items = bar.blastID GROUP BY BlastID
		UNION 
		SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'open' AS ActionTypeCode, BlastID FROM BlastActivityOpens bao WITH (NOLOCK) JOIN (SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')  where items <> '0') ids ON ids.items = bao.blastID GROUP BY BlastID
		UNION 
		SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'bounce' AS ActionTypeCode, BlastID FROM BlastActivityBounces bab WITH (NOLOCK) JOIN (SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')  where items <> '0') ids ON ids.items = bab.blastID GROUP BY BlastID      
		UNION
		SELECT  
				ISNULL(COUNT(DISTINCT bau.EmailID),0) AS DistinctCount,
				ISNULL(COUNT(bau.EmailID),0) AS total, 
				uc.UnsubscribeCode  as ActionTypeCode,
				BlastID
		FROM  
				BlastActivityUnSubscribes bau WITH (NOLOCK) JOIN UnsubscribeCodes uc WITH (NOLOCK) ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID JOIN (SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')  where items <> '0') ids ON ids.items = bau.blastID 
		GROUP BY  uc.UnsubscribeCode, bau.BlastID
		UNION
		SELECT  
				ISNULL(COUNT(DISTINCT basupp.EmailID),0) AS DistinctCount,
				ISNULL(COUNT(basupp.EmailID),0) AS total, 
				sc.SupressedCode  as ActionTypeCode,
				BlastID
		FROM  
				BlastActivitySuppressed basupp WITH (NOLOCK) JOIN SuppressedCodes sc WITH (NOLOCK) ON basupp.SuppressedCodeID = sc.SuppressedCodeID JOIN (SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')  where items <> '0') ids ON ids.items = basupp.blastID 
		GROUP BY  sc.SupressedCode, BlastID
		UNION 
		SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'conversion' AS ActionTypeCode, BlastID FROM BlastActivityConversion bac WITH (NOLOCK) JOIN (SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')  where items <> '0') ids ON ids.items = bac.blastID GROUP BY BlastID                   
		UNION 
		SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'refer' AS ActionTypeCode, BlastID FROM BlastActivityRefer bar WITH (NOLOCK) JOIN (SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')  where items <> '0') ids ON ids.items = bar.blastID GROUP BY BlastID
	) outer1 
	GROUP BY ActionTypeCode
END
