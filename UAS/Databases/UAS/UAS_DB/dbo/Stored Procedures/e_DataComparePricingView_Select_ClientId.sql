CREATE PROCEDURE [dbo].[e_DataComparePricingView_Select_ClientId] 
@clientId int
as
 begin    
  set nocount on    
  select dcv.DcViewId as 'DcViewID',  
  dcr.ClientId as 'ClientId',  
  dcr.SourceFileId as 'SourceFileId',  
  dcv.DcTargetCodeId as 'DcTargetCodeId',  
  dcv.DcTargetIdUad as 'DcTargetIdUad',  
  dcv.DcTypeCodeId as 'DcTypeCodeId',  
  dcv.CreatedByUserID as 'CreatedByUserID',  
  dcv.PaymentStatusId as 'PaymentStatusId',  
  dcv.DateCreated as 'DateCreated',  
  dcv.Cost as 'Cost',  
  dcv.Notes as 'Notes',  
  dcv.IsBillable as 'IsBillable',  
  dcv.UadNetCount as 'UadNetCount',
  dcr.UadConsensusCount as 'UadConsensusCount',  
  dcr.MatchedRecordCount as 'MatchedRecordCount',  
  dcr.FileRecordCount as 'FileRecordCount',  
  SUM(dcd.ProfileCount) as 'TotalRecordCount',
  SUM(dcd.TotalBilledCost) as 'TotalDownLoadCost'  
  from UAS..DataCompareView dcv with(nolock)   
  join UAS..DataCompareRun dcr with(nolock) on dcr.DcRunId =dcv.DcRunId  
  left join UAS..DataCompareDownload dcd with (nolock) on dcv.DcViewId =dcd.DcViewId  
  group by dcv.DcViewId,  
  dcr.ClientId,  
  dcr.SourceFileId,  
  dcv.DcTargetCodeId,  
  dcv.DcTargetIdUad,  
  dcv.DcTypeCodeId,  
  dcv.CreatedByUserID,  
  dcv.PaymentStatusId,  
  dcv.DateCreated,  
  dcv.Cost,  
  dcv.Notes,  
  dcv.IsBillable,  
  dcv.UadNetCount,
  dcr.UadConsensusCount,  
  dcr.MatchedRecordCount,  
  dcr.FileRecordCount  
  having ClientId = @clientId  
  order by dcv.DcViewId desc, dcv.DateCreated asc  
     
end
  