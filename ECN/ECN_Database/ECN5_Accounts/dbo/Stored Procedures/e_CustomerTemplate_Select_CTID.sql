CREATE PROCEDURE [dbo].[e_CustomerTemplate_Select_CTID]   
@CTID int = null
AS

	SELECT * FROM CustomerTemplate WHERE CTID = @CTID and IsDeleted = 0
