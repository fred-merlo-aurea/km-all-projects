CREATE PROCEDURE [dbo].[e_DataCompareRun_Select_SourceFileId]
@sourceFileId int
as
	begin
		set nocount on
		select * from DataCompareRun with(nolock) where SourceFileId = @sourceFileId
	end
go