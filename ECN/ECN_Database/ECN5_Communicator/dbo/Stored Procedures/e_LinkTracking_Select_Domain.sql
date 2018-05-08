CREATE  PROC dbo.e_LinkTracking_Select_Domain
(
@LTID int,
@CustomerID int,
@Domain varchar(200)
)
AS 
BEGIN
	IF EXISTS (
		SELECT 
			TOP 1 LinkTrackingDomain.LTID
		FROM 
			LinkTrackingDomain WITH (NOLOCK)
		WHERE 
			CustomerID = @CustomerID AND 
			LTID = @LTID AND
			IsDeleted = 0
	)
	BEGIN 
		IF EXISTS (
		SELECT 
			TOP 1 LinkTrackingDomain.LTID
		FROM 
			LinkTrackingDomain WITH (NOLOCK)
		WHERE 
			CustomerID = @CustomerID AND 
			LTID = @LTID AND 
			Domain=@Domain AND
			IsDeleted = 0
		)
		BEGIN 
			SELECT 1
		END
		ELSE
		BEGIN
			SELECT 0
		END
	END
	ELSE
	BEGIN
		SELECT 1
	END
END