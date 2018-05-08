CREATE PROCEDURE [dbo].[e_LinkTrackingParam_Select_LTPID]
@LTPID int
AS
	SELECT *
	FROM LinkTrackingParam WITH (NOLOCK)
	WHERE LTPID = @LTPID AND IsActive = 1