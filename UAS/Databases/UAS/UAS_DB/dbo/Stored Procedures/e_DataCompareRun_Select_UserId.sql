CREATE PROCEDURE [dbo].[e_DataCompareRun_Select_UserId]
@userId int
as
	begin
		set nocount on
		select dc.* 
		from DataCompareRun dc with(nolock) 
		join SourceFile sf with(nolock) on dc.SourceFileId = sf.SourceFileID
		where sf.CreatedByUserID = @userId
	end
go
