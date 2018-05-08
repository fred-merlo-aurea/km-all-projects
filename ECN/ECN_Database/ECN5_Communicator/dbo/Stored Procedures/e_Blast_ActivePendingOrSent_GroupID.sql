CREATE PROCEDURE [dbo].[e_Blast_ActivePendingOrSent_GroupID] 
	@CustomerID INT,
	@GroupID INT
AS     
BEGIN 
	IF EXISTS (
		SELECT 
			TOP 1 BlastID
		FROM 
			Blast WITH (NOLOCK)
		WHERE 
			CustomerID = @CustomerID AND 
			GroupID = @GroupID AND
			(
				StatusCode = 'active' OR 
				StatusCode = 'sent' OR 
				StatusCode = 'pending'
			)
	) 
	BEGIN
		SELECT 1 
	END
	ELSE IF EXISTS (
					SELECT TOP 1 b.BlastID from CampaignItem ci
					join CampaignItemBlast cib
					on cib.CampaignItemID=ci.CampaignItemID
					join Blast b
					on cib.BlastID=b.BlastID
					join CampaignItemSuppression cis 
					on ci.CampaignItemID=cis.CampaignItemID
					WHERE 
						b.CustomerID = @CustomerID AND 
						cis.GroupID = @GroupID AND
						(
							b.StatusCode = 'active' OR 
							b.StatusCode = 'sent' OR 
							b.StatusCode = 'pending'
						)
						AND
						ci.IsDeleted=0 AND
						cis.IsDeleted=0 and
						cib.IsDeleted=0
					 				
					)
	BEGIN
		SELECT 1
	END
	ELSE 
	BEGIN
		SELECT 0
	END

END

