CREATE PROCEDURE [dbo].[e_LandingPageAssign_Select_GetOneToUse]
(
@CustomerID int,
@LPID int
)
AS
BEGIN
	DECLARE @BaseChannelID int
	SELECT @BaseChannelID = BaseChannelID FROM Customer WHERE CustomerID = @CustomerID AND IsDeleted = 0
	IF EXISTS(
		SELECT TOP 1 LPAID 
		FROM LandingPageAssign with(nolock)
		WHERE BaseChannelID = @BaseChannelID AND
		ISNULL(BaseChannelDoesOverride, 0) = 1 AND
		LPID = @LPID
		)
	BEGIN
		IF EXISTS	
					(
					SELECT TOP 1 LPAID 
					FROM 
						LandingPageAssign WITH (NOLOCK) 
					WHERE
						CustomerID = @CustomerID AND
						IsNull(CustomerDoesOverride, 0) = 1 AND
						LPID = @LPID
					)
		BEGIN
			SELECT *
			FROM 
				LandingPageAssign WITH (NOLOCK) 
			WHERE
				CustomerID = @CustomerID AND
				IsNull(CustomerDoesOverride, 0) = 1 AND
				LPID = @LPID
		END
		ELSE
		BEGIN
			SELECT *
			FROM 
				LandingPageAssign WITH (NOLOCK) 
			WHERE
				BaseChannelID = @BaseChannelID AND
				IsNull(BaseChannelDoesOverride, 0) = 1 AND
				LPID = @LPID
		END
	END
	ELSE
	BEGIN
		SELECT *
		FROM 
			LandingPageAssign WITH (NOLOCK) 
		WHERE
			LPID = @LPID AND 
			IsNull(IsDefault, 0) = 1
	END
END