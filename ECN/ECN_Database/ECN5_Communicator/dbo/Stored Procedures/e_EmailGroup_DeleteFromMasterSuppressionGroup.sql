-- 2014-10-24 MK Added  WITH (NOLOCK) hints

CREATE  PROC [dbo].[e_EmailGroup_DeleteFromMasterSuppressionGroup] 
(
	@UserID int = NULL,
    @GroupID int = NULL,
    @EmailID int =NULL
)
AS 

SET NOCOUNT ON

BEGIN
	 IF EXISTS (SELECT GroupID FROM [Groups] WITH (NOLOCK) WHERE  GroupID = @GroupID AND ISNULL(MasterSupression,0) = 1)
	 BEGIN
		UPDATE 
			[EmailGroups]
		SET 
			SubscribeTypeCode = 'S', 
			LastChanged = getdate()
		WHERE 
			EmailID = @EmailID 
			AND SubscribeTypeCode = 'M' 
	 END
END
