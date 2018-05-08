CREATE PROCEDURE [dbo].[e_UASBridgeECN_Select_UserID]
	@UserID INT
AS
	SELECT * FROM UASBridgeECN With(NoLock) WHERE UASUserID = @UserID