CREATE proc [dbo].[e_EmailGroup_ExportFromImportEmails]
(  
 @filename varchar(200),
 @actionCode varchar(150)
)  
as  
Begin 
select * from ImportEmailsLog where FileName=@filename and ActionCode=@actionCode
end