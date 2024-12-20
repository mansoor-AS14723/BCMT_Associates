
USE [BCMT_DB]
GO
/****** Object:  Table [dbo].[Menus]    Script Date: 11/23/2024 3:03:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Icon] [varchar](50) NULL,
	[Url] [varchar](500) NOT NULL,
	[ParentId] [int] NOT NULL,
 CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleMenu]    Script Date: 11/23/2024 3:03:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleMenu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RolesId] [int] NOT NULL,
	[MenusId] [int] NOT NULL,
 CONSTRAINT [PK_RoleMenu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 11/23/2024 3:03:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](25) NOT NULL,
	[Title] [varchar](255) NULL,
	[Description] [varchar](max) NULL,
	[Access] [varchar](max) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/23/2024 3:03:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](200) NULL,
	[MiddleName] [varchar](200) NULL,
	[LastName] [varchar](200) NULL,
	[Gender] [char](1) NULL,
	[DOB] [date] NULL,
	[Email] [varchar](50) NULL,
	[ContactNo] [varchar](20) NULL,
	[Username] [varchar](200) NULL,
	[Password] [varchar](1000) NULL,
	[RoleId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[ProfilePicture] [varbinary](max) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Menus] ON 

INSERT [dbo].[Menus] ([Id], [Name], [Icon], [Url], [ParentId]) VALUES (1, N'Users', N'fa-fa-users', N'Users', 0)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [Url], [ParentId]) VALUES (2, N'List', N'fa-fa-users', N'/User/Index', 1)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [Url], [ParentId]) VALUES (3, N'Add', N'fa-fa-users', N'/User/Add', 1)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [Url], [ParentId]) VALUES (4, N'Edit', N'fa-fa-users', N'/User/Update', 1)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [Url], [ParentId]) VALUES (5, N'Delete', N'fa-fa-users', N'/User/Delete', 1)
SET IDENTITY_INSERT [dbo].[Menus] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([ID], [Name], [Title], [Description], [Access], [IsActive], [IsDeleted]) VALUES (1, N'Super Admin', N'Super Admin', N'Super Admin', NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([ID], [FirstName], [MiddleName], [LastName], [Gender], [DOB], [Email], [ContactNo], [Username], [Password], [RoleId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [ProfilePicture], [IsActive], [IsDeleted]) VALUES (3, N'Imran', NULL, N'Shah', N'1', NULL, N'abc2@hotmail.com', N'123455', N'Imran', N'imran1122', 1, CAST(N'2024-11-20T16:10:51.050' AS DateTime), NULL, CAST(N'2024-11-20T16:10:51.050' AS DateTime), 1, NULL, 1, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
/****** Object:  StoredProcedure [dbo].[RoleMenuGet]    Script Date: 11/23/2024 3:03:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create Procedure [dbo].[RoleMenuGet]
	@RoleId int = null

	
	
AS
BEGIN
	
	Select r.*,r.ID as RolesId,m.Name as MenuName,m.Id as MenusId, m.Url, m.ParentId, m.Icon 
	from Roles  r
	join RoleMenu rm on rm.RolesId =  r.ID
	join Menus m on m.Id = rm.MenusId
	where (r.ID = @RoleId or @RoleId is null) 
END





GO
/****** Object:  StoredProcedure [dbo].[UserLogin]    Script Date: 11/23/2024 3:03:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[UserLogin] --'hilal','hilal1122'
	@Email varchar(200),
	@Password varchar(1000)
	
AS
BEGIN
BEGIN TRY



		Select u.*, r.Name as RoleName
		from Users u with(nolock)
		join Roles r with(nolock) on r.ID = u.RoleId 
		
		Where (u.Email = @Email and u.Password = @Password and u.IsActive = 1) and (u.IsDeleted = 0 or u.IsDeleted is null)


end try
	begin catch
		select 
    ERROR_MESSAGE() AS Message
	end catch
END




GO
USE [master]
GO
ALTER DATABASE [BCMT_DB] SET  READ_WRITE 
GO
