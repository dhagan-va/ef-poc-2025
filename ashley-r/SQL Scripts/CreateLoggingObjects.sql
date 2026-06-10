USE [EDI]
GO
/****** Object:  Schema [log]    Script Date: 6/4/2026 2:04:54 PM ******/
CREATE SCHEMA [log]
GO
/****** Object:  Table [log].[ParsingErrors]    Script Date: 6/4/2026 2:04:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [log].[ParsingErrors](
	[ParsingErrorID] [int] IDENTITY(1,1) NOT NULL,
	[FullFilePath] [varchar](5000) NULL,
	[ErrorMessage] [nvarchar](max) NULL,
	[InsertDate] [datetime2](7) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [log].[ParsingErrors] ADD  DEFAULT (getdate()) FOR [InsertDate]
GO
/****** Object:  StoredProcedure [log].[InsertParsingErrors]    Script Date: 6/4/2026 2:04:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
EXEC EDI.log.InsertParsingErrors 'ExampleFile.TXT', 'Example Error Message';

select *
from EDI.log.ParsingErrors
*/
CREATE PROCEDURE [log].[InsertParsingErrors]
	-- Add the parameters for the stored procedure here
	@FilePath varchar(5000),
	@ErrorMessage nvarchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into EDI.log.ParsingErrors(
		FullFilePath,
		ErrorMessage
	)
	Values
	(@FilePath, @ErrorMessage);

	Select @@RowCount as RecordsInserted;
END
GO
