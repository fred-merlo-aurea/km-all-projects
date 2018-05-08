-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_LinkTrackingParamOption_ResetCustDefault]
	
	@LTPID int,
	@CustomerID int
AS
BEGIN

    Update LinkTrackingParamOption
    set IsDefault = 0
    where LTPID = @LTPID and CustomerID = @CustomerID and IsDeleted = 0 and IsActive = 1
END