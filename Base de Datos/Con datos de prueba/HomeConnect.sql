USE [master]
GO
/****** Object:  Database [HomeConnect]    Script Date: 21/11/2024 17:05:34 ******/
CREATE DATABASE [HomeConnect]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HomeConnect', FILENAME = N'/var/opt/mssql/data/HomeConnect.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'HomeConnect_log', FILENAME = N'/var/opt/mssql/data/HomeConnect_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [HomeConnect] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HomeConnect].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HomeConnect] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HomeConnect] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HomeConnect] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HomeConnect] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HomeConnect] SET ARITHABORT OFF 
GO
ALTER DATABASE [HomeConnect] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HomeConnect] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HomeConnect] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HomeConnect] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HomeConnect] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HomeConnect] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HomeConnect] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HomeConnect] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HomeConnect] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HomeConnect] SET  ENABLE_BROKER 
GO
ALTER DATABASE [HomeConnect] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HomeConnect] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HomeConnect] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HomeConnect] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HomeConnect] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HomeConnect] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [HomeConnect] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HomeConnect] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HomeConnect] SET  MULTI_USER 
GO
ALTER DATABASE [HomeConnect] SET PAGE_VERIFY NONE  
GO
ALTER DATABASE [HomeConnect] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HomeConnect] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HomeConnect] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [HomeConnect] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [HomeConnect] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'HomeConnect', N'ON'
GO
ALTER DATABASE [HomeConnect] SET QUERY_STORE = ON
GO
ALTER DATABASE [HomeConnect] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [HomeConnect]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Businesses]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Businesses](
	[Rut] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[Logo] [nvarchar](max) NOT NULL,
	[Validator] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Businesses] PRIMARY KEY CLUSTERED 
(
	[Rut] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Devices]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Devices](
	[Id] [uniqueidentifier] NOT NULL,
	[BusinessRut] [nvarchar](450) NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ModelNumber] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[MainPhoto] [nvarchar](max) NOT NULL,
	[SecondaryPhotos] [nvarchar](max) NOT NULL,
	[Type] [int] NOT NULL,
	[Discriminator] [nvarchar](8) NOT NULL,
	[MotionDetection] [bit] NULL,
	[PersonDetection] [bit] NULL,
	[IsExterior] [bit] NULL,
	[IsInterior] [bit] NULL,
 CONSTRAINT [PK_Devices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HomePermissions]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HomePermissions](
	[Value] [nvarchar](max) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_HomePermissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Homes]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Homes](
	[Id] [uniqueidentifier] NOT NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
	[MaxMembers] [int] NOT NULL,
	[NickName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Homes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MemberHomePermissions]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemberHomePermissions](
	[MemberId] [uniqueidentifier] NOT NULL,
	[HomePermissionsId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_MemberHomePermissions] PRIMARY KEY CLUSTERED 
(
	[HomePermissionsId] ASC,
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Members]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Members](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[HomeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Members] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [uniqueidentifier] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Read] [bit] NOT NULL,
	[Event] [nvarchar](max) NOT NULL,
	[OwnedDeviceHardwareId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OwnedDevices]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OwnedDevices](
	[HardwareId] [uniqueidentifier] NOT NULL,
	[HomeId] [uniqueidentifier] NOT NULL,
	[DeviceId] [uniqueidentifier] NOT NULL,
	[Connected] [bit] NOT NULL,
	[DeviceType] [nvarchar](21) NOT NULL,
	[IsOpen] [bit] NULL,
	[State] [bit] NULL,
	[Name] [nvarchar](max) NULL,
	[RoomId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_OwnedDevices] PRIMARY KEY CLUSTERED 
(
	[HardwareId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[Value] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Name] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleSystemPermission]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleSystemPermission](
	[PermissionsValue] [nvarchar](450) NOT NULL,
	[RolesName] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_RoleSystemPermission] PRIMARY KEY CLUSTERED 
