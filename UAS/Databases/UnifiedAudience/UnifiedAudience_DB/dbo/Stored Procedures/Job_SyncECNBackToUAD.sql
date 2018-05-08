CREATE PROCEDURE [dbo].[Job_SyncECNBackToUAD]
@GroupID	integer
AS
	SET XACT_ABORT, NOCOUNT ON
BEGIN TRY

--EXECUTE 	Job_SyncECNBackToUAD @GroupID = 190625
--declare @GroupID  int
--Set @GroupID = 374224

------------------------------------------------------
-- UPDATE the email ID back into ExternalKeyID in PubSubscriptions
--        matching on PubSubscriptionID   where emailID is new or changed
------------------------------------------------------------
	UPDATE ps
		Set ps.EmailID =  tnew.EmailID		
--select torig.EmailID, tnew.EmailID, ps.emailid, ps.*
		FROM 
			dbo.tmp_circtDataForECN torig with (NOLOCK) 
			join tmp_CirctDataForECNwithEmailIDs tnew with (NOLOCK) on tnew.PubSubscriptionID = torig.PubSubscriptionID
			join PubSubscriptions ps with (NOLOCK) on ps.PubSubscriptionID = torig.PubSubscriptionID
		WHERE	torig.GroupId = @GroupID
			AND IsNull(ps.EmailID, 0) <> tnew.EmailID
			AND IsNull(tnew.EmailID, 0) <> 0

END TRY

BEGIN CATCH
	IF @@trancount > 0 ROLLBACK TRANSACTION
	EXEC workspace.dbo.UserErrorHandler 
	RETURN 55555
END CATCH
GO
--@GroupID	integer
--AS
--BEGIN
--	SET XACT_ABORT, NOCOUNT ON
--	BEGIN TRY

--	--EXECUTE 	Job_SyncECNBackToUAD @GroupID = 190625
--	--declare @GroupID  int
--	--Set @GroupID = 190625

--	------------------------------------------------------
--	-- UPDATE the email ID back into ExternalKeyID in PubSubscriptions
--	--        matching on PubSubscriptionID   where ExternalKeyID (emailID) is NULL
--	------------------------------------------------------------
--		UPDATE ps
--			Set ps.EmailID = el.EmailID		
--			--select el.EmailID, ps.*
--			FROM dbo.tmp_circtDataForECN el with (NOLOCK) 
--				join PubSubscriptions ps with (NOLOCK) on ps.PubSubscriptionID = el.PubSubscriptionID
--			WHERE el.GroupId = @GroupID
--				AND IsNull(ps.EmailID, 0) = 0
--				AND IsNull(el.EmailID, 0) <> 0

--	END TRY

--	BEGIN CATCH
--		IF @@trancount > 0 ROLLBACK TRANSACTION
--		EXEC UAD_temp..UserErrorHandler 
--		RETURN 55555
--	END CATCH

--END