create PROCEDURE [dbo].[e_CustomerLicense_Select_CLID]
@CLID int
AS

SELECT * FROM CustomerLicense WHERE CLID = @CLID and IsDeleted=0
