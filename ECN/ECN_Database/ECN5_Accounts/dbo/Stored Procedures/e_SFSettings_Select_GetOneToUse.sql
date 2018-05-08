CREATE PROCEDURE dbo.e_SFSettings_Select_GetOneToUse
(
@CustomerID int
)
AS
BEGIN
	DECLARE @BaseChannelID int
	SELECT @BaseChannelID = BaseChannelID FROM Customer WHERE CustomerID = @CustomerID AND IsDeleted = 0
	IF EXISTS	
				(
				SELECT TOP 1 SFSettingsID 
				FROM 
					SFSettings WITH (NOLOCK) 
				WHERE
					CustomerID = @CustomerID AND
					IsNull(CustomerDoesOverride, 0) = 1 
				)
	BEGIN
		SELECT *
		FROM 
			SFSettings WITH (NOLOCK) 
		WHERE
			CustomerID = @CustomerID AND
			IsNull(CustomerDoesOverride, 0) = 1 
	END
	ELSE IF EXISTS
				(
				SELECT TOP 1 SFSettingsID 
				FROM 
					SFSettings WITH (NOLOCK) 
				WHERE
					BaseChannelID = @BaseChannelID 
				)
	BEGIN
		SELECT *
		FROM 
			SFSettings WITH (NOLOCK) 
		WHERE
			BaseChannelID = @BaseChannelID 
	END
END