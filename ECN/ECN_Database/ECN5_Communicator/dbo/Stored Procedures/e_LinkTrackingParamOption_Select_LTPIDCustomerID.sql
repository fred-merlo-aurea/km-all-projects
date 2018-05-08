-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_LinkTrackingParamOption_Select_LTPIDCustomerID]
	@LTPID int,
	@Value varchar(50),
	@CustomerID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT *
	FROM LinkTrackingParamOption WITH (NOLOCK)
	WHERE LTPID = @LTPID AND CustomerID=@CustomerID  AND Value= @Value AND IsActive = 1 and IsDeleted = 0
	
END
