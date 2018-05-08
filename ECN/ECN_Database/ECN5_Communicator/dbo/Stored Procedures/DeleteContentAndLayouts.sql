CREATE proc [dbo].[DeleteContentAndLayouts] 
@contentIDList as varchar(500), @layoutIDList as varchar(500)
as 
PRINT 'Start deleting Contens with ID ' + @contentIDList + ' Layout with ID' + @layoutIDList
exec('DELETE from LinkAlias where ContentID in ' + @contentIDList)
exec('DELETE from [CONTENTFILTERDETAIL] where FilterID in (select filterID from [CONTENTFILTER] where ContentID in ' + @contentIDList + ')')
exec('DELETE from [CONTENTFILTER] where LayoutID in '+ @layoutIDList)
exec('DELETE from content where ContentID in '+ @contentIDList)
exec('DELETE from [LAYOUT] where LayoutID in '+ @layoutIDList)
