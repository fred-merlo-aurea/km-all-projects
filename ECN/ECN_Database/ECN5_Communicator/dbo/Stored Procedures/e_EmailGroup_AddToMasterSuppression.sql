CREATE  PROC [dbo].[e_EmailGroup_AddToMasterSuppression] 
(
	@EmailID INT,
    @CustomerID INT
)
AS 
BEGIN
		
	DECLARE @MasterSuppressionGroupID INT
	SELECT
		@MasterSuppressionGroupID = GroupID 
	FROM 
		Groups WITH (NOLOCK)
	WHERE
		CustomerID = @CustomerID AND
		MasterSupression = 1
		
	IF NOT EXISTS (SELECT top 1 * FROM EmailGroups WITH (NOLOCK) WHERE EmailID = @EmailID AND GroupID = @MasterSuppressionGroupID)
	BEGIN
		INSERT INTO EmailGroups
			(EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn)
		VALUES 
			(@EmailID, @MasterSuppressionGroupID, 'html', 'S', GETDATE())
			
	END
	ELSE
	BEGIN
		UPDATE 
			EmailGroups
		SET
			FormatTypeCode = 'html', SubscribeTypeCode = 'S', LastChanged = GetDate()
		WHERE
			EmailID = @EmailID AND GroupID = @MasterSuppressionGroupID
			
	END
		
	UPDATE 
		EmailGroups 
	SET 
		SubscribeTypeCode = 'M', 
		LastChanged = GETDATE() 
	WHERE 
		GroupID in (SELECT 
						GroupID 
					FROM 
						Groups WITH (NOLOCK) 
					WHERE 
						CustomerID = @CustomerID AND GroupID <> @MasterSuppressionGroupID) AND
		EmailID = @EmailID AND
		SubscribeTypeCode in ('S', 'P')

	select @@ROWCOUNT

END