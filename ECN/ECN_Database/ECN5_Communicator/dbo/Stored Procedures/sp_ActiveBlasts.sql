CREATE  PROCEDURE [dbo].[sp_ActiveBlasts]
AS
BEGIN
--select getdate() as 'Time Now'
Select * from Blastsingles where processed = 'n' and sendtime < getDate()
Select * from [BLAST] Where StatusCode in ('active') order by SendTime
Select * from [BLAST] Where SendTime < getDate() AND StatusCode in ('pending') order by SendTime
END
