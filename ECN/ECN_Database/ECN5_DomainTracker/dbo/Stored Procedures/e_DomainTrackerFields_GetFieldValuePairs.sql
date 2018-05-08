CREATE PROCEDURE [dbo].[e_DomainTrackerFields_GetFieldValuePairs]
@DomainTrackerID int,		 --4
@DomainTrackerActivityID int --3028117
AS
SELECT TOP 1000 Value,FieldName,*
  FROM  [ECN5_DomainTracker].[dbo].[DomainTrackerFields] f (nolock)
  JOIN  [ECN5_DomainTracker].[dbo].[DomainTrackerValue] v (nolock)  on v.DomainTrackerFieldsID = f.DomainTrackerFieldsID  
  where DomainTrackerID = @DomainTrackerID
  AND   DomainTrackerActivityID = @DomainTrackerActivityID