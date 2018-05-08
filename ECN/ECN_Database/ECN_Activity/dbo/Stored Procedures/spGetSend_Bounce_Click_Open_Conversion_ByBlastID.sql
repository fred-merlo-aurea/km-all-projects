CREATE PROCEDURE [dbo].[spGetSend_Bounce_Click_Open_Conversion_ByBlastID] 
	@BlastIDs varchar(8000), @showParent bit
AS
BEGIN
	IF @showParent = 0
	BEGIN
		SELECT COUNT (DISTINCT EmailID) AS DistinctCount, COUNT(EmailID) AS Total, 'send' AS ActionTypeCode FROM BlastActivitySends 
		WHERE BlastID in (select Items from dbo.fn_Split(@BlastIDs,','))
		UNION
		SELECT COUNT (DISTINCT EmailID) AS DistinctCount, COUNT(EmailID) AS Total, 'bounce' AS ActionTypeCode FROM BlastActivityBounces
		WHERE BlastID in (select Items from dbo.fn_Split(@BlastIDs,','))
		UNION
		SELECT COUNT (DISTINCT EmailID) AS DistinctCount, COUNT(EmailID) AS Total, 'click' AS ActionTypeCode FROM BlastActivityClicks
		WHERE BlastID in (select Items from dbo.fn_Split(@BlastIDs,','))
		UNION
		SELECT COUNT (DISTINCT EmailID) AS DistinctCount, COUNT(EmailID) AS Total, 'open' AS ActionTypeCode FROM BlastActivityOpens
		WHERE BlastID in (select Items from dbo.fn_Split(@BlastIDs,','))
		UNION
		SELECT COUNT (DISTINCT EmailID) AS DistinctCount, COUNT(EmailID) AS Total, 'conversion' AS ActionTypeCode FROM BlastActivityConversion
		WHERE BlastID in (select Items from dbo.fn_Split(@BlastIDs,',')) 
	END
	ELSE
	BEGIN
		SELECT 'N' AS Parent, COUNT (DISTINCT EmailID) AS DistinctCount, COUNT(EmailID) AS Total, 'send' AS ActionTypeCode FROM BlastActivitySends 
		WHERE BlastID in (select Items from dbo.fn_Split(@BlastIDs,','))
		UNION
		SELECT 'N' AS Parent, COUNT (DISTINCT EmailID) AS DistinctCount, COUNT(EmailID) AS Total, 'bounce' AS ActionTypeCode FROM BlastActivityBounces
		WHERE BlastID in (select Items from dbo.fn_Split(@BlastIDs,','))
		UNION
		SELECT 'N' AS Parent, COUNT (DISTINCT EmailID) AS DistinctCount, COUNT(EmailID) AS Total, 'click' AS ActionTypeCode FROM BlastActivityClicks
		WHERE BlastID in (select Items from dbo.fn_Split(@BlastIDs,','))
		UNION
		SELECT 'N' AS Parent, COUNT (DISTINCT EmailID) AS DistinctCount, COUNT(EmailID) AS Total, 'open' AS ActionTypeCode FROM BlastActivityOpens
		WHERE BlastID in (select Items from dbo.fn_Split(@BlastIDs,','))
		UNION
		SELECT 'N' AS Parent, COUNT (DISTINCT EmailID) AS DistinctCount, COUNT(EmailID) AS Total, 'conversion' AS ActionTypeCode FROM BlastActivityConversion
		WHERE BlastID in (select Items from dbo.fn_Split(@BlastIDs,','))  
	END	
END
