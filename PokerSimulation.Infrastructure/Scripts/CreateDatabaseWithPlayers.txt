/****** Object:  Table [dbo].[Session]    Script Date: 17/05/2017 11:52:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Session](
	[Id] [uniqueidentifier] NOT NULL,
	[Player1Id] [uniqueidentifier] NOT NULL,
	[Player2Id] [uniqueidentifier] NOT NULL,
	[State] [int] NOT NULL,
	[TotalHandsCount] [int] NOT NULL,
	[PlayedHandsCount] [int] NOT NULL,
 CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[Player]    Script Date: 17/05/2017 11:52:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Player](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nchar](50) NOT NULL,
	[Description] [nchar](250) NULL,
	[Type] [int] NULL,
 CONSTRAINT [PK_Player] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[PlayedHand]    Script Date: 17/05/2017 11:52:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PlayedHand](
	[Id] [uniqueidentifier] NOT NULL,
	[WinnerId] [uniqueidentifier] NULL,
	[PotSize] [int] NOT NULL,
	[Timestamp] [datetime] NULL,
	[SessionId] [uniqueidentifier] NULL,
	[Holecards1] [nchar](5) NULL,
	[Holecards2] [nchar](5) NULL,
	[Board] [nchar](14) NULL,
	[AmountWon] [int] NULL,
 CONSTRAINT [PK_PlayedHand] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PlayedHand]  WITH CHECK ADD  CONSTRAINT [FK_Session_PlayedHand_Cascade] FOREIGN KEY([SessionId])
REFERENCES [dbo].[Session] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[PlayedHand] CHECK CONSTRAINT [FK_Session_PlayedHand_Cascade]
GO

/****** Object:  Table [dbo].[GameAction]    Script Date: 17/05/2017 11:52:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GameAction](
	[Id] [uniqueidentifier] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[HandId] [uniqueidentifier] NOT NULL,
	[PlayerId] [uniqueidentifier] NOT NULL,
	[ActionType] [int] NOT NULL,
	[Amount] [int] NOT NULL,
 CONSTRAINT [PK_GameAction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GameAction]  WITH CHECK ADD  CONSTRAINT [FK_PlayedHand_GameAction_Cascade] FOREIGN KEY([HandId])
REFERENCES [dbo].[PlayedHand] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[GameAction] CHECK CONSTRAINT [FK_PlayedHand_GameAction_Cascade]
GO

INSERT [dbo].[Player] ([Id], [Name], [Description], [Type]) VALUES (N'6be69315-40cc-466b-a88a-0b8c4f01f4db', N'RandomBot1                                        ', N'This bot is stupid and just randomly selects an option.                                                                                                                                                                                                   ', 3)
GO
INSERT [dbo].[Player] ([Id], [Name], [Description], [Type]) VALUES (N'a3fbede8-41c5-4a0a-b36b-20885ab8eacc', N'AlwaysMinRaiseBot                                 ', N'This bot always minraises, no matter what.                                                                                                                                                                                                                ', 2)
GO
INSERT [dbo].[Player] ([Id], [Name], [Description], [Type]) VALUES (N'492539d7-3fbc-4f31-897d-3d22ceac3d63', N'RandomBot2                                        ', N'This bot is stupid and just randomly selects an option.                                                                                                                                                                                                   ', 3)
GO
INSERT [dbo].[Player] ([Id], [Name], [Description], [Type]) VALUES (N'd91f0046-ca67-4ee4-a183-dadc18d49ae9', N'Callingstation                                    ', N'This bot always calls, that is why it is called Callingstation.                                                                                                                                                                                           ', 1)
GO