(
	[PermissionsValue] ASC,
	[RolesName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rooms]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rooms](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[HomeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tokens]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tokens](
	[Id] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Tokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[RolesName] [nvarchar](450) NOT NULL,
	[UsersId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[RolesName] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 21/11/2024 17:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Surname] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[CreatedAt] [date] NOT NULL,
	[ProfilePicture] [nvarchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_Businesses_OwnerId]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_Businesses_OwnerId] ON [dbo].[Businesses]
(
	[OwnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Devices_BusinessRut]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_Devices_BusinessRut] ON [dbo].[Devices]
(
	[BusinessRut] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Homes_OwnerId]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_Homes_OwnerId] ON [dbo].[Homes]
(
	[OwnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MemberHomePermissions_MemberId]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_MemberHomePermissions_MemberId] ON [dbo].[MemberHomePermissions]
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Members_HomeId]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_Members_HomeId] ON [dbo].[Members]
(
	[HomeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Members_UserId]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_Members_UserId] ON [dbo].[Members]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notifications_OwnedDeviceHardwareId]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_Notifications_OwnedDeviceHardwareId] ON [dbo].[Notifications]
(
	[OwnedDeviceHardwareId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notifications_UserId]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_Notifications_UserId] ON [dbo].[Notifications]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OwnedDevices_DeviceId]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_OwnedDevices_DeviceId] ON [dbo].[OwnedDevices]
(
	[DeviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OwnedDevices_HomeId]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_OwnedDevices_HomeId] ON [dbo].[OwnedDevices]
(
	[HomeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OwnedDevices_RoomId]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_OwnedDevices_RoomId] ON [dbo].[OwnedDevices]
(
	[RoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RoleSystemPermission_RolesName]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_RoleSystemPermission_RolesName] ON [dbo].[RoleSystemPermission]
(
	[RolesName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Rooms_HomeId]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_Rooms_HomeId] ON [dbo].[Rooms]
(
	[HomeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Tokens_UserId]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_Tokens_UserId] ON [dbo].[Tokens]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserRole_UsersId]    Script Date: 21/11/2024 17:05:34 ******/
