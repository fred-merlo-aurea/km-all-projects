CREATE PROCEDURE [dbo].[e_Email_GetEmailsForWATT_NXTBook_Sync]
	@GroupID int,
	@Job1 bit,
	@DateFrom datetime = null,
	@Field varchar(100) = '',
	@FieldValue varchar(MAX) = '',
	@DoFullNXTBookSync bit = 0
AS
DECLARE @GDFID int = -1

declare @ValueTable table(ID int IDentity(1,1),Value varchar(500))

insert into @ValueTable
select Items
from fn_Split(@FieldValue, ',')

IF @Job1 = 1--first NXTBook job
BEGIN
	--DateFrom is null so doing a full grab
	IF @DateFrom is null
	BEGIN
		SELECT @GDFID = ISNULL(GroupDataFieldsID,-1) FROM GroupDatafields gdf where gdf.GroupID = @GroupID and gdf.ShortName = @Field and ISNULL(gdf.IsDeleted,0) = 0

		SELECT distinct e.* 
		FROM Emails e with(nolock)
		JOIN EmailGroups eg with(nolock) on e.EmailID = eg.EmailID
		JOIN EmailDataValues edv with(nolock) on e.EmailID = edv.EmailID
		LEFT OUTER JOIN @ValueTable vt on edv.DataValue = vt.Value
		WHERE edv.GroupDataFieldsID = case when @GDFID = -1 then edv.GroupDataFieldsID when @GDFID > -1 then @GDFID end
			  and ISNULL(vt.ID,0) >= case when @GDFID = -1 then 0 when @GDFID > -1 then 1 end
			  and eg.GroupID = @GroupID
			  and eg.SubscribeTypeCode = 'S'
			  and ISNULL(e.User1,'') <> ''
	END
	ELSE--DateFrom has value so use that to do a delta grab
	BEGIN
		
		SELECT @GDFID = ISNULL(GroupDataFieldsID,-1) FROM GroupDatafields gdf where gdf.GroupID = @GroupID and gdf.ShortName = @Field and ISNULL(gdf.IsDeleted,0) = 0

		SELECT distinct e.* 
		FROM Emails e with(nolock)
			JOIN EmailGroups eg with(nolock) on e.EmailID = eg.EmailID			
			JOIN EmailDataValues edv with(nolock) on e.EmailID = edv.EmailID
			LEFT OUTER JOIN @ValueTable vt on edv.DataValue = vt.Value
		WHERE edv.GroupDataFieldsID = case when @GDFID = -1 then edv.GroupDataFieldsID when @GDFID > -1 then @GDFID end
			  and ISNULL(vt.ID,0) >= case when @GDFID = -1 then 0 when @GDFID > -1 then 1 end
			  and eg.GroupID = @GroupID			  
			  and eg.SubscribeTypeCode = 'S'
			  and ISNULL(e.User1,'') <> ''
			  and cast(ISNULL(eg.LastChanged,eg.CreatedOn) as date) >= cast(@DateFrom as date)
	END
END
ELSE if @Job1 = 0
BEGIN
	if @DoFullNXTBookSync = 1
	BEGIN
		SELECT @GDFID = ISNULL(GroupDataFieldsID,-1) FROM GroupDatafields gdf where gdf.GroupID = @GroupID and gdf.ShortName = @Field and ISNULL(gdf.IsDeleted,0) = 0

		select distinct e.*, ISNULL(edv.DataValue,'') AS DataValue from Emails e with(nolock)
			join EmailGroups eg with(Nolock) on e.EmailID = eg.EmailID
			join GroupDatafields gdf with(nolock) on eg.GroupID = gdf.GroupID and gdf.GroupDatafieldsID = @GDFID
			left outer join EmailDataValues edv with(nolock) on e.EmailID = edv.EmailID and gdf.GroupDatafieldsID = edv.GroupDatafieldsID
		where eg.GroupID = @GroupID and ISNULL(edv.DataValue,'') = ''

	END
	ELSE
	BEGIN
		SELECT @GDFID = ISNULL(GroupDataFieldsID,-1) FROM GroupDatafields gdf where gdf.GroupID = @GroupID and gdf.ShortName = @Field and ISNULL(gdf.IsDeleted,0) = 0

		SELECT distinct e.* , ISNULL(edv.DataValue,'') AS DataValue
		FROM Emails e with(nolock)
			join EmailGroups eg with(Nolock) on e.EmailID = eg.EmailID
			join GroupDatafields gdf with(nolock) on eg.GroupID = gdf.GroupID and gdf.GroupDatafieldsID = @GDFID
			left outer join EmailDataValues edv with(nolock) on e.EmailID = edv.EmailID and gdf.GroupDatafieldsID = edv.GroupDatafieldsID
			left outer join @ValueTable vt on ISNULL(edv.DataValue,'') = vt.Value
		WHERE
			ISNULL(vt.ID,0) >= case when @GDFID = -1 then 0
									when @GDFID > -1 then 1 end		 
			and eg.GroupID = @GroupID			   			
			

	END
END