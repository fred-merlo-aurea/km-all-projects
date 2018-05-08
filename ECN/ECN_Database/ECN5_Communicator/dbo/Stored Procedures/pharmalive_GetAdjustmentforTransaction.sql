CREATE proc [dbo].[pharmalive_GetAdjustmentforTransaction]
(
	@emailID int,
	@entryID varchar(100)
)
as
Begin
		select	edv.entryID,
				max(case when shortname='TransEntryID' then datavalue end) as TransEntryID,
				convert(datetime,max(case when shortname='AdjDate' then datavalue end)) as AdjDate,
				max(case when shortname='AdjType' then datavalue end) as AdjType,
				max(case when shortname='AdjAmount' then datavalue end) as AdjAmount,
				max(case when shortname='AdjExpDate' then datavalue end) as AdjExpDate,
				max(case when shortname='AdjDesc' then datavalue end) as AdjDesc
		from	
				(
					select entryID, gdf1.groupID from emaildatavalues edv1 join groupdatafields gdf1 on edv1.groupdatafieldsID = gdf1.groupdatafieldsID
					where  edv1.emailID = @emailID and gdf1.groupID in (select GroupID from ecn_misc..CANON_PAIDPUB_eNewsletters where CustomerID = 2069) and
							gdf1.shortname = 'TransEntryID' and datavalue = @entryID 
				) inn join
				groupdatafields gdf on gdf.groupID = inn.groupID join
				emaildatavalues edv on edv.entryID = inn.entryID and edv.groupdatafieldsID = gdf.groupdatafieldsID 
		where
			edv.emailID = @emailID
		group by 
				edv.entryID
		order by Adjdate asc
end
