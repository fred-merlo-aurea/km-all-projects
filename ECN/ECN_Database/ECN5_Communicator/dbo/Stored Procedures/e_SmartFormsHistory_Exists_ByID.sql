CREATE PROCEDURE [dbo].[e_SmartFormsHistory_Exists_ByID] 
	@SmartFormID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS	(
				SELECT TOP 1 sfh.SmartFormID
				FROM SmartFormsHistory sfh WITH (NOLOCK) join Groups g WITH (NOLOCK) on sfh.GroupID = g.GroupID 
				WHERE g.CustomerID = @CustomerID AND sfh.SmartFormID = @SmartFormID AND sfh.IsDeleted = 0
				) SELECT 1 ELSE SELECT 0
END