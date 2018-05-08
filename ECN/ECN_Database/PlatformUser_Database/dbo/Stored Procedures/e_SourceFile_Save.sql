
CREATE PROCEDURE [dbo].[e_SourceFile_Save]
@SourceFileID int,
@FileRecurrenceTypeId int,
@DatabaseFileTypeId int,
@FileName varchar(100),
@ClientID int,
@PublicationID int,
@IsDeleted bit,
@IsIgnored bit,
@FileSnippetID int,
@Extension varchar(10),
@IsDQMReady bit = 'true',
@Delimiter varchar(10),
@IsTextQualifier bit,
@ServiceID int,
@ServiceFeatureID int,
@MasterGroupID int,
@UseRealTimeGeocoding bit = 'false',
@IsSpecialFile bit,
@ClientCustomProcedureID int,
@SpecialFileResultID int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@QDateFormat varchar(20),
@BatchSize int = 2500

AS
if @DatabaseFileTypeId = 0
	begin
		set @DatabaseFileTypeId = 131
	end

IF @SourceFileID > 0
      BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END				
			
        UPDATE SourceFile
        SET   FileName = @FileName,
			  FileRecurrenceTypeId = @FileRecurrenceTypeId,
			  DatabaseFileTypeId = @DatabaseFileTypeId,
              ClientID = @ClientID,
			  PublicationID = @PublicationID,
              IsDeleted = @IsDeleted,  
              IsIgnored = @IsIgnored,
              FileSnippetID = @FileSnippetID,  
              Extension = @Extension,        
              IsDQMReady = @IsDQMReady,                       
              DateUpdated = @DateUpdated,
			  UpdatedByUserID = @UpdatedByUserID,
			  Delimiter = @Delimiter,
			  IsTextQualifier = @IsTextQualifier,
			  IsSpecialFile = @IsSpecialFile,
			  ClientCustomProcedureID = @ClientCustomProcedureID,
			  SpecialFileResultID = @SpecialFileResultID,
			  ServiceID = @ServiceID,
			  ServiceFeatureID = @ServiceFeatureID,
			  MasterGroupID = @MasterGroupID,
			  UseRealTimeGeocoding = @UseRealTimeGeocoding,
			  QDateFormat = @QDateFormat,
			  BatchSize = @BatchSize	  		  
        WHERE SourceFileID = @SourceFileID;

        SELECT @SourceFileID;
      END
ELSE
      BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
        INSERT INTO SourceFile (FileName, FileRecurrenceTypeId, DatabaseFileTypeId, ClientID, PublicationID, IsDeleted, IsIgnored, FileSnippetID, Extension, IsDQMReady, DateCreated,CreatedByUserID, Delimiter, IsTextQualifier,IsSpecialFile,ClientCustomProcedureID,SpecialFileResultID,ServiceID,ServiceFeatureID,MasterGroupID,UseRealTimeGeocoding,QDateFormat,BatchSize)
        VALUES(@FileName, @FileRecurrenceTypeId, @DatabaseFileTypeId, @ClientID, @PublicationID, @IsDeleted, @IsIgnored, @FileSnippetID,@Extension,@IsDQMReady,@DateCreated,@CreatedByUserID, @Delimiter, @IsTextQualifier,@IsSpecialFile,@ClientCustomProcedureID,@SpecialFileResultID,@ServiceID,@ServiceFeatureID,@MasterGroupID,@UseRealTimeGeocoding,@QDateFormat,@BatchSize);SELECT @@IDENTITY;
      END
GO



