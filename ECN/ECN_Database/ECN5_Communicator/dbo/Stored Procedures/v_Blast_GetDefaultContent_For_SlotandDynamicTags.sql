CREATE proc [dbo].[v_Blast_GetDefaultContent_For_SlotandDynamicTags]
(
	@BlastID int
)
as
Begin

set nocount on

declare		@layoutID int = 0 ,   
  			@DT_startTag varchar(25) = 'ECN.DynamicTag.',		
  			@DT_endTag varchar(25) = '.ECN.DynamicTag',
  			@DynamicTagSelect varchar(MAX) = '',
  			@CustomerID int,
  			@selectslotstr varchar(8000) = ''

	declare @allContent table (contentID int)
	declare @dynamicTags table (DynamicTagID int, Tag varchar(50), contentID int)

	select @layoutID = layoutID, @CustomerID = CustomerID
		from 
			blast b with (nolock)
		where 
			b.blastID = @BlastID and b.StatusCode <> 'Deleted'

		if exists (select top 1 filterID from contentFilter WITH (NOLOCK) where layoutID = @layoutID and IsDeleted = 0)
		Begin
			select @selectslotstr = ISNULL(convert(varchar,contentslot1), 0) + ' as slot1, ' + ISNULL(convert(varchar,contentslot2), 0) + ' as slot2,' + ISNULL(convert(varchar,contentslot3), 0) + ' as slot3, 
			' + ISNULL(convert(varchar,contentslot4), 0) + ' as slot4, ' + ISNULL(convert(varchar,contentslot5), 0) + ' as slot5, ' + ISNULL(convert(varchar,contentslot6), 0) + ' as slot6, 
			' + ISNULL(convert(varchar,contentslot7), 0) + ' as slot7, ' + ISNULL(convert(varchar,contentslot8), 0) + ' as slot8, ' + ISNULL(convert(varchar,contentslot9), 0) + ' as slot9 '   
			from layout  where layoutID = @layoutID	
		End

		insert into @allContent
		select contentID
		from
		(
		select LayoutID, ContentSlot1, ContentSlot2, ContentSlot3, ContentSlot4, ContentSlot5, ContentSlot6, ContentSlot7, ContentSlot8, ContentSlot9 from Layout 
		where LayoutID  = @layoutID  and IsDeleted = 0
		) x
		unpivot (contentID for slot in (ContentSlot1, ContentSlot2, ContentSlot3, ContentSlot4, ContentSlot5, ContentSlot6, ContentSlot7, ContentSlot8, ContentSlot9)) as unpvt
		where contentID > 0
		union
		select ContentID from ContentFilter where LayoutID = @layoutID and IsDeleted = 0
		
		insert into @dynamicTags
		select distinct DynamicTagID, dt.Tag, dt.ContentID from Content c join @allContent ac on c.ContentID = ac.contentID cross join DynamicTag dt 
		Where c.CustomerID = @CustomerID and dt.CustomerID = @CustomerID and c.IsDeleted = 0 and isnull(dt.IsDeleted,0) = 0 and 
		(PATINDEX('%' + @DT_startTag +dt.Tag+ @DT_endTag + '%', c.ContentSource) > 0 or   PATINDEX('%' + @DT_startTag +dt.Tag+ @DT_endTag + '%', c.ContentText) > 0 )		
		
		if exists (select top 1 contentID from @dynamicTags)
		Begin					

			SELECT 
			   @DynamicTagSelect = (Case when LEN(@selectslotstr) > 0 then ',' else '' end) +  STUFF( (SELECT ',' + convert(varchar(10), x.ContentID) + ' as [' + @DT_startTag + Tag + @DT_endTag +']'  from 
				@dynamicTags x
			 FOR XML PATH('')), 
			1, 1, '')			
		End

		--print  @selectslotstr
		--print  @DynamicTagSelect
		
		if Len(@selectslotstr) > 0 or LEN(@DynamicTagSelect) > 0
			exec('select  ' + @selectslotstr + @DynamicTagSelect)		

End