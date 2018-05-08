CREATE PROCEDURE [dbo].[o_ImportErrorSummary_Select_SourceFileID_ProcessCode]
@SourceFileID int,
@ProcessCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	create table #tmpSDT (Pubcode varchar(100), MafField varchar(4000), [value] varchar(max), ImportRowNumber int)
      
	CREATE INDEX IDX_Users_ImportRowNumber_MafField_value  ON #tmpSDT(ImportRowNumber)
      
	insert into #tmpSDT
	SELECT st.pubcode, sdt.MAFField,sdt.Value, st.ImportRowNumber
	from SubscriberTransformed st With(NoLock) 
		JOIN SubscriberDemographicTransformed sdt With(NoLock) ON st.STRecordIdentifier = sdt.STRecordIdentifier 
	where st.SourceFileID = @SourceFileID AND st.ProcessCode = @ProcessCode
      
	SELECT t.PubCode,ie.MAFField,t.Value,ie.ClientMessage,Count(ie.MAFField) as 'ErrorCount'
	FROM ImportError ie With(NoLock)
			join #tmpSDT t on ie.RowNumber = t.ImportRowNumber and ie.MAFField = t.MafField and ie.BadDataRow = t.value
	WHERE ie.SourceFileID = @SourceFileID AND ie.ProcessCode = @ProcessCode
	GROUP BY t.PubCode,ie.MAFField,t.Value,ie.ClientMessage
	ORDER BY ErrorCount DESC

	drop table #tmpSDT
End
go