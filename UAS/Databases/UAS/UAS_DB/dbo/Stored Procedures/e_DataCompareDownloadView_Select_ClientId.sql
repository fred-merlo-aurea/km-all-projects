CREATE PROCEDURE [dbo].[e_DataCompareDownloadView_Select_ClientId]
@clientId int  
as  
begin  
  set nocount on  
  select  dcd.DcDownloadId as 'DcDownloadId',  
  dcr.ClientId as 'ClientId',  
  dcr.SourceFileId as 'SourceFileId',  
  dcv.DcTargetCodeId as 'DcTargetCodeId',  
  dcv.DcTargetIdUad as 'DcTargetIdUad',  
  dcd.DcTypeCodeId as 'DcTypeCodeId',  
  dcd.PurchasedByUserId as 'CreatedByUserID',  
  dcd.PurchasedDate as 'DateCreated',  
  dcd.WhereClause as 'WhereClause',  
  dcd.ProfileCount as 'TotalRecordCount',  
  dcd.DownloadFileName as 'DownloadFileName',
  dcv.Cost as 'FileComparisonCost',
  SUM(dcd.TotalBilledCost) as 'TotalDownLoadCost'  
  from UAS..DataCompareDownload dcd with(nolock)   
  inner join UAS..DataCompareView dcv with(nolock) on dcd.DcViewId =dcv.DcViewId  
  inner join UAS..DataCompareRun dcr with (nolock) on dcr.DcRunId =dcv.DcRunId 
  Group By dcd.DcDownloadId ,  
  dcr.ClientId ,  
  dcr.SourceFileId,  
  dcv.DcTargetCodeId,  
  dcv.DcTargetIdUad,
  dcd.DcTypeCodeId,
  dcd.PurchasedByUserId,
  dcd.PurchasedDate ,
  dcd.WhereClause,
  dcd.ProfileCount,
  dcd.DownloadFileName,
  dcv.Cost
  having ClientId =@clientId  
  order by dcd.DcDownloadId desc  
end  