CREATE PROCEDURE [dbo].[e_Client_Select_FtpFolder] 
@FtpFolder varchar(50)
AS
	SELECT * 
	FROM Client With(NoLock)
	WHERE FtpFolder = @FtpFolder
go
