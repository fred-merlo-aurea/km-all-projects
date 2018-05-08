CREATE PROCEDURE [dbo].[e_ImportError_Select_ProcessCode_SourceFileID]
@ProcessCode varchar(50),
@SourceFileID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM ImportError With(NoLock)
	WHERE ProcessCode = @ProcessCode
	AND SourceFileID = @SourceFileID
	ORDER BY RowNumber

END
GO