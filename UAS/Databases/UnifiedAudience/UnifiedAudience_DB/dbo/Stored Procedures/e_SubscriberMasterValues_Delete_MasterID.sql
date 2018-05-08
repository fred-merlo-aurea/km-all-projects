CREATE PROCEDURE [dbo].[e_SubscriberMasterValues_Delete_MasterID]
	@MasterID int
AS	
BEGIN

	SET NOCOUNT ON

	DELETE 
	FROM SubscriberMasterValues 
	WHERE MasterGroupID in 
	(
		SELECT mcs.MasterGroupID 
		FROM SubscriberMasterValues smv with(nolock)
		JOIN Mastercodesheet mcs with(nolock) ON smv.MasterGroupID = mcs.MasterGroupID
		WHERE mcs.MasterID = @MasterID
	)

END