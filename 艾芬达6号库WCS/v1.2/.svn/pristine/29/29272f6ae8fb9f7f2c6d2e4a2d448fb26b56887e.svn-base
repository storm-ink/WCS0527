USE [wcs_logs]
GO

/****** Object:  Table [dbo].[Logs]    Script Date: 05/01/2014 13:45:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Logs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LongDate] [datetime2](7) NOT NULL,
	[Level] [nvarchar](10) NOT NULL,
	[ThreadId] [int] NOT NULL,
	[Logger] [nvarchar](255) NOT NULL,
	[Message] [nvarchar](4000) NULL,
	[Exception] [nvarchar](4000) NULL,
	[Sender] [nvarchar](255) NULL,
	[MachineName] [nvarchar](50) NULL,
	[ProcessName] [nvarchar](50) NULL,
	[UserName] [nvarchar](20) NULL,
	[EquipmentTaskId] [int] NULL,
	[TaskCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Index [IX_Logs_Level]    Script Date: 05/01/2014 13:45:20 ******/
CREATE NONCLUSTERED INDEX [IX_Logs_Level] ON [dbo].[Logs] 
(
	[Level] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_Logs_Level_LongDate]    Script Date: 05/01/2014 13:45:34 ******/
CREATE NONCLUSTERED INDEX [IX_Logs_Level_LongDate] ON [dbo].[Logs] 
(
	[Level] ASC,
	[LongDate] ASC
)
INCLUDE ( [Exception]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_Logs_LongDate] ON [dbo].[Logs] 
(
	[LongDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX [IX_Logs_Thread] ON [dbo].[Logs] 
(
	[ThreadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO



