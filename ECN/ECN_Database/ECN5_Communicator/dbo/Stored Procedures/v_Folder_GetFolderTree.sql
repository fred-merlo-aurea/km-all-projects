CREATE PROCEDURE [dbo].[v_Folder_GetFolderTree]  
(
	@customerID Int,
	@userID int,
	@FolderType varchar(10) 
) as 
Begin
	declare @space varchar(500)

	set @space = '&nbsp;'

	set nocount on

	declare @Folder Table  (
		FolderID int,
		FolderName  varchar(800),
		FolderDescription varchar(500),
		ParentID int,
		FolderType char(10),
		SystemFolder char(1),
		levelno int,
		sort varchar(100),
		DateCreated datetime,
		Items int
	)
		
declare @i int
select @i = 1

if @FolderType = 'GRP'  
	Begin  
		insert into @folder 
		SELECT f.FolderID, @space+'<sub><img src=''/ecn.images/images/L.gif''></sub>&nbsp;<sub><img src=''/ecn.communicator/images/ecn-icon-folder.png''></sub>&nbsp;&nbsp;'+f.FolderName, f.FolderDescription, f.parentID, @FolderType as FolderType, f.IsSystem, 2, f.FolderID, f.CreatedDate, COUNT(g.groupID)
		FROM 	
			Folder f WITH (NOLOCK) LEFT OUTER JOIN [Groups] g WITH (NOLOCK) ON f.folderID = g.folderID
		WHERE 
			f.CustomerID = @customerID AND FolderType = @FolderType AND parentID = 0 and f.IsDeleted = 0
		GROUP BY f.FolderID, f.FolderName, f.FolderDescription, f.parentID, f.IsSystem, f.FolderID, f.CreatedDate

		while @@rowcount > 0
		begin
		     select @i = @i + 1
			set @space = @space + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'
			insert @folder
			SELECT f.FolderID, @space + '<sub><img src=''/ecn.images/images/L.gif''></sub>&nbsp;<sub><img src=''/ecn.communicator/images/ecn-icon-folder.png''></sub>&nbsp;'+ f.FolderName,f.FolderDescription, f.parentID, @FolderType as FolderType, f.IsSystem, @i+1, Convert
(varchar,f1.sort)+'-'+Convert(varchar,f.FolderID), f.CreatedDate, COUNT(g.groupID)
			FROM 	   
				Folder f WITH (NOLOCK) join @folder f1 on F1.folderID = f.parentID  LEFT OUTER JOIN [Groups] g WITH (NOLOCK) ON f.folderID = g.folderID
			WHERE  
				F.CustomerID = @customerID AND F.FolderType =@FolderType and f.IsDeleted = 0 and f1.levelno= @i and
				not exists (select folderID from @folder where folderID = f.FolderID)
			GROUP BY f.FolderID, f.FolderName,f.FolderDescription, f.parentID, f.IsSystem, Convert(varchar,f1.sort)+'-'+Convert(varchar,f.FolderID), f.CreatedDate
		end
	END
else if @FolderType = 'CNT'  
	Begin  
		insert into @folder 
		SELECT f.FolderID, @space+'<sub><img src=''/ecn.images/images/L.gif''></sub>&nbsp;<sub><img src=''/ecn.communicator/images/ecn-icon-folder.png''></sub>&nbsp;&nbsp;'+f.FolderName, f.FolderDescription, f.parentID, @FolderType as FolderType, f.IsSystem, 2, f.FolderID, f.CreatedDate, 
		--(COUNT(distinct c.contentID) + COUNT(distinct l.LayoutID))
		(select COUNT(distinct contentID) from Content c WITH (NOLOCK) where c.CustomerID = @customerID and c.FolderID = f.folderID and c.IsDeleted = 0) + (select COUNT(distinct layoutID) from Layout l WITH (NOLOCK) where l.CustomerID = @customerID and l.FolderID = f.folderID and l.IsDeleted = 0)
		FROM 	
			Folder f WITH (NOLOCK) --LEFT OUTER JOIN Content c WITH (NOLOCK) ON f.folderID = c.folderID and c.IsDeleted = 0 LEFT OUTER JOIN Layout l WITH (NOLOCK) on f.FolderID = l.FolderID and l.IsDeleted = 0
		WHERE 
			f.CustomerID = @customerID AND FolderType = @FolderType AND parentID = 0 and f.IsDeleted = 0
		GROUP BY f.FolderID, f.FolderName, f.FolderDescription, f.parentID, f.IsSystem, f.FolderID, f.CreatedDate

		while @@rowcount > 0
		begin
		     select @i = @i + 1
			set @space = @space + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'
			insert @folder
			SELECT f.FolderID, @space +'<sub><img src=''/ecn.images/images/L.gif''></sub>&nbsp;<sub><img src=''/ecn.communicator/images/ecn-icon-folder.png''></sub>&nbsp;'+ f.FolderName,f.FolderDescription, f.parentID, @FolderType as FolderType, f.IsSystem, @i+1, Convert(
varchar,f1.sort)+'-'+Convert(varchar,f.FolderID), f.CreatedDate,
			--(COUNT(distinct c.contentID) + COUNT(distinct l.LayoutID))
			(select COUNT(distinct contentID) from Content c WITH (NOLOCK) where c.CustomerID = @customerID and c.FolderID = f.folderID and c.IsDeleted = 0) + (select COUNT(distinct layoutID) from Layout l WITH (NOLOCK) where l.CustomerID = @customerID and l.FolderID = f.folderID and l.IsDeleted = 0)
			FROM 	   
				Folder f WITH (NOLOCK) join @folder f1 on F1.folderID = f.parentID  --LEFT OUTER JOIN Content c WITH (NOLOCK) ON f.folderID = c.folderID and c.IsDeleted = 0 LEFT OUTER JOIN Layout l WITH (NOLOCK) on f.FolderID = l.FolderID and l.IsDeleted = 0 
			WHERE  
				F.CustomerID = @customerID AND F.FolderType =@FolderType and f.IsDeleted = 0 and f1.levelno= @i and
				not exists (select folderID from @folder where folderID = f.FolderID)
			GROUP BY f.FolderID, f.FolderName,f.FolderDescription, f.parentID, f.IsSystem, Convert(varchar,f1.sort)+'-'+Convert(varchar,f.FolderID), f.CreatedDate
		end
	END

	select * from @folder 
	UNION
	select 0 as 'FolderID', '&nbsp;<sub><img src=''/ecn.communicator/images/ecn-icon-folder.png''></sub>&nbsp;Root' AS 'FolderName', '<b>ROOT FOLDER</b>' AS 'FolderDescription', 0 AS 'ParentID', @FolderType AS 'FolderType', 'Y' as 'SystemFolder', 1,'0', null as 'DateCreated', ''
	order by sort, levelno
End