CREATE NONCLUSTERED INDEX [IX_UserRole_UsersId] ON [dbo].[UserRole]
(
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Businesses] ADD  DEFAULT (N'') FOR [Logo]
GO
ALTER TABLE [dbo].[Devices] ADD  DEFAULT (N'') FOR [Discriminator]
GO
ALTER TABLE [dbo].[HomePermissions] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [Id]
GO
ALTER TABLE [dbo].[MemberHomePermissions] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [HomePermissionsId]
GO
ALTER TABLE [dbo].[Members] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [HomeId]
GO
ALTER TABLE [dbo].[OwnedDevices] ADD  DEFAULT (CONVERT([bit],(0))) FOR [Connected]
GO
ALTER TABLE [dbo].[OwnedDevices] ADD  DEFAULT (N'') FOR [DeviceType]
GO
ALTER TABLE [dbo].[Tokens] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [UserId]
GO
ALTER TABLE [dbo].[Businesses]  WITH CHECK ADD  CONSTRAINT [FK_Businesses_Users_OwnerId] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Businesses] CHECK CONSTRAINT [FK_Businesses_Users_OwnerId]
GO
ALTER TABLE [dbo].[Devices]  WITH CHECK ADD  CONSTRAINT [FK_Devices_Businesses_BusinessRut] FOREIGN KEY([BusinessRut])
REFERENCES [dbo].[Businesses] ([Rut])
GO
ALTER TABLE [dbo].[Devices] CHECK CONSTRAINT [FK_Devices_Businesses_BusinessRut]
GO
ALTER TABLE [dbo].[Homes]  WITH CHECK ADD  CONSTRAINT [FK_Homes_Users_OwnerId] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Homes] CHECK CONSTRAINT [FK_Homes_Users_OwnerId]
GO
ALTER TABLE [dbo].[MemberHomePermissions]  WITH CHECK ADD  CONSTRAINT [FK_MemberHomePermissions_HomePermissions_HomePermissionsId] FOREIGN KEY([HomePermissionsId])
REFERENCES [dbo].[HomePermissions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MemberHomePermissions] CHECK CONSTRAINT [FK_MemberHomePermissions_HomePermissions_HomePermissionsId]
GO
ALTER TABLE [dbo].[MemberHomePermissions]  WITH CHECK ADD  CONSTRAINT [FK_MemberHomePermissions_Members_MemberId] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Members] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MemberHomePermissions] CHECK CONSTRAINT [FK_MemberHomePermissions_Members_MemberId]
GO
ALTER TABLE [dbo].[Members]  WITH CHECK ADD  CONSTRAINT [FK_Members_Homes_HomeId] FOREIGN KEY([HomeId])
REFERENCES [dbo].[Homes] ([Id])
GO
ALTER TABLE [dbo].[Members] CHECK CONSTRAINT [FK_Members_Homes_HomeId]
GO
ALTER TABLE [dbo].[Members]  WITH CHECK ADD  CONSTRAINT [FK_Members_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Members] CHECK CONSTRAINT [FK_Members_Users_UserId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_OwnedDevices_OwnedDeviceHardwareId] FOREIGN KEY([OwnedDeviceHardwareId])
REFERENCES [dbo].[OwnedDevices] ([HardwareId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_OwnedDevices_OwnedDeviceHardwareId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Users_UserId]
GO
ALTER TABLE [dbo].[OwnedDevices]  WITH CHECK ADD  CONSTRAINT [FK_OwnedDevices_Devices_DeviceId] FOREIGN KEY([DeviceId])
REFERENCES [dbo].[Devices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OwnedDevices] CHECK CONSTRAINT [FK_OwnedDevices_Devices_DeviceId]
GO
ALTER TABLE [dbo].[OwnedDevices]  WITH CHECK ADD  CONSTRAINT [FK_OwnedDevices_Homes_HomeId] FOREIGN KEY([HomeId])
REFERENCES [dbo].[Homes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OwnedDevices] CHECK CONSTRAINT [FK_OwnedDevices_Homes_HomeId]
GO
ALTER TABLE [dbo].[OwnedDevices]  WITH CHECK ADD  CONSTRAINT [FK_OwnedDevices_Rooms_RoomId] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Rooms] ([Id])
GO
ALTER TABLE [dbo].[OwnedDevices] CHECK CONSTRAINT [FK_OwnedDevices_Rooms_RoomId]
GO
ALTER TABLE [dbo].[RoleSystemPermission]  WITH CHECK ADD  CONSTRAINT [FK_RoleSystemPermission_Permissions_PermissionsValue] FOREIGN KEY([PermissionsValue])
REFERENCES [dbo].[Permissions] ([Value])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleSystemPermission] CHECK CONSTRAINT [FK_RoleSystemPermission_Permissions_PermissionsValue]
GO
ALTER TABLE [dbo].[RoleSystemPermission]  WITH CHECK ADD  CONSTRAINT [FK_RoleSystemPermission_Roles_RolesName] FOREIGN KEY([RolesName])
REFERENCES [dbo].[Roles] ([Name])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleSystemPermission] CHECK CONSTRAINT [FK_RoleSystemPermission_Roles_RolesName]
GO
ALTER TABLE [dbo].[Rooms]  WITH CHECK ADD  CONSTRAINT [FK_Rooms_Homes_HomeId] FOREIGN KEY([HomeId])
REFERENCES [dbo].[Homes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Rooms] CHECK CONSTRAINT [FK_Rooms_Homes_HomeId]
GO
ALTER TABLE [dbo].[Tokens]  WITH CHECK ADD  CONSTRAINT [FK_Tokens_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tokens] CHECK CONSTRAINT [FK_Tokens_Users_UserId]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Roles_RolesName] FOREIGN KEY([RolesName])
REFERENCES [dbo].[Roles] ([Name])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_Roles_RolesName]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_Users_UsersId]
GO
USE [master]
GO
ALTER DATABASE [HomeConnect] SET  READ_WRITE 
GO
