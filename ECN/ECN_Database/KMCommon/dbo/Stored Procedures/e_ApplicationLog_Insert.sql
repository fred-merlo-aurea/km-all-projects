CREATE PROCEDURE dbo.e_ApplicationLog_Insert
@ApplicationID int = -1,
@SeverityID int = 1,
@SourceMethod nvarchar(250),
@Exception text,
@LogNote text,
@IsBug bit,
@IsUserSubmitted bit,
@GDCharityID int,
@ECNCustomerID int,
@SubmittedBy nvarchar(250),
@SubmittedByEmail nvarchar(100)
AS
INSERT INTO ApplicationLog (ApplicationID,SeverityID,SourceMethod,Exception,LogNote,IsBug,IsUserSubmitted,GDCharityID,ECNCustomerID,SubmittedBy,SubmittedByEmail,IsFixed,FixedDate,FixedTime,
							 FixedBy,FixedNote,LogAddedDate,LogAddedTime,LogUpdatedDate,LogUpdatedTime)
VALUES(@ApplicationID,@SeverityID,@SourceMethod,@Exception,@LogNote,@IsBug,@IsUserSubmitted,@GDCharityID,@ECNCustomerID,@SubmittedBy,@SubmittedByEmail,
	   0,'1/1/1900','00:00:00','','',GETDATE(),GETDATE(),GETDATE(),GETDATE());SELECT @@IDENTITY;
