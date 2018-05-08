CREATE proc [dbo].[ShowContentAndLayouts] 
@contentIDList as varchar(500), @layoutIDList as varchar(500)
as 
exec('select count(*) AS LinkAliasCount from LinkAlias where ContentID in ' + @contentIDList)
exec('select count(*) AS FilterDetailCount from [CONTENTFILTERDETAIL] where FilterID in (select filterID from [CONTENTFILTER] where ContentID in ' + @contentIDList + ')')
exec('select count(*) AS FilterCount from [CONTENTFILTER] where LayoutID in ' + @layoutIDList)
exec('select contentID from content where ContentID in '+ @contentIDList)
exec('select LayoutID from [LAYOUT] where LayoutID in '+ @layoutIDList)
