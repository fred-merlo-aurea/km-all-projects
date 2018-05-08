-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[v_LinkTrackingParamOption_Select_CustomerID_LTPID]
	@LTPID int,
	@CustomerID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT CONVERT(varchar(200),ltpo.LTPOID) as 'LTPOID', ltpo.BaseChannelID, ltpo.ColumnName, ltpo.CreatedDate, ltpo.CreatedUserID, ltpo.CustomerID, ltpo.DisplayName,
    ltpo.IsActive,ltpo.IsDefault, ltpo.IsDeleted, ltpo.IsDynamic, ltpo.LTPID, ltpo.UpdatedDate, ltpo.UpdatedUserID, ltpo.Value
    FROM LinkTrackingParamOption ltpo with(nolock)
    WHERE LTPID = @LTPID and CustomerID = @CustomerID and IsActive = 1 and IsDeleted = 0
END