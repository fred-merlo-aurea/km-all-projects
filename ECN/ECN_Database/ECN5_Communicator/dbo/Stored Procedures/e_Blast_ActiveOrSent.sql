CREATE PROCEDURE [dbo].[e_Blast_ActiveOrSent] 
	@CustomerID INT,
	@BlastID INT
AS     
BEGIN 
	IF EXISTS (
		SELECT 
			TOP 1 BlastID
		FROM 
			Blast WITH (NOLOCK)
		WHERE 
			CustomerID = @CustomerID AND 
			BlastID = @BlastID AND
			BlastType not in ('Layout','NoOpen') AND
			(
				StatusCode = 'active' OR 
				StatusCode = 'sent' OR 
				(
					StatusCode = 'pending' AND
					(
						SendTime <= GETDATE() OR
						(
							SendTime > GETDATE() AND
							DATEDIFF(ss,GETDATE(), SendTime) < 5
						)
					)
				)
			)
	) SELECT 1 ELSE SELECT 0


	--DECLARE @SQLSelect VARCHAR(4000)
	--DECLARE @Where VARCHAR(500)
	--IF @GroupID != NULL
	--BEGIN
	--	SET @Where = ' AND GroupID = ' + CONVERT(VARCHAR,@GroupID)
	--END	
	--IF @BlastType != NULL
	--BEGIN
	--	SET @Where = ' AND BlastType = ''' + @BlastType + ''''
	--END	
	
	--SET @SQLSelect = 
	--		'IF EXISTS 
	--		(
	--			SELECT 
	--				top 1 BlastID 
	--			FROM 
	--				Blast WITH (NOLOCK)
	--			WHERE 
	--				CustomerID = ' + CONVERT(VARCHAR,@CustomerID) + ' AND
	--				CampaignItemID = ' + CONVERT(VARCHAR,@CampaignItemID) + 
	--				@Where + ' AND 
	--				(
	--					StatusCode = ''active'' OR 
	--					StatusCode = ''sent'' OR 
	--					(
	--						StatusCode = ''pending'' AND
	--						SendTime > GETDATE() AND
	--						DATEDIFF(mi,Sendtime, GETDATE()) < 5
	--					)
	--				) AND
	--				IsDeleted = 0
	--		) SELECT 1 ELSE SELECT 0'
	--EXEC (@SQLSelect)
END
