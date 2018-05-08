CREATE proc [dbo].[sp_SaveEdition]   
(  
	@EditionID    int,  
	@EditionName    varchar(100),  
	@PublicationID   int,  
	@status    varchar(10),  
	@filename   varchar(100),  
	@Totalpages   int,  
	@ActivateDate  varchar(25),  
	@DeactivateDate  varchar(25),  
	@IsSearchEnabled bit,
	@IsLoginRequired bit,  
	@TOC    Text  
)  
as  
Begin  

	declare @currentstatus    varchar(10),
			@EditionHistoryID int	
	if exists(select EditionID from [EDITION] where EditionName = @EditionName and EditionID <> @EditionID and PublicationID = @PublicationID)  
	Begin  
		RAISERROR('Edition already exists!!!',16,1)  
	end  
	else  
	begin  
		if @EditionID = 0   
		Begin  
			Insert into [EDITION]   
				(EditionName, PublicationID, Status, FileName, Pages, EnableDate, DisableDate, IsSearchable, IsLoginRequired, xmlTOC, CreatedDate)   
			values  
				(@EditionName, @PublicationID, @status, @filename, @totalpages, (case when len(@ActivateDate)=0 then NULL else @ActivateDate end), (case when len(@DeactivateDate)=0 then NULL else @DeactivateDate end), @IsSearchEnabled, @IsLoginRequired, @TOC, getdate
())  
   
			set @EditionID = @@IDENTITY  

			if @status = 'active'
				insert into editionHistory (editionID, ActivatedDate) values (@EditionID, getdate())
			if @status = 'archieve'
				insert into editionHistory (editionID, ArchievedDate) values (@EditionID, getdate())

		End  
		Else  
		Begin  

			select @currentstatus=status from [EDITION] where EditionID = @EditionID

			Update [EDITION]  
				Set EditionName = @EditionName,  
					PublicationID = @PublicationID,  
					Status = @Status,  
					FileName = (case when @FileName = '' then FileName else @FileName end),  
					Pages = (case when @totalpages = 0 then Pages else @totalpages end),  
					EnableDate = (case when len(@ActivateDate)=0 then NULL else @ActivateDate end),  
					DisableDate = (case when len(@DeactivateDate)=0 then NULL else @DeactivateDate end),  
					IsSearchable = @IsSearchEnabled,  
					IsLoginRequired = @IsLoginRequired,
					xmlTOC = (case when Convert(varchar,@TOC) = '' then xmlTOC else @TOC end),  
					UpdatedDate = getdate()  
			where  
				EditionID = @EditionID  

			-- Update History
			if @currentstatus <> @status
			Begin
				if @status = 'active'
				Begin
					if exists (select editionhistoryID from editionHistory where editionID = @editionID and isnull(ArchievedDate,'') <> '' and isnull(DeActivatedDate,'') = '')
					Begin
						select @EditionHistoryID =  editionhistoryID from editionHistory where editionID = @editionID and isnull(ArchievedDate,'') <> '' and isnull(DeActivatedDate,'') = ''
						
						-- set the inactive date if archieve exists.
						update EditionHistory
						set DeActivatedDate = getdate()
						where editionhistoryID = @EditionHistoryID
					end

					insert into editionHistory (editionID, ActivatedDate) values (@EditionID, getdate())
				end
				else if @status = 'archieve'
				Begin
					if exists (select editionhistoryID from editionHistory where editionID = @editionID and isnull(ActivatedDate,'') <> '' and isnull(DeActivatedDate,'') = '')
					Begin
						select @EditionHistoryID =  editionhistoryID from editionHistory where editionID = @editionID and isnull(ActivatedDate,'') <> '' and isnull(DeActivatedDate,'') = ''
						
						update EditionHistory
						set DeActivatedDate = getdate()
						where editionhistoryID = @EditionHistoryID

					end

					insert into editionHistory (editionID, ArchievedDate) values (@EditionID, getdate())
				end
				else if @status = 'inactive'
				Begin
					select @EditionHistoryID =  editionhistoryID from editionHistory where editionID = @editionID and (isnull(ActivatedDate,'') <> '' or isnull(ArchievedDate,'') <> '') and isnull(DeActivatedDate,'') = ''
						
					-- set the inactive date if archieve exists.
					update EditionHistory
					set DeActivatedDate = getdate()
					where editionhistoryID = @EditionHistoryID
				end
			end
		End  
		
		select @EditionID as ID  
	end  
End
