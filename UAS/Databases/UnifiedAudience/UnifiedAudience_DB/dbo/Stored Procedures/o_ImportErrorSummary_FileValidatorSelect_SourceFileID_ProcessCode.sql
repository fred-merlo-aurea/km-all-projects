CREATE PROCEDURE [dbo].[o_ImportErrorSummary_FileValidatorSelect_SourceFileID_ProcessCode]
@SourceFileID int,
@ProcessCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	SELECT st.PubCode,ie.MAFField,sdt.Value,ie.ClientMessage,Count(ie.MAFField) as 'ErrorCount'
	FROM FileValidator_ImportError ie With(NoLock)
		JOIN FileValidator_Transformed st With(NoLock) ON ie.SourceFileID = st.SourceFileID AND ie.ProcessCode = st.ProcessCode AND ie.RowNumber = st.ImportRowNumber 
		JOIN FileValidator_DemographicTransformed sdt With(NoLock) ON st.STRecordIdentifier = sdt.STRecordIdentifier AND ie.MAFField = sdt.MAFField and ie.BadDataRow = sdt.Value
	WHERE ie.SourceFileID = @SourceFileID AND ie.ProcessCode = @ProcessCode
	GROUP BY st.PubCode,ie.MAFField,sdt.Value,ie.ClientMessage
	ORDER BY ErrorCount DESC

END
GO