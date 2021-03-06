USE [1gb_trading]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] DROP CONSTRAINT [FK_webpages_UsersInRoles_Users]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] DROP CONSTRAINT [fk_RoleId]
GO
ALTER TABLE [dbo].[webpages_Membership] DROP CONSTRAINT [FK_webpages_Membership_Users]
GO
ALTER TABLE [dbo].[UserPermissions] DROP CONSTRAINT [FK_UserPermissions_Users]
GO
ALTER TABLE [dbo].[UserPermissions] DROP CONSTRAINT [FK_UserPermissions_Permissions]
GO
ALTER TABLE [dbo].[UserAllowedRoles] DROP CONSTRAINT [FK_UserAllowedRoles_webpages_Roles]
GO
ALTER TABLE [dbo].[Stores] DROP CONSTRAINT [FK_Stores_Shops]
GO
ALTER TABLE [dbo].[Stores] DROP CONSTRAINT [FK_Stores_OrderDelivery]
GO
ALTER TABLE [dbo].[ShopSettings] DROP CONSTRAINT [FK_ShopSettings_Shops]
GO
ALTER TABLE [dbo].[Shops] DROP CONSTRAINT [FK_Shops_TaxPlanes]
GO
ALTER TABLE [dbo].[Shops] DROP CONSTRAINT [FK_Shop_Users]
GO
ALTER TABLE [dbo].[ShopProductChars] DROP CONSTRAINT [FK_ShopProductChars_Users]
GO
ALTER TABLE [dbo].[ShopProductChars] DROP CONSTRAINT [FK_ShopProductChars_Shops]
GO
ALTER TABLE [dbo].[ShopManagers] DROP CONSTRAINT [FK_ShopManagers_Shops]
GO
ALTER TABLE [dbo].[ShopManagers] DROP CONSTRAINT [FK_ShopManagers_Managers]
GO
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_Products_Users]
GO
ALTER TABLE [dbo].[ProductChars] DROP CONSTRAINT [FK_ProductChars_OrderedProducts]
GO
ALTER TABLE [dbo].[PermissionsForRoles] DROP CONSTRAINT [FK_PermissionsForRoles_webpages_Roles]
GO
ALTER TABLE [dbo].[PermissionsForRoles] DROP CONSTRAINT [FK_PermissionsForRoles_Permissions]
GO
ALTER TABLE [dbo].[Permissions] DROP CONSTRAINT [FK_Permissions_PermissionGroups]
GO
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Users1]
GO
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Users]
GO
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Shops]
GO
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_OrderDelivery]
GO
ALTER TABLE [dbo].[OrderedProducts] DROP CONSTRAINT [FK_OrderedProducts_Products]
GO
ALTER TABLE [dbo].[OrderedProducts] DROP CONSTRAINT [FK_OrderedProducts_Orders]
GO
ALTER TABLE [dbo].[OrderChars] DROP CONSTRAINT [FK_OrderChars_Orders]
GO
ALTER TABLE [dbo].[OperatorShops] DROP CONSTRAINT [FK_OperatorShops_Users]
GO
ALTER TABLE [dbo].[OperatorShops] DROP CONSTRAINT [FK_OperatorShops_Shops]
GO
ALTER TABLE [dbo].[Marks] DROP CONSTRAINT [FK_Marks_Users]
GO
ALTER TABLE [dbo].[Marks] DROP CONSTRAINT [FK_Marks_Shops]
GO
ALTER TABLE [dbo].[MapSectors] DROP CONSTRAINT [FK_MapSectors_Stores]
GO
ALTER TABLE [dbo].[MapSectors] DROP CONSTRAINT [FK_MapSectors_Shops]
GO
ALTER TABLE [dbo].[MapPoints] DROP CONSTRAINT [FK_MapPoints_MapSectors]
GO
ALTER TABLE [dbo].[Managers] DROP CONSTRAINT [FK_ShopManagers_Users1]
GO
ALTER TABLE [dbo].[Managers] DROP CONSTRAINT [FK_ShopManagers_Users]
GO
ALTER TABLE [dbo].[LoginHistory] DROP CONSTRAINT [FK_LoginHistory_Users]
GO
ALTER TABLE [dbo].[LogEntry] DROP CONSTRAINT [FK_LogEntry_Users]
GO
ALTER TABLE [dbo].[LogEntry] DROP CONSTRAINT [FK_LogEntry_LogAction]
GO
ALTER TABLE [dbo].[DeliveryAddress] DROP CONSTRAINT [FK_OrderDelivery_MapPoints]
GO
ALTER TABLE [dbo].[Consumers] DROP CONSTRAINT [FK_Consumers_Users]
GO
ALTER TABLE [dbo].[webpages_Roles] DROP CONSTRAINT [DF_webpages_Roles_RoleDescription]
GO
ALTER TABLE [dbo].[webpages_Membership] DROP CONSTRAINT [DF__webpages___Passw__5C229E14]
GO
ALTER TABLE [dbo].[webpages_Membership] DROP CONSTRAINT [DF__webpages___IsCon__5B2E79DB]
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF_Users_IsDeleted]
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF_Users_IsLocked]
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF_Users_IsPhoneConfirmed]
GO
ALTER TABLE [dbo].[Shops] DROP CONSTRAINT [DF_Shops_CreateDate]
GO
ALTER TABLE [dbo].[ShopProductChars] DROP CONSTRAINT [DF_ShopProductChars_LoadInForm]
GO
ALTER TABLE [dbo].[ShopProductChars] DROP CONSTRAINT [DF_ShopProductChars_Type]
GO
ALTER TABLE [dbo].[ShopProductChars] DROP CONSTRAINT [DF_ShopProductChars_IsMain]
GO
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF_Products_UnitCode]
GO
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF_Orders_IsPayed]
GO
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF_Orders_IsImportant]
GO
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF_Orders_IsTemp]
GO
ALTER TABLE [dbo].[MapPoints] DROP CONSTRAINT [DF_MapPoints_Num]
GO
ALTER TABLE [dbo].[Consumers] DROP CONSTRAINT [DF_Consumers_Sex]
GO
ALTER TABLE [dbo].[Consumers] DROP CONSTRAINT [DF_Consumers_Email]
GO
ALTER TABLE [dbo].[Consumers] DROP CONSTRAINT [DF_Consumers_AddPhone]
GO
/****** Object:  Index [UQ__webpages__8A2B6160AE40273D]    Script Date: 31.10.2014 18:47:34 ******/
ALTER TABLE [dbo].[webpages_Roles] DROP CONSTRAINT [UQ__webpages__8A2B6160AE40273D]
GO
/****** Object:  Table [dbo].[webpages_UsersInRoles]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[webpages_UsersInRoles]
GO
/****** Object:  Table [dbo].[webpages_Roles]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[webpages_Roles]
GO
/****** Object:  Table [dbo].[webpages_OAuthMembership]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[webpages_OAuthMembership]
GO
/****** Object:  Table [dbo].[webpages_Membership]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[webpages_Membership]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[UserPermissions]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[UserPermissions]
GO
/****** Object:  Table [dbo].[UserAllowedRoles]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[UserAllowedRoles]
GO
/****** Object:  Table [dbo].[TaxPlanes]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[TaxPlanes]
GO
/****** Object:  Table [dbo].[Stores]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[Stores]
GO
/****** Object:  Table [dbo].[ShopSettings]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[ShopSettings]
GO
/****** Object:  Table [dbo].[Shops]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[Shops]
GO
/****** Object:  Table [dbo].[ShopProductChars]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[ShopProductChars]
GO
/****** Object:  Table [dbo].[ShopManagers]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[ShopManagers]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[Products]
GO
/****** Object:  Table [dbo].[ProductChars]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[ProductChars]
GO
/****** Object:  Table [dbo].[PermissionsForRoles]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[PermissionsForRoles]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[Permissions]
GO
/****** Object:  Table [dbo].[PermissionGroups]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[PermissionGroups]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[Orders]
GO
/****** Object:  Table [dbo].[OrderedProducts]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[OrderedProducts]
GO
/****** Object:  Table [dbo].[OrderChars]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[OrderChars]
GO
/****** Object:  Table [dbo].[OperatorShops]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[OperatorShops]
GO
/****** Object:  Table [dbo].[Marks]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[Marks]
GO
/****** Object:  Table [dbo].[MapSectors]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[MapSectors]
GO
/****** Object:  Table [dbo].[MapPoints]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[MapPoints]
GO
/****** Object:  Table [dbo].[Managers]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[Managers]
GO
/****** Object:  Table [dbo].[LoginHistory]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[LoginHistory]
GO
/****** Object:  Table [dbo].[LogEntry]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[LogEntry]
GO
/****** Object:  Table [dbo].[LogAction]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[LogAction]
GO
/****** Object:  Table [dbo].[DeliveryAddress]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[DeliveryAddress]
GO
/****** Object:  Table [dbo].[Consumers]    Script Date: 31.10.2014 18:47:34 ******/
DROP TABLE [dbo].[Consumers]
GO
USE [master]
GO
/****** Object:  Database [1gb_trading]    Script Date: 31.10.2014 18:47:34 ******/
DROP DATABASE [1gb_trading]
GO
/****** Object:  Database [1gb_trading]    Script Date: 31.10.2014 18:47:34 ******/
CREATE DATABASE [1gb_trading]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'1gb_trading', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\1gb_trading.mdf' , SIZE = 4160KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'1gb_trading_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\1gb_trading_log.ldf' , SIZE = 1040KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [1gb_trading] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [1gb_trading].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [1gb_trading] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [1gb_trading] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [1gb_trading] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [1gb_trading] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [1gb_trading] SET ARITHABORT OFF 
GO
ALTER DATABASE [1gb_trading] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [1gb_trading] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [1gb_trading] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [1gb_trading] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [1gb_trading] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [1gb_trading] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [1gb_trading] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [1gb_trading] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [1gb_trading] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [1gb_trading] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [1gb_trading] SET  ENABLE_BROKER 
GO
ALTER DATABASE [1gb_trading] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [1gb_trading] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [1gb_trading] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [1gb_trading] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [1gb_trading] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [1gb_trading] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [1gb_trading] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [1gb_trading] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [1gb_trading] SET  MULTI_USER 
GO
ALTER DATABASE [1gb_trading] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [1gb_trading] SET DB_CHAINING OFF 
GO
ALTER DATABASE [1gb_trading] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [1gb_trading] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [1gb_trading]
GO
/****** Object:  Table [dbo].[Consumers]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consumers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Surname] [nvarchar](500) NOT NULL,
	[Patrinomic] [nvarchar](500) NULL,
	[Phone] [nvarchar](500) NOT NULL,
	[UserID] [int] NULL,
	[AddPhone] [nvarchar](500) NOT NULL,
	[Email] [nvarchar](500) NOT NULL,
	[Sex] [bit] NOT NULL,
 CONSTRAINT [PK_Consumers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DeliveryAddress]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeliveryAddress](
	[Region] [nvarchar](400) NULL,
	[Town] [nvarchar](400) NULL,
	[Street] [nvarchar](400) NULL,
	[House] [nvarchar](50) NULL,
	[Building] [nvarchar](50) NULL,
	[Doorway] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[Floor] [nvarchar](50) NULL,
	[Flat] [nvarchar](50) NULL,
	[Section] [nvarchar](50) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PointID] [int] NULL,
	[District] [nvarchar](400) NULL,
 CONSTRAINT [PK_OrderDelivery] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LogAction]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogAction](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_LogAction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LogEntry]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogEntry](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ActionDate] [datetime] NOT NULL,
	[UserID] [int] NOT NULL,
	[ActionID] [int] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_LogEntry] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LoginHistory]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[LoginDate] [datetime] NOT NULL,
	[IPAddress] [bigint] NOT NULL,
 CONSTRAINT [PK_LoginHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Managers]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Managers](
	[ManagerUserID] [int] NOT NULL,
	[ShopOwnerID] [int] NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ShopManagers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MapPoints]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MapPoints](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Lat] [decimal](18, 16) NOT NULL,
	[Lng] [decimal](18, 16) NOT NULL,
	[SectorID] [int] NULL,
	[Num] [int] NOT NULL,
 CONSTRAINT [PK_MapPoints] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MapSectors]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MapSectors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ShopID] [int] NOT NULL,
	[StoreID] [int] NULL,
 CONSTRAINT [PK_MapSectors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Marks]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Marks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Quality] [int] NOT NULL,
	[Price] [int] NOT NULL,
	[Operator] [int] NOT NULL,
	[Delivery] [int] NOT NULL,
	[MarkDate] [datetime] NOT NULL,
	[OrderID] [int] NOT NULL,
	[MarkAuthorID] [int] NOT NULL,
 CONSTRAINT [PK_Marks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OperatorShops]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperatorShops](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ShopID] [int] NOT NULL,
 CONSTRAINT [PK_OperatorShops] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderChars]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderChars](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[OrderID] [int] NOT NULL,
	[Value] [nvarchar](200) NULL,
 CONSTRAINT [PK_OrderChars] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderedProducts]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderedProducts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[OrderID] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Amount] [int] NOT NULL,
	[Weight] [decimal](18, 2) NULL,
 CONSTRAINT [PK_OrderedProducts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Orders]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ShopID] [int] NULL,
	[Status] [int] NOT NULL,
	[AddedByID] [int] NOT NULL,
	[ConsumerID] [int] NULL,
	[IsTemp] [bit] NOT NULL,
	[Warning] [nvarchar](max) NULL,
	[IsImportant] [bit] NOT NULL,
	[OrderNumber] [nvarchar](50) NULL,
	[DeliveryDate] [datetime] NULL,
	[DeliveryAddress] [nvarchar](max) NULL,
	[SkipMarkTo] [datetime] NULL,
	[IsPayed] [bit] NOT NULL,
	[AdressID] [int] NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PermissionGroups]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionGroups](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_PermissionGroups] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[GroupID] [int] NOT NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PermissionsForRoles]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionsForRoles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[PermissionID] [int] NOT NULL,
 CONSTRAINT [PK_PermissionsForRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProductChars]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductChars](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[OrderedProductID] [int] NOT NULL,
	[Value] [nvarchar](200) NULL,
 CONSTRAINT [PK_ProductChars] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Products]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[AddedDate] [datetime] NOT NULL,
	[Article] [nvarchar](500) NOT NULL,
	[OwnerID] [int] NOT NULL,
	[UnitCode] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ShopManagers]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShopManagers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ManagerID] [int] NOT NULL,
	[ShopID] [int] NULL,
 CONSTRAINT [PK_ShopManagers_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ShopProductChars]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShopProductChars](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[ShopID] [int] NULL,
	[DefValue] [nvarchar](500) NULL,
	[UserID] [int] NOT NULL,
	[IsMain] [bit] NOT NULL,
	[Type] [int] NOT NULL,
	[LoadInForm] [bit] NOT NULL,
 CONSTRAINT [PK_ShopProductChars] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Shops]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shops](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Owner] [int] NOT NULL,
	[TaxPlanID] [int] NULL,
	[Link] [nvarchar](200) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Shop] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ShopSettings]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShopSettings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ShopID] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[ItemKey] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ShopSettings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Stores]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stores](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[AdressID] [int] NULL,
	[ShopID] [int] NOT NULL,
 CONSTRAINT [PK_Stores] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TaxPlanes]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaxPlanes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[MonthCost] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_TaxPlanes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserAllowedRoles]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAllowedRoles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[AllowedRoleID] [int] NOT NULL,
 CONSTRAINT [PK_UserAllowedRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserPermissions]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPermissions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PermissionID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_UserPermissions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](200) NULL,
	[Name] [nvarchar](200) NOT NULL,
	[UserName] [nvarchar](500) NULL,
	[UserSurname] [nvarchar](500) NULL,
	[UserPatrinomic] [nvarchar](500) NULL,
	[LastVisitDate] [datetime] NULL,
	[Phone] [nvarchar](50) NULL,
	[IsPhoneConfirmed] [bit] NULL,
	[RegStep] [int] NULL,
	[Nick] [nvarchar](500) NULL,
	[DigitID] [nvarchar](9) NULL,
	[ConfirmKey] [uniqueidentifier] NULL,
	[IsLocked] [bit] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_Membership]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Membership](
	[UserId] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[ConfirmationToken] [nvarchar](128) NULL,
	[IsConfirmed] [bit] NULL,
	[LastPasswordFailureDate] [datetime] NULL,
	[PasswordFailuresSinceLastSuccess] [int] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordChangedDate] [datetime] NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[PasswordVerificationToken] [nvarchar](128) NULL,
	[PasswordVerificationTokenExpirationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_OAuthMembership]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_OAuthMembership](
	[Provider] [nvarchar](30) NOT NULL,
	[ProviderUserId] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Provider] ASC,
	[ProviderUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_Roles]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[RoleDescription] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_UsersInRoles]    Script Date: 31.10.2014 18:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_UsersInRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Consumers] ON 

INSERT [dbo].[Consumers] ([ID], [Name], [Surname], [Patrinomic], [Phone], [UserID], [AddPhone], [Email], [Sex]) VALUES (1, N'Антон', N'Петров', N'Сергеевич', N'+7(926) 265-75-08', NULL, N'', N'megaprogrammer7@gmail.com', 1)
INSERT [dbo].[Consumers] ([ID], [Name], [Surname], [Patrinomic], [Phone], [UserID], [AddPhone], [Email], [Sex]) VALUES (2, N'123', N'123', N'123', N'79262657508', NULL, N'', N'', 0)
INSERT [dbo].[Consumers] ([ID], [Name], [Surname], [Patrinomic], [Phone], [UserID], [AddPhone], [Email], [Sex]) VALUES (3, N'123', N'123', N'123', N'79262657508', NULL, N'', N'', 0)
INSERT [dbo].[Consumers] ([ID], [Name], [Surname], [Patrinomic], [Phone], [UserID], [AddPhone], [Email], [Sex]) VALUES (4, N'123', N'123', N'234', N'79262657508', NULL, N'', N'', 0)
INSERT [dbo].[Consumers] ([ID], [Name], [Surname], [Patrinomic], [Phone], [UserID], [AddPhone], [Email], [Sex]) VALUES (5, N'Иван', N'Иванов', N'Иванович', N'79262657508', NULL, N'', N'', 0)
INSERT [dbo].[Consumers] ([ID], [Name], [Surname], [Patrinomic], [Phone], [UserID], [AddPhone], [Email], [Sex]) VALUES (6, N'Мощность', N'Иванов', N'Сергеевич', N'79262657508', NULL, N'', N'', 0)
INSERT [dbo].[Consumers] ([ID], [Name], [Surname], [Patrinomic], [Phone], [UserID], [AddPhone], [Email], [Sex]) VALUES (7, N'Товар', N'Петров', N'Иванович', N'79262657508', NULL, N'', N'', 0)
INSERT [dbo].[Consumers] ([ID], [Name], [Surname], [Patrinomic], [Phone], [UserID], [AddPhone], [Email], [Sex]) VALUES (8, N'Мощность', N'Иванов', N'Иванович', N'79262657508', NULL, N'', N'', 0)
INSERT [dbo].[Consumers] ([ID], [Name], [Surname], [Patrinomic], [Phone], [UserID], [AddPhone], [Email], [Sex]) VALUES (9, N'admin', N'Иванов', N'Сергеевич', N'79262657508', NULL, N'', N'', 0)
INSERT [dbo].[Consumers] ([ID], [Name], [Surname], [Patrinomic], [Phone], [UserID], [AddPhone], [Email], [Sex]) VALUES (10, N'Антон', N'Ковецкий', N'Сергеевич', N'79262657508', NULL, N'', N'', 0)
INSERT [dbo].[Consumers] ([ID], [Name], [Surname], [Patrinomic], [Phone], [UserID], [AddPhone], [Email], [Sex]) VALUES (11, N'Иван', N'Иванов', N'Иванович', N'79262657508', NULL, N'', N'', 0)
SET IDENTITY_INSERT [dbo].[Consumers] OFF
SET IDENTITY_INSERT [dbo].[DeliveryAddress] ON 

INSERT [dbo].[DeliveryAddress] ([Region], [Town], [Street], [House], [Building], [Doorway], [Code], [Floor], [Flat], [Section], [ID], [PointID], [District]) VALUES (NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[DeliveryAddress] ([Region], [Town], [Street], [House], [Building], [Doorway], [Code], [Floor], [Flat], [Section], [ID], [PointID], [District]) VALUES (NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, NULL, NULL)
INSERT [dbo].[DeliveryAddress] ([Region], [Town], [Street], [House], [Building], [Doorway], [Code], [Floor], [Flat], [Section], [ID], [PointID], [District]) VALUES (N'Московская область', N'Тучково', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 3, NULL, NULL)
INSERT [dbo].[DeliveryAddress] ([Region], [Town], [Street], [House], [Building], [Doorway], [Code], [Floor], [Flat], [Section], [ID], [PointID], [District]) VALUES (NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL)
INSERT [dbo].[DeliveryAddress] ([Region], [Town], [Street], [House], [Building], [Doorway], [Code], [Floor], [Flat], [Section], [ID], [PointID], [District]) VALUES (N'Московская область', N'Москва', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5, NULL, NULL)
INSERT [dbo].[DeliveryAddress] ([Region], [Town], [Street], [House], [Building], [Doorway], [Code], [Floor], [Flat], [Section], [ID], [PointID], [District]) VALUES (NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 6, NULL, NULL)
INSERT [dbo].[DeliveryAddress] ([Region], [Town], [Street], [House], [Building], [Doorway], [Code], [Floor], [Flat], [Section], [ID], [PointID], [District]) VALUES (NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 7, NULL, NULL)
INSERT [dbo].[DeliveryAddress] ([Region], [Town], [Street], [House], [Building], [Doorway], [Code], [Floor], [Flat], [Section], [ID], [PointID], [District]) VALUES (NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 8, NULL, NULL)
INSERT [dbo].[DeliveryAddress] ([Region], [Town], [Street], [House], [Building], [Doorway], [Code], [Floor], [Flat], [Section], [ID], [PointID], [District]) VALUES (NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 9, NULL, NULL)
INSERT [dbo].[DeliveryAddress] ([Region], [Town], [Street], [House], [Building], [Doorway], [Code], [Floor], [Flat], [Section], [ID], [PointID], [District]) VALUES (N'Московская область', N'Тучково', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 10, NULL, NULL)
INSERT [dbo].[DeliveryAddress] ([Region], [Town], [Street], [House], [Building], [Doorway], [Code], [Floor], [Flat], [Section], [ID], [PointID], [District]) VALUES (N'Центральный федеральный округ', N'Москва', N'Ломоносовский проспект', N'35а', NULL, NULL, NULL, NULL, NULL, NULL, 11, 8, N'Москва')
INSERT [dbo].[DeliveryAddress] ([Region], [Town], [Street], [House], [Building], [Doorway], [Code], [Floor], [Flat], [Section], [ID], [PointID], [District]) VALUES (N'Центральный федеральный округ', N'Москва', N'Большая Екатерининская улица', N'27с2', NULL, NULL, NULL, NULL, NULL, NULL, 12, 5, N'Москва')
SET IDENTITY_INSERT [dbo].[DeliveryAddress] OFF
SET IDENTITY_INSERT [dbo].[LogAction] ON 

INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (1, N'Добавление пользователя')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (2, N'Редактирование данных пользователя')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (3, N'Блокировка пользователя')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (4, N'Отладочная информация')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (5, N'Добавление магазина')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (6, N'Редактирование данных магазина')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (7, N'Редактирование характеристик товаров магазина')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (8, N'Редактирование общих характеристик товаров')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (9, N'Разблокировка пользователя')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (10, N'Авторизация под чужим аккаунтом')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (11, N'Авторизация в системе')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (12, N'Удаление характеристик товаров магазина')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (13, N'Удаление общих характеристик товаров')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (14, N'Добавление характеристик товаров магазина')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (15, N'Добавление общих характеристик товаров')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (16, N'Удаление магазина')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (17, N'Добавление заказа')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (18, N'Регистрация в системе')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (19, N'Изменение пароля')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (20, N'Выход из системы')
INSERT [dbo].[LogAction] ([ID], [Name]) VALUES (21, N'Удаление пользователя')
SET IDENTITY_INSERT [dbo].[LogAction] OFF
SET IDENTITY_INSERT [dbo].[LogEntry] ON 

INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (3, CAST(0x0000A39600FE4B65 AS DateTime), 2, 2, N'm9e7loa5p7fv97e6ka5j6eqvdpsm93')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4, CAST(0x0000A39D00FE4B92 AS DateTime), 2, 3, N'xsoki8ioaydeoc18eknx3if8jitwvn')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (5, CAST(0x0000A39E00FE4B93 AS DateTime), 2, 2, N'dfxhiuncrg981b61nk7ofwwecg0zem')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (6, CAST(0x0000A39D00FE4B94 AS DateTime), 2, 2, N'1q09ejqkxb5m9777cs8bf77gji42tn')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (7, CAST(0x0000A39E00FE4B94 AS DateTime), 2, 1, N'nm6aaxm3unr3r9549tnldh7ujkqvfn')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (8, CAST(0x0000A39C00FE4B95 AS DateTime), 2, 1, N'dzym1wwe54hsg3p9d0tzzcs6fcrq6q')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (9, CAST(0x0000A39C00FE4B96 AS DateTime), 2, 1, N'whprai3cw9f80fdfuhietra17s7o45')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (10, CAST(0x0000A39700FE4B97 AS DateTime), 2, 2, N'gawzeyyuwg3qwk60zcwp79c57okiaf')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (11, CAST(0x0000A39700FE4B98 AS DateTime), 2, 3, N'10spie0wioicoj4nm7ps9i76yrrqef')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (12, CAST(0x0000A39A00FE4B9A AS DateTime), 2, 2, N'9bpvrghkbz7jrzgojww3pfiitmrrma')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (13, CAST(0x0000A39A00FE4B9A AS DateTime), 2, 1, N'413ct9v5q4g9bneya37hjgrtt625pp')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (14, CAST(0x0000A39E00FE4B9B AS DateTime), 2, 1, N'h4fpv4ua1qcky8767thi31in99in7q')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (15, CAST(0x0000A39A00FE4B9C AS DateTime), 2, 1, N'6fr67p259xgt46vkzvyv7fyglsfml7')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (16, CAST(0x0000A39600FE4B9D AS DateTime), 2, 1, N'9bejg1ziqc96kpj3exnbrkuqhbkf9g')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (17, CAST(0x0000A39A00FE4B9E AS DateTime), 2, 1, N'u5jssc4xwg4v5svw9c7njrjoho15gj')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (18, CAST(0x0000A39800FE4B9E AS DateTime), 2, 1, N's7bf5qa1tqebfykpfo5w8wi2lub3ay')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (19, CAST(0x0000A39A00FE4B9F AS DateTime), 2, 2, N'7u2yggqi15utc88i3cprdc2wp9tbe7')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (20, CAST(0x0000A39C00FE4BA0 AS DateTime), 2, 3, N'823lwm9zymu21pqdlt0x8qyus686mf')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (21, CAST(0x0000A39700FE4BA0 AS DateTime), 2, 3, N'eyka4nmg3qtqizuzjgpl6tmi39tpe2')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (22, CAST(0x0000A39900FE4BA1 AS DateTime), 2, 2, N'izo34g9y7kthw7csc8n6pjokc26uuw')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (23, CAST(0x0000A39600FE4BA1 AS DateTime), 2, 1, N'da8jbrv26qdmz4lwx0c5e6tza3q19e')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (24, CAST(0x0000A39700FE4BA2 AS DateTime), 2, 1, N'hs4mzshldkzy52k7gaxtjxn7h8870t')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (25, CAST(0x0000A39D00FE4BA3 AS DateTime), 2, 3, N'm3470zw4jrrt6uu8jjlzgark5x5qk9')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (26, CAST(0x0000A39B00FE4BA3 AS DateTime), 2, 2, N'r8c2yd7hdtas5t0me0oh8tabcj13u6')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (27, CAST(0x0000A39A00FE4BA4 AS DateTime), 2, 1, N'671yhs95b3ln0yfbwibkl3byx0peb3')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (28, CAST(0x0000A39B00FE4BA5 AS DateTime), 2, 1, N'91lxr24mqi547nbd4p7buzn2w31h2v')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (29, CAST(0x0000A39D00FE4BA6 AS DateTime), 2, 1, N'gedydhktdot2y6q9oc9lwbc9di0n1n')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (30, CAST(0x0000A39B00FE4BA6 AS DateTime), 2, 3, N'oxotaczn5dx56l4qdrspl8b423lugw')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (31, CAST(0x0000A39D00FE4BA7 AS DateTime), 2, 3, N'4ye1iwzbcmfje63seaa9uu9fljr6r5')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (32, CAST(0x0000A39700FE4BA8 AS DateTime), 2, 1, N'k717637gshcyssqv0tf5ljv9pyfy5t')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (33, CAST(0x0000A39B00FE4BA9 AS DateTime), 2, 1, N'g6y64vlb3thdibmttv5yp0n1bri8o8')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (34, CAST(0x0000A39800FE4BAA AS DateTime), 2, 3, N'w5imtjq4742s1irdcl59nmhfjxvddd')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (35, CAST(0x0000A39B00FE4BAA AS DateTime), 2, 3, N'iigcs00941qlyvzs04w231ban32vnd')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (36, CAST(0x0000A39C00FE4BAD AS DateTime), 2, 3, N'g7aj1elbkv4lwwgox2d8hkrg66ppx4')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (37, CAST(0x0000A39B00FE4BAD AS DateTime), 2, 3, N'av4qkgrljkh7wpdrmeyk0idwgtcupv')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (38, CAST(0x0000A39900FE4BAE AS DateTime), 2, 2, N'9db0rp3gbrpw3jjhtx3i61rsi89q4w')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (39, CAST(0x0000A39D00FE4BAF AS DateTime), 2, 3, N'61yjdc0wbh98tgsxav1srv98ljiigc')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (40, CAST(0x0000A39900FE4BB1 AS DateTime), 2, 2, N'w0q1kz3gx77psjy1fcg8u3wbsqxuuh')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (41, CAST(0x0000A39A00FE4BB2 AS DateTime), 2, 3, N'4tbsqrcasluknjf2kg23w3ks4e8u18')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (42, CAST(0x0000A39B00FE4BB2 AS DateTime), 2, 2, N'q9ky76boz5g98bn8av9dec119inbii')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (43, CAST(0x0000A39800FE4BB3 AS DateTime), 2, 2, N'zfuezhve5e4rmaktq0k6z81e7jooiz')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (44, CAST(0x0000A39700FE4BB3 AS DateTime), 2, 2, N'wxath99s0v808aeyyulhy2kqdg6gs6')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (45, CAST(0x0000A39E00FE4BB4 AS DateTime), 2, 1, N'7mup0qchrr54005x5ppneojtdjf13t')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (46, CAST(0x0000A39E00FE4BB5 AS DateTime), 2, 3, N'z2smfoy7i4q7wfqda9a2g8rj31bgbb')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (47, CAST(0x0000A39900FE4BB6 AS DateTime), 2, 3, N'rc5y8jhgri5axcwxt2qtiwdqa8ieci')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (48, CAST(0x0000A39E00FE4BB7 AS DateTime), 2, 1, N'qsfkas4qruby2gmr4en8kkfuzenz03')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (49, CAST(0x0000A39600FE4BB8 AS DateTime), 2, 1, N'lyk4wcm0xz2oleoumr9yvnm5dllw55')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (50, CAST(0x0000A39C00FE4BB8 AS DateTime), 2, 2, N'q3d4whusrmblhr9kqtc9u5dxberaqq')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (51, CAST(0x0000A39D00FE4BB9 AS DateTime), 2, 3, N'9exh1jx4wh79c1nlc16bsgtlpfthqj')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (52, CAST(0x0000A39600FE4BBB AS DateTime), 2, 1, N'qjb3tri6v8dcy3l9q5ypkjzslcrlqf')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (53, CAST(0x0000A39900FE4BBC AS DateTime), 2, 2, N'g6jspkrkizgk3gmlujqllbg6kihhzw')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (54, CAST(0x0000A39A00FE4BBD AS DateTime), 2, 2, N't0wsksk6dd7k3fp5p6r7otr4pc873a')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (55, CAST(0x0000A39B00FE4BBE AS DateTime), 2, 2, N'j0q3ju11zk774w25tcrt49q09r27kd')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (56, CAST(0x0000A39600FE4BBE AS DateTime), 2, 1, N'sm02omlu4q73lj0ta3dqisga9qtsst')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (57, CAST(0x0000A39A00FE4BBF AS DateTime), 2, 2, N'i4fivzg7rxoe1mepr9xhzck05qtyst')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (58, CAST(0x0000A39600FE4BC0 AS DateTime), 2, 1, N'allh4ixam6zl329mb40ivzm5kec56i')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (59, CAST(0x0000A39900FE4BC1 AS DateTime), 2, 3, N'uc3f0xk8bmvscyw3h6mnsjfbygxq6s')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (60, CAST(0x0000A39700FE4BC1 AS DateTime), 2, 1, N's1rqytmekr793utli8n0wq5axshk0u')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (61, CAST(0x0000A39600FE4BC2 AS DateTime), 2, 2, N'ir0a755ok830c674mc7x5461wgt5b1')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (62, CAST(0x0000A39900FE4BC2 AS DateTime), 2, 3, N'oprbtidc3zgon4yqlcyom5ddlzerb0')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (63, CAST(0x0000A39900FE4BC3 AS DateTime), 2, 2, N'udbnevb4yvu5oieri2jz3p3jbdqu3m')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (64, CAST(0x0000A39600FE4BC4 AS DateTime), 2, 1, N'nxt9sabq1upqa7p2kn88gcf59sb14h')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (65, CAST(0x0000A39600FE4BC4 AS DateTime), 2, 2, N'uriqp4y7rty9zd83no9gdrmaio00za')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (66, CAST(0x0000A39B00FE4BC5 AS DateTime), 2, 2, N'tcwjpycgp1hne65wj7df7kz347814x')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (67, CAST(0x0000A39700FE4BC6 AS DateTime), 2, 2, N'7efnhpeocsrb9zwbj4hrw6b9dtfhqb')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (68, CAST(0x0000A39900FE4BC7 AS DateTime), 2, 3, N't1fzqhfuso3ftnt4bwhzggj0259o9x')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (69, CAST(0x0000A39E00FE4BC8 AS DateTime), 2, 1, N'gzi8jwl1pny82wgqzb5ttcadhexk8q')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (70, CAST(0x0000A39B00FE4BC9 AS DateTime), 2, 3, N'q2yt4kkfbgathbx5pjoyr464o5q5y0')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (71, CAST(0x0000A39D00FE4BCA AS DateTime), 2, 3, N'23i0ymleqphd1hw6grvo2m3lvu4zuf')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (72, CAST(0x0000A39A00FE4BCA AS DateTime), 2, 1, N'ncrdtx72y972mt279ko654vfzx44il')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (73, CAST(0x0000A39B00FE4BCB AS DateTime), 2, 1, N'86poqidjksqsf1y2rxq9ra6jhbewt2')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (74, CAST(0x0000A39B00FE4BCC AS DateTime), 2, 2, N'pwtg3ehr74lce20yy2dluakx067e3h')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (75, CAST(0x0000A39C00FE4BCD AS DateTime), 2, 2, N'8d3i7hycgz2mot5ebgxqpyiecvgx62')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (76, CAST(0x0000A39B00FE4BCD AS DateTime), 2, 3, N'f440zhqh405v7x961t3njzs9uo6c10')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (77, CAST(0x0000A39700FE4BCE AS DateTime), 2, 2, N'cc7kp4euqgmptn6em9vczz6fcu6aae')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (78, CAST(0x0000A39900FE4BCF AS DateTime), 2, 1, N'trjvqogxe8x3c2g7hspr5tec5r8vx3')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (79, CAST(0x0000A39A00FE4BCF AS DateTime), 2, 1, N'0iv14boxtxyq2239eytiony0d6pygl')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (80, CAST(0x0000A39E00FE4BD0 AS DateTime), 2, 1, N'pl3t6e363sy7vna3oyhza1ndkvoxlp')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (81, CAST(0x0000A39A00FE4BD1 AS DateTime), 2, 1, N'yn1gz93zvmewpq4pwmai400nyq9bq5')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (82, CAST(0x0000A39B00FE4BD2 AS DateTime), 2, 2, N'bq98mt4evh4nnqntqy1zey3xmdi90r')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (83, CAST(0x0000A39800FE4BD2 AS DateTime), 2, 3, N'2zlyhw32iwmmwc072fks4lzcmbl8au')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (84, CAST(0x0000A39900FE4BD3 AS DateTime), 2, 1, N'8eln5pbuvyhich0adb7k7hkg08b5au')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (85, CAST(0x0000A39D00FE4BD3 AS DateTime), 2, 2, N'hiehqd1c3pw64g05bbvzntvru4nxd5')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (86, CAST(0x0000A39B00FE4BD4 AS DateTime), 2, 1, N'6t1t2ijz9uhikecj86uyys0onnnlq3')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (87, CAST(0x0000A39E00FE4BD5 AS DateTime), 2, 3, N'5w0af73sswpscd6gs3p7al1dtc6fvx')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (88, CAST(0x0000A39800FE4BD6 AS DateTime), 2, 1, N'vjcnd131326227b9g7tmesnkumh30o')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (89, CAST(0x0000A39E00FE4BD7 AS DateTime), 2, 3, N'mjxt0j2q0o48fyam1wyn3nr2wssrk0')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (90, CAST(0x0000A39600FE4BD8 AS DateTime), 2, 2, N'5agj9i8x52lqoe4wbv14p9rzsux7qa')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (91, CAST(0x0000A39700FE4BD9 AS DateTime), 2, 1, N'op24ircin0i111ehnwfpyvpwihrel1')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (92, CAST(0x0000A39700FE4BD9 AS DateTime), 2, 3, N'vl2zc0ekijgprqxrggkuuh1sz6l10x')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (93, CAST(0x0000A39E00FE4BDA AS DateTime), 2, 2, N'tn5gzf4niv6ff04sj0y4gc0ej9z6fd')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (94, CAST(0x0000A39A00FE4BDB AS DateTime), 2, 2, N'luv4lhr0bwbzbmdz1sh0zt5gnrxpgy')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (95, CAST(0x0000A39800FE4BDC AS DateTime), 2, 1, N'bltk40728ysk103wwz5mvjlsoehtj2')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (96, CAST(0x0000A39B00FE4BDC AS DateTime), 2, 2, N'o6npssu0les00lfmo0tgsl9sz1wqjd')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (97, CAST(0x0000A39A00FE4BDD AS DateTime), 2, 1, N'dfvudd33440grks3zv5j93l3wvprw4')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (98, CAST(0x0000A39A00FE4BDE AS DateTime), 2, 1, N'wmmlqk8awpcnls0e9tro7zwausxhnc')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (99, CAST(0x0000A39700FE4BDF AS DateTime), 2, 3, N'osk4x7cemvmkrxi4ubi3ds0wqcqynq')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (100, CAST(0x0000A39B00FE4BDF AS DateTime), 2, 2, N'hczs7vg1gc2ccbjw9tncu4mvsoelge')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (101, CAST(0x0000A39C00FE4BE0 AS DateTime), 2, 3, N'822fxpwzugm0pnketuot3aqdq3zjhv')
GO
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (102, CAST(0x0000A39B00FE4BE1 AS DateTime), 2, 1, N'l63a93zkwnidil643zrmpwsycoouqc')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (103, CAST(0x0000A39D014E0742 AS DateTime), 2, 3, N'dmrngfhd0yhc5z6bj7om1q4frse0it')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (104, CAST(0x0000A39E014E075A AS DateTime), 2, 3, N'95fqpodielvupmeohthvj9qmicwo0g')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (105, CAST(0x0000A39D014E075B AS DateTime), 2, 1, N'0s20kgmuu57fvaq4dqjpb250ft8cso')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (106, CAST(0x0000A39A014E075B AS DateTime), 2, 3, N'rntwkd417n1d1aq8msbax5wyn2kq79')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (107, CAST(0x0000A396014E075B AS DateTime), 2, 1, N'n9o7gtkzmhhn1sz420qzd70lrv9htd')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (108, CAST(0x0000A39D014E075B AS DateTime), 2, 3, N'kvprlirs8bofn8wv3fn7j2rg92glk2')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (109, CAST(0x0000A397014E075C AS DateTime), 2, 1, N'oj2527jj8xr2jlyjyeedpmei39ii26')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (110, CAST(0x0000A398014E075C AS DateTime), 2, 1, N'4fr45qsvw4ylapwn4xm6otj24k2sp0')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (111, CAST(0x0000A39A014E075D AS DateTime), 2, 1, N'olx4uud7liglns7ddue5j2hbwpzlo0')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (112, CAST(0x0000A397014E075E AS DateTime), 2, 3, N'mwxprstajfk5cxebdofvmcil4m84us')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (113, CAST(0x0000A39A014E075E AS DateTime), 2, 2, N'ckyrvilk04l4hcyyan8zka9aebl5ko')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (114, CAST(0x0000A399014E075E AS DateTime), 2, 3, N'wzssyngu2uvird3ro9u7jj02aocjxg')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (115, CAST(0x0000A396014E075E AS DateTime), 2, 1, N'ot8ik5zm7dxpfrj07thau77dyphaaw')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (116, CAST(0x0000A39E014E075F AS DateTime), 2, 2, N'tjty4l7i9806lbnj5cc4zqhglvjuop')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (117, CAST(0x0000A398014E075F AS DateTime), 2, 1, N'gbvvxkvmly0l71rn2y4yktm32dc92d')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (118, CAST(0x0000A39D014E075F AS DateTime), 2, 2, N'wpqafslkgpi0pv8j4vsqkdpmorsk8m')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (119, CAST(0x0000A39A014E075F AS DateTime), 2, 1, N'5gphbg79ckhuale74t46l30iz3ln00')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (120, CAST(0x0000A399014E0760 AS DateTime), 2, 3, N'5av95r8noiezvvb3dkgfhdn1g6i7vj')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (121, CAST(0x0000A398014E0760 AS DateTime), 2, 1, N'8no0p41ckhi2p0n4si9fkxcsntsy91')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (122, CAST(0x0000A39D014E0760 AS DateTime), 2, 1, N'82r76ad9i3u0vkcgjdfd9urabwdwc3')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (123, CAST(0x0000A396014E0761 AS DateTime), 2, 2, N'odtm8fyh40wg2bw2hcfvmfvebgw9xh')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (124, CAST(0x0000A39E014E0761 AS DateTime), 2, 2, N'zbbirp74fx88xa7zzbezn8w9ieario')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (125, CAST(0x0000A39B014E0761 AS DateTime), 2, 2, N'3ggr0ebr5bgo6mg19wam2xsre10cfg')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (126, CAST(0x0000A39B014E0761 AS DateTime), 2, 3, N'dk5hugzkznucr83vy9imdxl6oocyez')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (127, CAST(0x0000A399014E0761 AS DateTime), 2, 3, N'ppalhz6swpme3f06y4e5xydfzys541')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (128, CAST(0x0000A397014E0762 AS DateTime), 2, 1, N'b5m2xhey2qqob638o6zal7rqcvli71')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (129, CAST(0x0000A39C014E0762 AS DateTime), 2, 3, N'im3xtx8hpf278o4tvjgx2wf4evq2vw')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (130, CAST(0x0000A39C014E0762 AS DateTime), 2, 1, N'1v5o25fqxhl55jinqsmnkqsm7zjx76')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (131, CAST(0x0000A39B014E0763 AS DateTime), 2, 3, N'o05djfqq4pzka9yud2b294gb2ih3mh')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (132, CAST(0x0000A39A014E0763 AS DateTime), 2, 3, N'vsh4d44a1wiuq7bx0ok4ujjkkua0xm')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (133, CAST(0x0000A397014E0763 AS DateTime), 2, 2, N'68pisug8y00dllkv6k3xxewb8xkju3')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (134, CAST(0x0000A399014E0764 AS DateTime), 2, 2, N'svoiit567b6ti5yyaftdj6psbdav9x')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (135, CAST(0x0000A399014E0764 AS DateTime), 2, 3, N'nllqx2cdfxqm72t3ym9e90vl5v65j9')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (136, CAST(0x0000A39E014E0764 AS DateTime), 2, 1, N'wb6wk8082gxmg3k9j7wnn0knqzk2v7')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (137, CAST(0x0000A39D014E0764 AS DateTime), 2, 1, N'1fpvwwiql1ct2952axlcc8mwri6kny')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (138, CAST(0x0000A399014E0764 AS DateTime), 2, 3, N'lrl0dpcmo62mnrlhuhrylitstt4dqy')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (139, CAST(0x0000A396014E0765 AS DateTime), 2, 2, N'og7an5olkyn66z8zxoq56d93ys6kky')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (140, CAST(0x0000A39E014E0765 AS DateTime), 2, 3, N'4fx7jatps5dnummtk4g27wga74bvy4')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (141, CAST(0x0000A396014E0765 AS DateTime), 2, 2, N'2x1q21gnu1d00m8c5dzfiti7ymzfzy')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (142, CAST(0x0000A39C014E0766 AS DateTime), 2, 3, N'4rkxkvqjot76jfalvzpk5l38eusf20')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (143, CAST(0x0000A39B014E0766 AS DateTime), 2, 3, N'aow81br9mgrpmbojcekz86ijnciq3b')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (144, CAST(0x0000A39A014E0766 AS DateTime), 2, 2, N'5awuj1jk8fyoudm2593ngnn35ckps0')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (145, CAST(0x0000A397014E0767 AS DateTime), 2, 1, N'z3kbftzau0rs8uo9pflu7t7hmso6gu')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (146, CAST(0x0000A39C014E0767 AS DateTime), 2, 1, N'9mpqtanc9ttgwvvboazcfc35w3om5s')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (147, CAST(0x0000A39D014E0767 AS DateTime), 2, 3, N'8jjl4xges9kezrlrxci5uesxjkuqm1')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (148, CAST(0x0000A397014E0768 AS DateTime), 2, 2, N'zt7cb97qukk6rp55hwosan4tqm2k2p')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (149, CAST(0x0000A39D014E0768 AS DateTime), 2, 3, N'23e6j8gn5sbaum5uep8pce6cpdmp7n')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (150, CAST(0x0000A39B014E0769 AS DateTime), 2, 1, N'st4nazl9g1micuz4g85ch3enx2h5mq')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (151, CAST(0x0000A399014E0769 AS DateTime), 2, 2, N'46i0ihje4y8brozuei8zhanofgq7ig')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (152, CAST(0x0000A39B014E076A AS DateTime), 2, 3, N'l4d5cylzoq8iu2v92mr4ig0ujtl1rc')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (153, CAST(0x0000A39E014E076A AS DateTime), 2, 3, N'biqnbuhfw9bq14tclh4voj744imjdj')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (154, CAST(0x0000A398014E076A AS DateTime), 2, 2, N'uk7bc8x84c7959sez9g970zrblis8y')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (155, CAST(0x0000A396014E076B AS DateTime), 2, 2, N'1zwhxizcjvrcbxzvq7j43y4tlfzqq5')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (156, CAST(0x0000A396014E076B AS DateTime), 2, 1, N'au85cbgh94qb9mfeln10fx92u2vii9')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (157, CAST(0x0000A39B014E076C AS DateTime), 2, 2, N'vl2gptier2vy8sjyzl523zqdl5bafy')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (158, CAST(0x0000A39E014E076C AS DateTime), 2, 1, N'hcgokypr5n9c2au1ixiw3k9vupvji7')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (159, CAST(0x0000A39E014E076C AS DateTime), 2, 3, N'3vhvcve0wdxgqeaab9rif2wx2ktv35')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (160, CAST(0x0000A398014E076D AS DateTime), 2, 3, N'ng87tyonji0qnt5gel78ggd1yjts1i')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (161, CAST(0x0000A39E014E076D AS DateTime), 2, 3, N'4zs953hcl3zx16a9edgiy0g637895v')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (162, CAST(0x0000A39B014E076D AS DateTime), 2, 1, N'k9ln0waa4undhgzs9jen1fs3im5xv7')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (163, CAST(0x0000A397014E076D AS DateTime), 2, 3, N'hzcrkn9h26e3d3pm7f0mhxy5hi4eqd')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (164, CAST(0x0000A399014E076E AS DateTime), 2, 3, N'nxvi378mawlv1oqfxipf6l7k0791iv')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (165, CAST(0x0000A39C014E076E AS DateTime), 2, 3, N'qhgf86j06eclc23trpg7n0q2qbhwx6')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (166, CAST(0x0000A396014E076E AS DateTime), 2, 2, N'5v5k8a7qzp8s9m7y4gatej2prepwon')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (167, CAST(0x0000A396014E076E AS DateTime), 2, 1, N'axggxjlgiq7xrys33jshwa0ltftula')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (168, CAST(0x0000A396014E076F AS DateTime), 2, 2, N'n8hb6q1kvtbnb4ysllt4ul30xunhqq')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (169, CAST(0x0000A39D014E076F AS DateTime), 2, 1, N'ovajghxdi7l3zcaoh090hxh24geb3j')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (170, CAST(0x0000A39D014E076F AS DateTime), 2, 1, N'y3zgufc3oflnz00kky0phvqrd7ez2l')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (171, CAST(0x0000A398014E0770 AS DateTime), 2, 3, N'ch4vctt1o5c1bu34gtr4j4m3d83n0c')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (172, CAST(0x0000A396014E0770 AS DateTime), 2, 1, N'5mahv47q5y1cjofi4aj8t5f7u1i4q6')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (173, CAST(0x0000A396014E0770 AS DateTime), 2, 2, N'49vpjl8bl1zk3ajotkiqss8z62mul3')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (174, CAST(0x0000A39E014E0770 AS DateTime), 2, 3, N'xp89tqyj18ms6v4khub6oajb0vihye')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (175, CAST(0x0000A39C014E0771 AS DateTime), 2, 2, N'20ne1au2lyok5gtby2cqssan5w8x7h')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (176, CAST(0x0000A39B014E0771 AS DateTime), 2, 2, N'i4q57qi708lplz6k27iv1yq9q0851x')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (177, CAST(0x0000A39D014E0771 AS DateTime), 2, 1, N'd110qn48j4qj7k15pov0zpmjege6hd')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (178, CAST(0x0000A397014E0772 AS DateTime), 2, 1, N'dhbky5te2acw7616y2fc2u4ofhm98x')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (179, CAST(0x0000A39C014E0772 AS DateTime), 2, 1, N'yo6q8h7kugxpcpc7f88fxt0idn5gjk')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (180, CAST(0x0000A397014E0772 AS DateTime), 2, 1, N'j6xj0bxguvhlwebp5et0ob85nndlcr')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (181, CAST(0x0000A396014E0772 AS DateTime), 2, 1, N'br66rs74ark2bfmybfb4q5h7yswcxc')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (182, CAST(0x0000A397014E0773 AS DateTime), 2, 1, N't7a485yc6ppxq5t81f9xfwj6ay8zwv')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (183, CAST(0x0000A397014E0773 AS DateTime), 2, 3, N'nh935hp6hyy1fkhsyobi26nwo3uaxy')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (184, CAST(0x0000A39A014E0773 AS DateTime), 2, 1, N'jr99wq2jq3ghyiqqgf08jnvgtcf1ne')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (185, CAST(0x0000A398014E0774 AS DateTime), 2, 2, N'hmioa7w8lrszq6y6d3uhx8svvsfka1')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (186, CAST(0x0000A39E014E0774 AS DateTime), 2, 1, N'uyzcz2g9jsmrvwqnd9ojkrk9unshrc')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (187, CAST(0x0000A39E014E0774 AS DateTime), 2, 2, N'vg4s6m6ankx5l00y2s61ddr2ephcay')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (188, CAST(0x0000A39A014E0775 AS DateTime), 2, 1, N'u3wesygr2dwzteoumtfz6w6ip1dh4t')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (189, CAST(0x0000A396014E0775 AS DateTime), 2, 3, N'3qai3l63ceyzhs8z1npgii0yxepqkz')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (190, CAST(0x0000A39B014E0775 AS DateTime), 2, 1, N'd6w246q1nbmsy60xsefh4k6kqblpwf')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (191, CAST(0x0000A39A014E0776 AS DateTime), 2, 2, N'rplav6uvxos6v80qdsx5j64t0ccsk0')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (192, CAST(0x0000A397014E0776 AS DateTime), 2, 1, N's908kq1mkkkfrzohbuc4hdtlksajt1')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (193, CAST(0x0000A398014E0776 AS DateTime), 2, 3, N'jkem7r5mqblzmp10nuhqo0pegfnsgh')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (194, CAST(0x0000A39D014E0777 AS DateTime), 2, 2, N'9nv6l2ajppd6suyvqawykk7juz6r3d')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (195, CAST(0x0000A397014E0778 AS DateTime), 2, 1, N'i2qytvelkx0zmjmlgs16luyogbblwi')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (196, CAST(0x0000A39A014E0778 AS DateTime), 2, 2, N'i4np402vvadmj8xbdkabbzno31ihjs')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (197, CAST(0x0000A39C014E0778 AS DateTime), 2, 1, N'hogivwhgp6bagb57e08b741jgzm3ik')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (198, CAST(0x0000A398014E0779 AS DateTime), 2, 1, N'lb5urffg2ul4oc8rva2jkducnx1wae')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (199, CAST(0x0000A39E014E0779 AS DateTime), 2, 1, N'v8p46agzls9djwu3ru77iri8gt7udi')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (200, CAST(0x0000A397014E0779 AS DateTime), 2, 2, N'wg8fn3llu2yr3htq3x5f7qm3qgnc3m')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (201, CAST(0x0000A39B014E0779 AS DateTime), 2, 1, N'llh2oegn9mdftq5z0grgdqg5u4o7f9')
GO
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (202, CAST(0x0000A39A014E077A AS DateTime), 2, 1, N'gw6hzq2ohh6lc7a0bocmv6ou5b8jp9')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (203, CAST(0x0000A3A400B356BD AS DateTime), 5, 2, N'Редактирование пользователя, Email пользователя - tester@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (204, CAST(0x0000A3A400B364B9 AS DateTime), 2, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (205, CAST(0x0000A3A8013E4DAC AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (206, CAST(0x0000A3A8013F9F27 AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (207, CAST(0x0000A3A8013FA1CF AS DateTime), 2, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (208, CAST(0x0000A3A8013FD12E AS DateTime), 2, 1, N'Создание пользователя, Email пользователя - oper@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (209, CAST(0x0000A3A801405C6C AS DateTime), 2, 2, N'Редактирование пользователя, Email пользователя - oper@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (210, CAST(0x0000A3A801408566 AS DateTime), 2, 19, N'Изменение пароля для пользователя oper@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (211, CAST(0x0000A3A801408906 AS DateTime), 2, 2, N'Редактирование пользователя, Email пользователя - oper@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (212, CAST(0x0000A3A80140A5E6 AS DateTime), 2, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (213, CAST(0x0000A3A80140ACCB AS DateTime), 9, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (214, CAST(0x0000A3A9008A9EA5 AS DateTime), 9, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (215, CAST(0x0000A3A9008AA330 AS DateTime), 2, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (216, CAST(0x0000A3A900939BF0 AS DateTime), 2, 2, N'Редактирование пользователя, Email пользователя - oper@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (217, CAST(0x0000A3A9009AD9DB AS DateTime), 2, 2, N'Редактирование пользователя, Email пользователя - admin123@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (218, CAST(0x0000A3A9009B198C AS DateTime), 2, 2, N'Редактирование пользователя, Email пользователя - admin123@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (219, CAST(0x0000A3A9009B1D21 AS DateTime), 2, 2, N'Редактирование пользователя, Email пользователя - admin123@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (220, CAST(0x0000A3A9009B754B AS DateTime), 2, 2, N'Редактирование пользователя, Email пользователя - admin123@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (221, CAST(0x0000A3A9009BAA22 AS DateTime), 2, 2, N'Редактирование пользователя, Email пользователя - admin123@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (222, CAST(0x0000A3A9009BB82E AS DateTime), 2, 2, N'Редактирование пользователя, Email пользователя - admin123@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (223, CAST(0x0000A3A9009BC32B AS DateTime), 2, 2, N'Редактирование пользователя, Email пользователя - oper@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (224, CAST(0x0000A3A9009BC7A1 AS DateTime), 2, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (225, CAST(0x0000A3A9009BD50D AS DateTime), 2, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (226, CAST(0x0000A3A9009BDE3D AS DateTime), 2, 10, N'Вход под другим логином, Email пользователя - oper@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (227, CAST(0x0000A3B200A0CD8E AS DateTime), 2, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (228, CAST(0x0000A3B2013EB226 AS DateTime), 2, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (229, CAST(0x0000A3B2013EC048 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (230, CAST(0x0000A3B20142F3A3 AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (231, CAST(0x0000A3B20142F684 AS DateTime), 2, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (232, CAST(0x0000A3B201431A7D AS DateTime), 2, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (233, CAST(0x0000A3B201432B5B AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (234, CAST(0x0000A3B500FC8C5E AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (235, CAST(0x0000A3B50101E870 AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (236, CAST(0x0000A3B50101EB1A AS DateTime), 2, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (237, CAST(0x0000A3B501027172 AS DateTime), 2, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (238, CAST(0x0000A3B501027DEF AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (239, CAST(0x0000A3B501028C76 AS DateTime), 3, 21, N'Удаление пользователя, Email пользователя - tester@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (240, CAST(0x0000A3B50103D1BF AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (241, CAST(0x0000A3B50103D607 AS DateTime), 2, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (242, CAST(0x0000A3B50104DC56 AS DateTime), 2, 3, N'Блокирование пользователяEmail пользователя - photon-cms@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (243, CAST(0x0000A3B50104F9FA AS DateTime), 2, 9, N'Разблокирование пользователяEmail пользователя - photon-cms@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (244, CAST(0x0000A3B5010506F0 AS DateTime), 2, 3, N'Блокирование пользователяEmail пользователя - photon-cms@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (245, CAST(0x0000A3B50105ED80 AS DateTime), 2, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (246, CAST(0x0000A3B50105FE45 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (247, CAST(0x0000A3B700BE1359 AS DateTime), 3, 7, N'Редактирование характеристики Количество в упаковке для магазина Магазин Игрушек')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (248, CAST(0x0000A3B700BF1E6D AS DateTime), 3, 8, N'Редактирование характеристики В наличии')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (249, CAST(0x0000A3B700BF277B AS DateTime), 3, 8, N'Редактирование характеристики В наличии')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (250, CAST(0x0000A3B700BF32DE AS DateTime), 3, 8, N'Редактирование характеристики В наличии')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (251, CAST(0x0000A3B700BF40B4 AS DateTime), 3, 7, N'Редактирование характеристики Количество в упаковке для магазина Магазин Игрушек')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (252, CAST(0x0000A3B700BF4D43 AS DateTime), 3, 7, N'Редактирование характеристики Размер для магазина Мой магазин')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (253, CAST(0x0000A3B700C6DF19 AS DateTime), 3, 17, N'Добавление в обработку заказа #00000014')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (254, CAST(0x0000A3B700C72233 AS DateTime), 3, 17, N'Добавление в обработку заказа #00000006')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (255, CAST(0x0000A3B700D0629D AS DateTime), 3, 17, N'Добавление в обработку заказа #00000001')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (256, CAST(0x0000A3B700EDAD57 AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (257, CAST(0x0000A3B700EDC40E AS DateTime), 2, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (258, CAST(0x0000A3B700EE1176 AS DateTime), 2, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (259, CAST(0x0000A3B700EE2474 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (260, CAST(0x0000A3B700EE375E AS DateTime), 3, 10, N'Вход под другим логином, Email пользователя - admin123@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (261, CAST(0x0000A3B700EEA7F5 AS DateTime), 5, 2, N'Редактирование пользователя, Email пользователя - admin123@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (262, CAST(0x0000A3B700F0D8FF AS DateTime), 5, 2, N'Редактирование пользователя, Email пользователя - admin123@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (263, CAST(0x0000A3B700F0EDB7 AS DateTime), 5, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (264, CAST(0x0000A3B700F0F0A9 AS DateTime), 2, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (265, CAST(0x0000A3B700F0F70F AS DateTime), 2, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (266, CAST(0x0000A3B700F1218F AS DateTime), 2, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (267, CAST(0x0000A3B700F12BF7 AS DateTime), 2, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (268, CAST(0x0000A3B700F13BC9 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (269, CAST(0x0000A3B700F1616D AS DateTime), 3, 2, N'Редактирование пользователя, Email пользователя - admin123@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (270, CAST(0x0000A3B700F8FDD4 AS DateTime), 3, 17, N'Добавление в обработку заказа #00000007')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (271, CAST(0x0000A3B700F95BFE AS DateTime), 3, 17, N'Добавление в обработку заказа #00000003')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (272, CAST(0x0000A3B700F986C2 AS DateTime), 3, 17, N'Добавление в обработку заказа #00000004')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (273, CAST(0x0000A3B700FCE893 AS DateTime), 3, 8, N'Редактирование характеристики В наличии')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (274, CAST(0x0000A3B700FCF0AB AS DateTime), 3, 8, N'Редактирование характеристики В наличии')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (275, CAST(0x0000A3B700FD6D58 AS DateTime), 3, 8, N'Редактирование характеристики В наличии')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (276, CAST(0x0000A3B8012367B5 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (277, CAST(0x0000A3B9013E3D59 AS DateTime), 2, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (278, CAST(0x0000A3B9013E41C7 AS DateTime), 2, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (279, CAST(0x0000A3B9013E5244 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (280, CAST(0x0000A3BD00968DE0 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (281, CAST(0x0000A3BE009F7D37 AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (282, CAST(0x0000A3BE009F82A4 AS DateTime), 2, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (283, CAST(0x0000A3BE00A043DF AS DateTime), 2, 10, N'Вход под другим логином, Email пользователя - oper@mail.ru')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (284, CAST(0x0000A3BE00A058A0 AS DateTime), 9, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (285, CAST(0x0000A3BE00A05CF1 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (286, CAST(0x0000A3BE00AC3B7A AS DateTime), 3, 8, N'Редактирование характеристики В наличии')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (287, CAST(0x0000A3BE00D5A748 AS DateTime), 3, 17, N'Добавление в обработку заказа #00000019')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (288, CAST(0x0000A3BE00D999F5 AS DateTime), 3, 8, N'Редактирование характеристики В наличии')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (289, CAST(0x0000A3BE00E013A7 AS DateTime), 3, 17, N'Добавление в обработку заказа #00000005')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (290, CAST(0x0000A3BF00A788CA AS DateTime), 3, 15, N'Добавление характеристики Этаж')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1276, CAST(0x0000A3BF00B03631 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1277, CAST(0x0000A3BF00B8A10F AS DateTime), 3, 14, N'Добавление характеристики Номер 1С для магазина Магазин Игрушек')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1278, CAST(0x0000A3BF00B8B7C6 AS DateTime), 3, 7, N'Редактирование характеристики Номер 1С для магазина Магазин Игрушек')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1279, CAST(0x0000A3BF00C09F93 AS DateTime), 3, 17, N'Добавление в обработку заказа #00000014')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1280, CAST(0x0000A3BF00C19D59 AS DateTime), 3, 17, N'Добавление в обработку заказа #00000014')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1281, CAST(0x0000A3C000ED927F AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1282, CAST(0x0000A3C000ED9DB9 AS DateTime), 9, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1283, CAST(0x0000A3C0010E2ECA AS DateTime), 9, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1284, CAST(0x0000A3C0010E346D AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1285, CAST(0x0000A3C0010ECF10 AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1286, CAST(0x0000A3C0010ED4DC AS DateTime), 9, 11, N'Авторизация в системе')
GO
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1287, CAST(0x0000A3C0013E088A AS DateTime), 9, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1288, CAST(0x0000A3C0013E0D0C AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (1289, CAST(0x0000A3C30133AD04 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (2276, CAST(0x0000A3C40132CD0C AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (2277, CAST(0x0000A3C50114B9A4 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (2278, CAST(0x0000A3C50121B1B2 AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (2279, CAST(0x0000A3C50121BD91 AS DateTime), 9, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (2280, CAST(0x0000A3C5012849D4 AS DateTime), 9, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (2281, CAST(0x0000A3C501284EB9 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (2282, CAST(0x0000A3C5012A974C AS DateTime), 3, 17, N'Добавление в обработку заказа #00000021')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (3276, CAST(0x0000A3C601176D61 AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (3277, CAST(0x0000A3C6011772F2 AS DateTime), 9, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (3278, CAST(0x0000A3C6011B0104 AS DateTime), 9, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (3279, CAST(0x0000A3C6011B052E AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4276, CAST(0x0000A3C7011E5070 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4277, CAST(0x0000A3C7013C1769 AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4278, CAST(0x0000A3C7013C2010 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4279, CAST(0x0000A3C7013C5B24 AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4280, CAST(0x0000A3C7013C6092 AS DateTime), 9, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4281, CAST(0x0000A3C7013C8740 AS DateTime), 9, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4282, CAST(0x0000A3C7013C8B69 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4283, CAST(0x0000A3C7018957BB AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4284, CAST(0x0000A3C9008F9E4F AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4285, CAST(0x0000A3C900998C98 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4286, CAST(0x0000A3C900A01B2F AS DateTime), 3, 12, N'Удаление характеристики Test: для магазина Еще один')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4287, CAST(0x0000A3CC00D5996D AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4288, CAST(0x0000A3CC011B07DA AS DateTime), 3, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4289, CAST(0x0000A3CC011B0DD9 AS DateTime), 9, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4290, CAST(0x0000A3CC011B3474 AS DateTime), 9, 20, N'Выход из системы')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4291, CAST(0x0000A3CC011B37FF AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4292, CAST(0x0000A3CE01107230 AS DateTime), 3, 17, N'Добавление в обработку заказа #00000021')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4293, CAST(0x0000A3D1010E2452 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4294, CAST(0x0000A3D201330ADA AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4295, CAST(0x0000A3D4009ECEA5 AS DateTime), 3, 11, N'Авторизация в системе')
INSERT [dbo].[LogEntry] ([ID], [ActionDate], [UserID], [ActionID], [Description]) VALUES (4296, CAST(0x0000A3D4013DDB93 AS DateTime), 3, 11, N'Авторизация в системе')
SET IDENTITY_INSERT [dbo].[LogEntry] OFF
SET IDENTITY_INSERT [dbo].[LoginHistory] ON 

INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (1, 2, CAST(0x0000A394011C656E AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (2, 3, CAST(0x0000A39900B259B2 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (3, 3, CAST(0x0000A39900B607B9 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4, 2, CAST(0x0000A39900B617D5 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (5, 2, CAST(0x0000A399013CC5C5 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (6, 3, CAST(0x0000A399013E1300 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (7, 2, CAST(0x0000A399013E5AC8 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (8, 2, CAST(0x0000A399014801CC AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (10, 2, CAST(0x0000A39B00BDA865 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (11, 3, CAST(0x0000A39B00BDC93D AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (12, 2, CAST(0x0000A39B015DC850 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (13, 3, CAST(0x0000A39B015DF2EB AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (14, 2, CAST(0x0000A39D00A84B19 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (15, 3, CAST(0x0000A39D00A8621C AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (16, 3, CAST(0x0000A39D00B8F8A1 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (17, 7, CAST(0x0000A39D010D0A63 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (18, 3, CAST(0x0000A39D01206403 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (19, 5, CAST(0x0000A3A100F43AC9 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (20, 5, CAST(0x0000A3A100F466B8 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (21, 5, CAST(0x0000A3A100F4A099 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (22, 2, CAST(0x0000A3A100F4ABBB AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (23, 3, CAST(0x0000A3A100F4FFB9 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (24, 2, CAST(0x0000A3A100F6A66B AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (25, 2, CAST(0x0000A3A100F75590 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (26, 2, CAST(0x0000A3A100FE64E4 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (27, 2, CAST(0x0000A3A300B38E90 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (28, 3, CAST(0x0000A3A300B3A5FD AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (29, 3, CAST(0x0000A3A300BBF4E8 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (30, 2, CAST(0x0000A3A400B364B5 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (31, 3, CAST(0x0000A3A8013E4DA3 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (32, 2, CAST(0x0000A3A8013FA1CF AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (33, 9, CAST(0x0000A3A80140ACC9 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (34, 2, CAST(0x0000A3A9008AA32C AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (35, 2, CAST(0x0000A3A9009BD50B AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (36, 2, CAST(0x0000A3B200A0CD87 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (37, 3, CAST(0x0000A3B2013EC045 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (38, 2, CAST(0x0000A3B20142F683 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (39, 3, CAST(0x0000A3B201432B5A AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (40, 3, CAST(0x0000A3B500FC8C57 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (41, 2, CAST(0x0000A3B50101EB16 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (42, 3, CAST(0x0000A3B501027DEE AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (43, 2, CAST(0x0000A3B50103D602 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (44, 3, CAST(0x0000A3B50105FE42 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (45, 2, CAST(0x0000A3B700EDC408 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (46, 3, CAST(0x0000A3B700EE2473 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (47, 2, CAST(0x0000A3B700F0F0A5 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (48, 2, CAST(0x0000A3B700F1218F AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (49, 3, CAST(0x0000A3B700F13BC8 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (50, 3, CAST(0x0000A3B8012367AC AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (51, 2, CAST(0x0000A3B9013E3D51 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (52, 3, CAST(0x0000A3B9013E5243 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (53, 3, CAST(0x0000A3BD00968DDA AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (54, 2, CAST(0x0000A3BE009F82A1 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (55, 3, CAST(0x0000A3BE00A05CEE AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (1050, 3, CAST(0x0000A3BF00B0361F AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (1051, 9, CAST(0x0000A3C000ED9DB6 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (1052, 3, CAST(0x0000A3C0010E346A AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (1053, 9, CAST(0x0000A3C0010ED4DB AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (1054, 3, CAST(0x0000A3C0013E0D09 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (1055, 3, CAST(0x0000A3C30133ACF4 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (2050, 3, CAST(0x0000A3C40132CD03 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (2051, 3, CAST(0x0000A3C50114B99C AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (2052, 9, CAST(0x0000A3C50121BD8E AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (2053, 3, CAST(0x0000A3C501284EB9 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (3050, 9, CAST(0x0000A3C6011772EE AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (3051, 3, CAST(0x0000A3C6011B052D AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4050, 3, CAST(0x0000A3C7011E5064 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4051, 3, CAST(0x0000A3C7013C200C AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4052, 9, CAST(0x0000A3C7013C6091 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4053, 3, CAST(0x0000A3C7013C8B69 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4054, 3, CAST(0x0000A3C9008F9E46 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4055, 3, CAST(0x0000A3C900998C8F AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4056, 3, CAST(0x0000A3CC00D59962 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4057, 9, CAST(0x0000A3CC011B0DD5 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4058, 3, CAST(0x0000A3CC011B37FE AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4059, 3, CAST(0x0000A3D1010E2441 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4060, 3, CAST(0x0000A3D201330AD3 AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4061, 3, CAST(0x0000A3D4009ECE9D AS DateTime), 2130706433)
INSERT [dbo].[LoginHistory] ([ID], [UserID], [LoginDate], [IPAddress]) VALUES (4062, 3, CAST(0x0000A3D4013DDB8E AS DateTime), 776591592)
SET IDENTITY_INSERT [dbo].[LoginHistory] OFF
SET IDENTITY_INSERT [dbo].[Managers] ON 

INSERT [dbo].[Managers] ([ManagerUserID], [ShopOwnerID], [ID]) VALUES (5, 3, 1)
INSERT [dbo].[Managers] ([ManagerUserID], [ShopOwnerID], [ID]) VALUES (8, 3, 2)
SET IDENTITY_INSERT [dbo].[Managers] OFF
SET IDENTITY_INSERT [dbo].[MapPoints] ON 

INSERT [dbo].[MapPoints] ([ID], [Lat], [Lng], [SectorID], [Num]) VALUES (1, CAST(55.7673198258131750 AS Decimal(18, 16)), CAST(37.6636251855467500 AS Decimal(18, 16)), NULL, 0)
INSERT [dbo].[MapPoints] ([ID], [Lat], [Lng], [SectorID], [Num]) VALUES (3, CAST(55.7518327473496000 AS Decimal(18, 16)), CAST(37.5908407617185100 AS Decimal(18, 16)), NULL, 0)
INSERT [dbo].[MapPoints] ([ID], [Lat], [Lng], [SectorID], [Num]) VALUES (4, CAST(55.7084360604389800 AS Decimal(18, 16)), CAST(37.5091299462883750 AS Decimal(18, 16)), NULL, 0)
INSERT [dbo].[MapPoints] ([ID], [Lat], [Lng], [SectorID], [Num]) VALUES (5, CAST(55.7822203058220500 AS Decimal(18, 16)), CAST(37.6148733544921900 AS Decimal(18, 16)), NULL, 0)
INSERT [dbo].[MapPoints] ([ID], [Lat], [Lng], [SectorID], [Num]) VALUES (6, CAST(55.7084360604389800 AS Decimal(18, 16)), CAST(37.5091299462883750 AS Decimal(18, 16)), NULL, 0)
INSERT [dbo].[MapPoints] ([ID], [Lat], [Lng], [SectorID], [Num]) VALUES (7, CAST(55.7084360604389800 AS Decimal(18, 16)), CAST(37.5091299462883750 AS Decimal(18, 16)), NULL, 0)
INSERT [dbo].[MapPoints] ([ID], [Lat], [Lng], [SectorID], [Num]) VALUES (8, CAST(55.7084360604389800 AS Decimal(18, 16)), CAST(37.5091299462883750 AS Decimal(18, 16)), NULL, 0)
SET IDENTITY_INSERT [dbo].[MapPoints] OFF
SET IDENTITY_INSERT [dbo].[Marks] ON 

INSERT [dbo].[Marks] ([ID], [Quality], [Price], [Operator], [Delivery], [MarkDate], [OrderID], [MarkAuthorID]) VALUES (1028, 4, 3, 2, 4, CAST(0x0000A3BD013C35B4 AS DateTime), 10, 9)
INSERT [dbo].[Marks] ([ID], [Quality], [Price], [Operator], [Delivery], [MarkDate], [OrderID], [MarkAuthorID]) VALUES (2004, 2, 4, 3, 5, CAST(0x0000A3C2013C77DA AS DateTime), 21, 9)
INSERT [dbo].[Marks] ([ID], [Quality], [Price], [Operator], [Delivery], [MarkDate], [OrderID], [MarkAuthorID]) VALUES (2005, 4, 3, 4, 5, CAST(0x0000A3BD013C7CB7 AS DateTime), 19, 9)
INSERT [dbo].[Marks] ([ID], [Quality], [Price], [Operator], [Delivery], [MarkDate], [OrderID], [MarkAuthorID]) VALUES (2006, 4, 2, 4, 2, CAST(0x0000A3C7013C8209 AS DateTime), 5, 9)
INSERT [dbo].[Marks] ([ID], [Quality], [Price], [Operator], [Delivery], [MarkDate], [OrderID], [MarkAuthorID]) VALUES (2007, 4, 3, 4, 5, CAST(0x0000A3CC011B2943 AS DateTime), 11, 9)
INSERT [dbo].[Marks] ([ID], [Quality], [Price], [Operator], [Delivery], [MarkDate], [OrderID], [MarkAuthorID]) VALUES (2008, 5, 3, 5, 2, CAST(0x0000A3CC011B2FC9 AS DateTime), 9, 9)
SET IDENTITY_INSERT [dbo].[Marks] OFF
SET IDENTITY_INSERT [dbo].[OperatorShops] ON 

INSERT [dbo].[OperatorShops] ([ID], [UserID], [ShopID]) VALUES (1, 9, 2)
INSERT [dbo].[OperatorShops] ([ID], [UserID], [ShopID]) VALUES (2, 9, 4)
SET IDENTITY_INSERT [dbo].[OperatorShops] OFF
SET IDENTITY_INSERT [dbo].[OrderChars] ON 

INSERT [dbo].[OrderChars] ([ID], [Name], [OrderID], [Value]) VALUES (1006, N'Этаж', 5, N'2')
INSERT [dbo].[OrderChars] ([ID], [Name], [OrderID], [Value]) VALUES (1008, N'Номер 1С', 5, N'1')
INSERT [dbo].[OrderChars] ([ID], [Name], [OrderID], [Value]) VALUES (1009, N'В наличии', 14, N'Да')
INSERT [dbo].[OrderChars] ([ID], [Name], [OrderID], [Value]) VALUES (1010, N'Этаж', 14, N'1')
INSERT [dbo].[OrderChars] ([ID], [Name], [OrderID], [Value]) VALUES (2002, N'В наличии', 19, N'Да')
INSERT [dbo].[OrderChars] ([ID], [Name], [OrderID], [Value]) VALUES (2003, N'Этаж', 19, N'1')
INSERT [dbo].[OrderChars] ([ID], [Name], [OrderID], [Value]) VALUES (2004, N'Номер 1С', 19, N'')
INSERT [dbo].[OrderChars] ([ID], [Name], [OrderID], [Value]) VALUES (2005, N'В наличии', 21, N'Да')
INSERT [dbo].[OrderChars] ([ID], [Name], [OrderID], [Value]) VALUES (2006, N'Этаж', 21, N'1')
INSERT [dbo].[OrderChars] ([ID], [Name], [OrderID], [Value]) VALUES (2007, N'Номер 1С', 21, N'')
INSERT [dbo].[OrderChars] ([ID], [Name], [OrderID], [Value]) VALUES (3005, N'В наличии', 1023, N'Да')
INSERT [dbo].[OrderChars] ([ID], [Name], [OrderID], [Value]) VALUES (3006, N'Этаж', 1023, N'1')
INSERT [dbo].[OrderChars] ([ID], [Name], [OrderID], [Value]) VALUES (3007, N'Test', 1023, N'Test')
SET IDENTITY_INSERT [dbo].[OrderChars] OFF
SET IDENTITY_INSERT [dbo].[OrderedProducts] ON 

INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (11, 2, 1, CAST(123.00 AS Decimal(18, 2)), 123, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (12, 2, 10, CAST(123.00 AS Decimal(18, 2)), 123, CAST(123.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (13, 2, 10, CAST(123.00 AS Decimal(18, 2)), 123, CAST(123.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (14, 2, 9, CAST(123.00 AS Decimal(18, 2)), 123, CAST(123.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (15, 5, 11, CAST(87654.00 AS Decimal(18, 2)), 8, NULL)
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (16, 1, 14, CAST(123.00 AS Decimal(18, 2)), 1, CAST(2.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (17, 6, 6, CAST(123.00 AS Decimal(18, 2)), 123, CAST(4.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (18, 7, 1, CAST(24.00 AS Decimal(18, 2)), 30, CAST(123.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (19, 8, 7, CAST(123.00 AS Decimal(18, 2)), 123, CAST(8765.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (20, 4, 3, CAST(234532.00 AS Decimal(18, 2)), 3453, CAST(345.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (21, 2, 4, CAST(123.00 AS Decimal(18, 2)), 3453, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (27, 9, 19, CAST(5000.00 AS Decimal(18, 2)), 2, CAST(15.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (28, 9, 19, CAST(5000.00 AS Decimal(18, 2)), 1, NULL)
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (29, 9, 5, CAST(5000.00 AS Decimal(18, 2)), 1, CAST(23.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (1022, 1, 19, CAST(123.00 AS Decimal(18, 2)), 5, CAST(10.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (1023, 9, 19, CAST(5000.00 AS Decimal(18, 2)), 7, NULL)
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (1024, 1, 19, CAST(123.00 AS Decimal(18, 2)), 1, NULL)
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (1025, 3, 19, CAST(24554.00 AS Decimal(18, 2)), 1, NULL)
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (1026, 2, 19, CAST(123.00 AS Decimal(18, 2)), 3, NULL)
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (2022, 1, 21, CAST(123.00 AS Decimal(18, 2)), 500, NULL)
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (3026, 10, 1024, CAST(5000.00 AS Decimal(18, 2)), 1, NULL)
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (3042, 10, 1024, CAST(5000.00 AS Decimal(18, 2)), 1, NULL)
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (3043, 12, 1024, CAST(5000.00 AS Decimal(18, 2)), 1, NULL)
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (3044, 12, 1024, CAST(5000.00 AS Decimal(18, 2)), 1, NULL)
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (3045, 12, 1024, CAST(5000.00 AS Decimal(18, 2)), 1, NULL)
INSERT [dbo].[OrderedProducts] ([ID], [ProductID], [OrderID], [Price], [Amount], [Weight]) VALUES (3046, 13, 1024, CAST(5000.00 AS Decimal(18, 2)), 1, NULL)
SET IDENTITY_INSERT [dbo].[OrderedProducts] OFF
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (1, CAST(0x0000A39D00C7F54C AS DateTime), 4, -10, 3, 1, 1, N'Важная информация', 1, N'452354325', NULL, NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (3, CAST(0x0000A39D00C832A2 AS DateTime), 2, 0, 3, 8, 1, N'123', 1, N'Номер', NULL, NULL, NULL, 1, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (4, CAST(0x0000A39D00CFAA40 AS DateTime), 5, 0, 3, 9, 1, NULL, 0, NULL, NULL, NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (5, CAST(0x0000A39D00CFAF32 AS DateTime), 2, 100, 3, 1, 1, N'', 0, N'Новый номер', CAST(0x0000A3D600000000 AS DateTime), N'Тучково', NULL, 1, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (6, CAST(0x0000A39D00E1194B AS DateTime), 5, 100, 3, 6, 1, N'23452354', 0, N'9876543', CAST(0x0000A3B700000000 AS DateTime), NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (7, CAST(0x0000A39D010194DA AS DateTime), 5, 100, 3, 7, 1, N'Коммент', 1, N'09876543', CAST(0x0000A3B700000000 AS DateTime), NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (9, CAST(0x0000A3A300C1D26B AS DateTime), 4, 100, 5, 3, 1, NULL, 0, NULL, CAST(0x0000A3B700000000 AS DateTime), NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (10, CAST(0x0000A3A300C6A290 AS DateTime), 4, 100, 3, 2, 1, NULL, 0, NULL, CAST(0x0000A3B700000000 AS DateTime), NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (11, CAST(0x0000A3A300CA03A2 AS DateTime), 4, 100, 5, 4, 1, NULL, 0, NULL, CAST(0x0000A3B700000000 AS DateTime), NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (14, CAST(0x0000A3B50107236E AS DateTime), 5, 100, 3, 5, 1, N'Тут текст заметки', 1, N'''14''', CAST(0x0000A3B700000000 AS DateTime), N'Тучково', NULL, 0, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (19, CAST(0x0000A3BE00AEC672 AS DateTime), 2, 100, 6, 1, 1, N'', 0, N'', CAST(0x0000A3CF00000000 AS DateTime), N'', NULL, 1, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (20, CAST(0x0000A3C4014239A5 AS DateTime), 2, -1, 3, NULL, 1, NULL, 0, NULL, NULL, NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (21, CAST(0x0000A3C5011AD70D AS DateTime), 2, 0, 3, 7, 1, N'', 0, N'', CAST(0x0000A3CE00000000 AS DateTime), NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (1021, CAST(0x0000A3C7013C3359 AS DateTime), 2, -1, 3, NULL, 1, NULL, 0, NULL, NULL, NULL, NULL, 1, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (1022, CAST(0x0000A3C9008FB185 AS DateTime), 2, -1, 3, NULL, 1, NULL, 0, NULL, NULL, NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (1023, CAST(0x0000A3C9009991F9 AS DateTime), 2, -1, 3, NULL, 1, N'', 0, N'', NULL, NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (1024, CAST(0x0000A3C900A00612 AS DateTime), 2, -1, 3, NULL, 1, NULL, 0, NULL, NULL, NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([ID], [CreateDate], [ShopID], [Status], [AddedByID], [ConsumerID], [IsTemp], [Warning], [IsImportant], [OrderNumber], [DeliveryDate], [DeliveryAddress], [SkipMarkTo], [IsPayed], [AdressID]) VALUES (1025, CAST(0x0000A3D20133114D AS DateTime), 2, -1, 3, 1, 1, NULL, 0, NULL, NULL, NULL, NULL, 0, 10)
SET IDENTITY_INSERT [dbo].[Orders] OFF
SET IDENTITY_INSERT [dbo].[PermissionGroups] ON 

INSERT [dbo].[PermissionGroups] ([ID], [Name]) VALUES (1, N'Магазины')
INSERT [dbo].[PermissionGroups] ([ID], [Name]) VALUES (2, N'Пользователи')
INSERT [dbo].[PermissionGroups] ([ID], [Name]) VALUES (3, N'Заказы')
INSERT [dbo].[PermissionGroups] ([ID], [Name]) VALUES (4, N'Оценка заказов')
INSERT [dbo].[PermissionGroups] ([ID], [Name]) VALUES (5, N'Статистика')
INSERT [dbo].[PermissionGroups] ([ID], [Name]) VALUES (6, N'Доставка')
SET IDENTITY_INSERT [dbo].[PermissionGroups] OFF
SET IDENTITY_INSERT [dbo].[Permissions] ON 

INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (1, N'Добавление магазина', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (2, N'Редактирование магазина', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (4, N'Просмотр списка магазинов', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (5, N'Добавление', 2)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (6, N'Редактирование данных', 2)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (7, N'Изменение характеристик товаров', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (8, N'Удаление', 2)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (9, N'Блокировка', 2)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (10, N'Удаление магазина', 1)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (11, N'Просмотр списка', 2)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (12, N'Добавление заказа', 3)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (13, N'Редактирование заказа', 3)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (14, N'Изменение статуса', 3)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (15, N'Просмотр списка заказов', 3)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (16, N'Удаление заказа из системы', 3)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (17, N'Авторизация', 2)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (18, N'Просмотр списка заказов', 4)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (19, N'Выставление оценки', 4)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (20, N'Выставление статуса оплаты', 3)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (1021, N'Просмотр статистики удовлетворенности клиентов', 5)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (1022, N'Просмотр статистики качества облуживания', 5)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (1024, N'Просмотр статистики количества заказов', 5)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (1027, N'Просмотр статистики по "среднему чеку"', 5)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (1028, N'Просмотр статистики по объему продаж', 5)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (1029, N'Просмотр общей статистики', 5)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (1030, N'СМС-рассылка', 3)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (1031, N'E-mail рассылка', 3)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (1033, N'Просмотр статистики по статусам заказов', 5)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (1034, N'Адреса складов', 6)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (1036, N'Список зон', 6)
INSERT [dbo].[Permissions] ([ID], [Name], [GroupID]) VALUES (1037, N'Заказы для доставки', 6)
SET IDENTITY_INSERT [dbo].[Permissions] OFF
SET IDENTITY_INSERT [dbo].[PermissionsForRoles] ON 

INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (1, 3, 1)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (2, 3, 2)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (6, 3, 4)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (7, 3, 5)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (8, 3, 6)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (9, 3, 7)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (10, 3, 8)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (11, 3, 9)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (12, 3, 10)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (13, 3, 11)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (14, 3, 12)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (15, 3, 13)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (16, 3, 14)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (17, 3, 15)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (18, 3, 16)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (19, 3, 17)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (20, 5, 18)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (21, 5, 19)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (22, 1, 1)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (23, 1, 2)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (24, 1, 4)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (25, 1, 5)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (26, 1, 6)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (27, 1, 7)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (28, 1, 8)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (29, 1, 9)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (30, 1, 10)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (31, 1, 11)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (32, 1, 12)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (33, 1, 13)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (34, 1, 14)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (35, 1, 15)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (36, 1, 16)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (37, 1, 17)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (38, 1, 18)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (39, 1, 19)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (40, 1, 20)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (41, 3, 20)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (1040, 3, 1021)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (1041, 3, 1022)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (1042, 3, 1024)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (1043, 3, 1027)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (1044, 3, 1028)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (1045, 3, 1029)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (1046, 3, 1030)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (1047, 3, 1031)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (1048, 3, 1033)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (1049, 3, 1034)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (1052, 3, 1036)
INSERT [dbo].[PermissionsForRoles] ([ID], [RoleID], [PermissionID]) VALUES (1053, 3, 1037)
SET IDENTITY_INSERT [dbo].[PermissionsForRoles] OFF
SET IDENTITY_INSERT [dbo].[ProductChars] ON 

INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (31, N'В наличии', 27, N'Нет')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (32, N'Количество в упаковке', 27, N'10')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (33, N'В наличии', 28, N'Да')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (34, N'Количество в упаковке', 28, N'5')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (35, N'В наличии', 29, N'Да')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (36, N'Количество в упаковке', 29, N'10')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (1031, N'В наличии', 1022, N'Да')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (1032, N'Количество в упаковке', 1022, N'5')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (1033, N'В наличии', 1023, N'Да')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (1034, N'Количество в упаковке', 1023, N'10')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (1035, N'В наличии', 1024, N'Да')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (1036, N'Количество в упаковке', 1024, N'5')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (1037, N'В наличии', 1025, N'Да')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (1038, N'Количество в упаковке', 1025, N'5')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (1039, N'В наличии', 1026, N'Да')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (1040, N'Количество в упаковке', 1026, N'10')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (1041, N'Test', 1025, N'Test')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (2031, N'В наличии', 2022, N'Да')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (2032, N'Количество в упаковке', 2022, N'5')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (2033, N'Test', 2022, N'Test')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (2038, N'123', 2022, N'123')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (3039, N'Test1', 3026, N'Test1')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (3040, N'Test2', 3026, N'Test2')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (3079, N'Test1', 3042, N'Test1')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (3080, N'Test1', 3043, N'Test1')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (3081, N'Цвет', 3043, N'Красный')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (3082, N'Test1', 3044, N'Test1')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (3083, N'Цвет', 3044, N'Красный')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (3084, N'Test1', 3045, N'Test1')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (3085, N'Цвет', 3045, N'Красный')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (3088, N'Test1', 3046, N'Test1')
INSERT [dbo].[ProductChars] ([ID], [Name], [OrderedProductID], [Value]) VALUES (3089, N'Цвет', 3046, N'Красный')
SET IDENTITY_INSERT [dbo].[ProductChars] OFF
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ID], [Name], [AddedDate], [Article], [OwnerID], [UnitCode]) VALUES (1, N'Новый телефон', CAST(0x0000A39D00DD6735 AS DateTime), N'ArtXXX', 3, N'796')
INSERT [dbo].[Products] ([ID], [Name], [AddedDate], [Article], [OwnerID], [UnitCode]) VALUES (2, N'123', CAST(0x0000A39D00EBA8EA AS DateTime), N'Art2', 3, N'796')
INSERT [dbo].[Products] ([ID], [Name], [AddedDate], [Article], [OwnerID], [UnitCode]) VALUES (3, N'0985432', CAST(0x0000A39D00EE3854 AS DateTime), N'Art20', 3, N'796')
INSERT [dbo].[Products] ([ID], [Name], [AddedDate], [Article], [OwnerID], [UnitCode]) VALUES (4, N'-098765432', CAST(0x0000A39D00F322CD AS DateTime), N'Art002', 3, N'796')
INSERT [dbo].[Products] ([ID], [Name], [AddedDate], [Article], [OwnerID], [UnitCode]) VALUES (5, N'76543', CAST(0x0000A3A300CA0E73 AS DateTime), N'876543', 5, N'796')
INSERT [dbo].[Products] ([ID], [Name], [AddedDate], [Article], [OwnerID], [UnitCode]) VALUES (6, N'В наличии', CAST(0x0000A3B700C716BA AS DateTime), N'345345', 3, N'796')
INSERT [dbo].[Products] ([ID], [Name], [AddedDate], [Article], [OwnerID], [UnitCode]) VALUES (7, N'35', CAST(0x0000A3B700D0224B AS DateTime), N'5634253', 3, N'796')
INSERT [dbo].[Products] ([ID], [Name], [AddedDate], [Article], [OwnerID], [UnitCode]) VALUES (8, N'Мощность', CAST(0x0000A3B700F8E9EB AS DateTime), N'123', 3, N'796')
INSERT [dbo].[Products] ([ID], [Name], [AddedDate], [Article], [OwnerID], [UnitCode]) VALUES (9, N'Новый телефон', CAST(0x0000A3BE00B7855E AS DateTime), N'ArtXXX', 3, N'796')
INSERT [dbo].[Products] ([ID], [Name], [AddedDate], [Article], [OwnerID], [UnitCode]) VALUES (10, N'Телефон', CAST(0x0000A3C900B42B02 AS DateTime), N'ArtN', 3, N'796')
INSERT [dbo].[Products] ([ID], [Name], [AddedDate], [Article], [OwnerID], [UnitCode]) VALUES (11, N'Телефон', CAST(0x0000A3C900B9B8CF AS DateTime), N'ArtNNNN', 3, N'796')
INSERT [dbo].[Products] ([ID], [Name], [AddedDate], [Article], [OwnerID], [UnitCode]) VALUES (12, N'Телефон', CAST(0x0000A3C900BBB59D AS DateTime), N'ArtNRed', 3, N'796')
INSERT [dbo].[Products] ([ID], [Name], [AddedDate], [Article], [OwnerID], [UnitCode]) VALUES (13, N'Телефон', CAST(0x0000A3C900BC4297 AS DateTime), N'ArtNRed', 3, N'796')
SET IDENTITY_INSERT [dbo].[Products] OFF
SET IDENTITY_INSERT [dbo].[ShopManagers] ON 

INSERT [dbo].[ShopManagers] ([ID], [ManagerID], [ShopID]) VALUES (2, 1, 4)
INSERT [dbo].[ShopManagers] ([ID], [ManagerID], [ShopID]) VALUES (5, 1, 2)
SET IDENTITY_INSERT [dbo].[ShopManagers] OFF
SET IDENTITY_INSERT [dbo].[ShopProductChars] ON 

INSERT [dbo].[ShopProductChars] ([ID], [Name], [ShopID], [DefValue], [UserID], [IsMain], [Type], [LoadInForm]) VALUES (2, N'В наличии', NULL, N'Да', 3, 1, 3, 0)
INSERT [dbo].[ShopProductChars] ([ID], [Name], [ShopID], [DefValue], [UserID], [IsMain], [Type], [LoadInForm]) VALUES (4, N'Количество в упаковке', 2, N'5', 3, 1, 1, 1)
INSERT [dbo].[ShopProductChars] ([ID], [Name], [ShopID], [DefValue], [UserID], [IsMain], [Type], [LoadInForm]) VALUES (5, N'Цвет', 4, NULL, 3, 0, 1, 1)
INSERT [dbo].[ShopProductChars] ([ID], [Name], [ShopID], [DefValue], [UserID], [IsMain], [Type], [LoadInForm]) VALUES (7, N'Размер', 4, NULL, 3, 1, 1, 1)
INSERT [dbo].[ShopProductChars] ([ID], [Name], [ShopID], [DefValue], [UserID], [IsMain], [Type], [LoadInForm]) VALUES (1008, N'Номер 1С', 2, NULL, 3, 1, 3, 0)
SET IDENTITY_INSERT [dbo].[ShopProductChars] OFF
SET IDENTITY_INSERT [dbo].[Shops] ON 

INSERT [dbo].[Shops] ([ID], [Name], [Owner], [TaxPlanID], [Link], [CreateDate]) VALUES (2, N'Магазин Игрушек', 3, 1, N'http://myshop.ru', CAST(0x0000A39C010AEF8C AS DateTime))
INSERT [dbo].[Shops] ([ID], [Name], [Owner], [TaxPlanID], [Link], [CreateDate]) VALUES (4, N'Мой магазин', 3, 1, N'http://myshop.com1', CAST(0x0000A39D00C30C6C AS DateTime))
INSERT [dbo].[Shops] ([ID], [Name], [Owner], [TaxPlanID], [Link], [CreateDate]) VALUES (5, N'Еще один', 3, 3, N'123', CAST(0x0000A3A300D701BE AS DateTime))
SET IDENTITY_INSERT [dbo].[Shops] OFF
SET IDENTITY_INSERT [dbo].[ShopSettings] ON 

INSERT [dbo].[ShopSettings] ([ID], [ShopID], [Name], [Value], [ItemKey]) VALUES (10, 2, N'Наименование юр. лица', N'ООО ФЭМ "Промтекс-Ориент"', N'OrgName')
INSERT [dbo].[ShopSettings] ([ID], [ShopID], [Name], [Value], [ItemKey]) VALUES (11, 2, N'ИНН', N'7724762824,', N'INN')
INSERT [dbo].[ShopSettings] ([ID], [ShopID], [Name], [Value], [ItemKey]) VALUES (12, 2, N'Адрес', N'115201, Москва г, Каширский проезд, дом № 17, строение 2', N'Address')
INSERT [dbo].[ShopSettings] ([ID], [ShopID], [Name], [Value], [ItemKey]) VALUES (13, 2, N'Тел./факс', N'тел.: (499) 794-53-48, факс: (499) 794-53-80', N'Phones')
INSERT [dbo].[ShopSettings] ([ID], [ShopID], [Name], [Value], [ItemKey]) VALUES (14, 2, N'Р/с', N'40702810800000003942', N'OrgAccount')
INSERT [dbo].[ShopSettings] ([ID], [ShopID], [Name], [Value], [ItemKey]) VALUES (15, 2, N'Банк', N'АБ "ИНТЕРПРОГРЕССБАНК" (ЗАО)', N'Bank')
INSERT [dbo].[ShopSettings] ([ID], [ShopID], [Name], [Value], [ItemKey]) VALUES (16, 2, N'БИК', N'044525402', N'Bik')
INSERT [dbo].[ShopSettings] ([ID], [ShopID], [Name], [Value], [ItemKey]) VALUES (17, 2, N'К/с', N'30101810100000000402', N'Korr')
INSERT [dbo].[ShopSettings] ([ID], [ShopID], [Name], [Value], [ItemKey]) VALUES (18, 2, N'Код ОКПО', N'68826510', N'OKPO')
SET IDENTITY_INSERT [dbo].[ShopSettings] OFF
SET IDENTITY_INSERT [dbo].[Stores] ON 

INSERT [dbo].[Stores] ([ID], [Name], [AdressID], [ShopID]) VALUES (1, N'Мой склад', 11, 2)
INSERT [dbo].[Stores] ([ID], [Name], [AdressID], [ShopID]) VALUES (2, N'Склад телефонов', 12, 4)
SET IDENTITY_INSERT [dbo].[Stores] OFF
SET IDENTITY_INSERT [dbo].[TaxPlanes] ON 

INSERT [dbo].[TaxPlanes] ([ID], [Name], [MonthCost], [Description]) VALUES (1, N'Promo', CAST(0.00 AS Decimal(18, 2)), N'«Промо» тариф без оплаты – смс опрос, email рассылка, qr – коды')
INSERT [dbo].[TaxPlanes] ([ID], [Name], [MonthCost], [Description]) VALUES (2, N'Auto', CAST(200.00 AS Decimal(18, 2)), N'Автообзвон через автоматическую систему, для разгрузки операторов')
INSERT [dbo].[TaxPlanes] ([ID], [Name], [MonthCost], [Description]) VALUES (3, N'Live', CAST(500.00 AS Decimal(18, 2)), N'Живые звонки от операторов, индивидуальные вопросы')
INSERT [dbo].[TaxPlanes] ([ID], [Name], [MonthCost], [Description]) VALUES (4, N'Crysis', CAST(1000.00 AS Decimal(18, 2)), N'Кризис – менеджер, урегулирование проблемных ситуаций, представитель клиента перед интернет магазином. Значительное повышение имиджа интернет магазина. ')
SET IDENTITY_INSERT [dbo].[TaxPlanes] OFF
SET IDENTITY_INSERT [dbo].[UserAllowedRoles] ON 

INSERT [dbo].[UserAllowedRoles] ([ID], [RoleID], [AllowedRoleID]) VALUES (1, 1, 2)
INSERT [dbo].[UserAllowedRoles] ([ID], [RoleID], [AllowedRoleID]) VALUES (2, 1, 3)
INSERT [dbo].[UserAllowedRoles] ([ID], [RoleID], [AllowedRoleID]) VALUES (3, 1, 4)
INSERT [dbo].[UserAllowedRoles] ([ID], [RoleID], [AllowedRoleID]) VALUES (4, 1, 5)
INSERT [dbo].[UserAllowedRoles] ([ID], [RoleID], [AllowedRoleID]) VALUES (5, 3, 4)
INSERT [dbo].[UserAllowedRoles] ([ID], [RoleID], [AllowedRoleID]) VALUES (7, 4, 4)
INSERT [dbo].[UserAllowedRoles] ([ID], [RoleID], [AllowedRoleID]) VALUES (8, 4, 2)
INSERT [dbo].[UserAllowedRoles] ([ID], [RoleID], [AllowedRoleID]) VALUES (9, 5, 2)
INSERT [dbo].[UserAllowedRoles] ([ID], [RoleID], [AllowedRoleID]) VALUES (10, 1, 1)
SET IDENTITY_INSERT [dbo].[UserAllowedRoles] OFF
SET IDENTITY_INSERT [dbo].[UserPermissions] ON 

INSERT [dbo].[UserPermissions] ([ID], [PermissionID], [UserID]) VALUES (40, 15, 5)
INSERT [dbo].[UserPermissions] ([ID], [PermissionID], [UserID]) VALUES (44, 13, 5)
INSERT [dbo].[UserPermissions] ([ID], [PermissionID], [UserID]) VALUES (46, 11, 5)
INSERT [dbo].[UserPermissions] ([ID], [PermissionID], [UserID]) VALUES (50, 6, 5)
INSERT [dbo].[UserPermissions] ([ID], [PermissionID], [UserID]) VALUES (56, 2, 5)
INSERT [dbo].[UserPermissions] ([ID], [PermissionID], [UserID]) VALUES (57, 7, 5)
INSERT [dbo].[UserPermissions] ([ID], [PermissionID], [UserID]) VALUES (58, 7, 8)
INSERT [dbo].[UserPermissions] ([ID], [PermissionID], [UserID]) VALUES (60, 19, 9)
INSERT [dbo].[UserPermissions] ([ID], [PermissionID], [UserID]) VALUES (61, 18, 9)
SET IDENTITY_INSERT [dbo].[UserPermissions] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([ID], [Email], [Name], [UserName], [UserSurname], [UserPatrinomic], [LastVisitDate], [Phone], [IsPhoneConfirmed], [RegStep], [Nick], [DigitID], [ConfirmKey], [IsLocked], [IsDeleted]) VALUES (2, N'megaprogrammer7@gmail.com', N'admin', N'Антон', N'Ковецкий', N'Сергеевич', NULL, NULL, NULL, NULL, N'Admin', NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([ID], [Email], [Name], [UserName], [UserSurname], [UserPatrinomic], [LastVisitDate], [Phone], [IsPhoneConfirmed], [RegStep], [Nick], [DigitID], [ConfirmKey], [IsLocked], [IsDeleted]) VALUES (3, N'KovetskiyAS@mail.ru', N'KovetskiyAS@mail.ru', N'Антон', N'Ковецкий', N'Сергеевич', NULL, N'(926) 265-75-08', 0, NULL, NULL, N'954721439', N'8dfa9282-8973-4173-a4bf-ffe6a5eccdfd', NULL, NULL)
INSERT [dbo].[Users] ([ID], [Email], [Name], [UserName], [UserSurname], [UserPatrinomic], [LastVisitDate], [Phone], [IsPhoneConfirmed], [RegStep], [Nick], [DigitID], [ConfirmKey], [IsLocked], [IsDeleted]) VALUES (5, N'admin123@mail.ru', N'admin123@mail.ru', N'Mans', N'Manager', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([ID], [Email], [Name], [UserName], [UserSurname], [UserPatrinomic], [LastVisitDate], [Phone], [IsPhoneConfirmed], [RegStep], [Nick], [DigitID], [ConfirmKey], [IsLocked], [IsDeleted]) VALUES (6, N'KovetskiyAS@yandex.ru', N'KovetskiyAS@yandex.ru', N'Петр', N'Петров', N'Петрович', NULL, N'(926) 788-99-00', 0, 1, NULL, N'631071585', N'30452aae-87aa-4eca-bdd0-04513c9d1a83', NULL, NULL)
INSERT [dbo].[Users] ([ID], [Email], [Name], [UserName], [UserSurname], [UserPatrinomic], [LastVisitDate], [Phone], [IsPhoneConfirmed], [RegStep], [Nick], [DigitID], [ConfirmKey], [IsLocked], [IsDeleted]) VALUES (7, N'photon-cms@mail.ru', N'photon-cms@mail.ru', N'Иван', N'Иванов', N'Иванович', NULL, N'(234) 567-89-09', 0, NULL, NULL, N'167628140', N'791af207-3303-47b2-84e6-975228755e16', 1, NULL)
INSERT [dbo].[Users] ([ID], [Email], [Name], [UserName], [UserSurname], [UserPatrinomic], [LastVisitDate], [Phone], [IsPhoneConfirmed], [RegStep], [Nick], [DigitID], [ConfirmKey], [IsLocked], [IsDeleted]) VALUES (8, N'tester@mail.ru', N'tester@mail.ru', N'Петр', N'Петров', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([ID], [Email], [Name], [UserName], [UserSurname], [UserPatrinomic], [LastVisitDate], [Phone], [IsPhoneConfirmed], [RegStep], [Nick], [DigitID], [ConfirmKey], [IsLocked], [IsDeleted]) VALUES (9, N'oper@mail.ru', N'oper@mail.ru', N'Опер', N'Оператор', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (2, CAST(0x0000A382009F29E9 AS DateTime), NULL, 1, CAST(0x0000A39900FC0B2A AS DateTime), 0, N'APIub5ksHdRW+0Ycb2gM3FDTOIN/gWKmQF7B6LhjxIQWJ+VBLS02h+FwQ+TFZIfl+A==', CAST(0x0000A39400D63D6B AS DateTime), N'', N'lYRnH_oCaRQVI3GGe2jKnQ2', CAST(0x0000A3A200B2C0F7 AS DateTime))
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (3, CAST(0x0000A39700950DFE AS DateTime), NULL, 1, CAST(0x0000A3BD00549D70 AS DateTime), 0, N'AL9ejOdxcL+7w9g7+6tx+1IQ0aL7XDX2QiE7FxnL91Ko01uezh9byZYf2wVnBvldnA==', CAST(0x0000A3A100B5772C AS DateTime), N'', N'q1RnVQVrT0iSxFt2iBRQbQ2', CAST(0x0000A3A200B58C61 AS DateTime))
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (5, CAST(0x0000A39B011A3571 AS DateTime), NULL, 1, CAST(0x0000A3A100B276BA AS DateTime), 0, N'AEdtNRQyXJPEP7f3CKM5GCLwhOWPASnctrpDNy9mTo1v8Dh1i+SArA7i3Df/2kWFZA==', CAST(0x0000A3A100B2439F AS DateTime), N'', N'Z_9gl_rrVfCkCXxBlPIh0Q2', CAST(0x0000A3A200B24FD2 AS DateTime))
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (6, CAST(0x0000A39D0074688C AS DateTime), NULL, 1, NULL, 0, N'AAfyq0zKSdl2i1qfRKW5uqt5bN4HN8YxRQJOJwNST3Qg9h5TfPpQicZ9Hx4r8grwnw==', CAST(0x0000A39D0074688C AS DateTime), N'', NULL, NULL)
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (7, CAST(0x0000A39D00764DED AS DateTime), NULL, 1, NULL, 0, N'AOU0SPbWraZJ0YJ9WsyGJeq3Y5Sx4SsancYAgwRHTxZLryk/yyiT7nBOTo852fyv2g==', CAST(0x0000A39D00766DEC AS DateTime), N'', NULL, NULL)
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (8, CAST(0x0000A3A40066C53D AS DateTime), NULL, 1, NULL, 0, N'AASdhzD7VFcJWH0PXjp3PvyDETQqdXHg6visEfQto4P74oeOR5gDROQDMgMRt7ssvQ==', CAST(0x0000A3A40066C53D AS DateTime), N'', NULL, NULL)
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (9, CAST(0x0000A3A800FDE621 AS DateTime), NULL, 1, NULL, 0, N'AByKFeSAMnDGTemCd/lU/mhziqneuT0oPU4EfVxpMlFLc3DPL8I/ano6yN3iWoD2Kg==', CAST(0x0000A3A800FE9AB3 AS DateTime), N'', NULL, NULL)
SET IDENTITY_INSERT [dbo].[webpages_Roles] ON 

INSERT [dbo].[webpages_Roles] ([RoleId], [RoleName], [RoleDescription]) VALUES (1, N'Admin', N'Администратор системы')
INSERT [dbo].[webpages_Roles] ([RoleId], [RoleName], [RoleDescription]) VALUES (2, N'Client', N'Покупатель')
INSERT [dbo].[webpages_Roles] ([RoleId], [RoleName], [RoleDescription]) VALUES (3, N'ShopOwner', N'Владелец магазина')
INSERT [dbo].[webpages_Roles] ([RoleId], [RoleName], [RoleDescription]) VALUES (4, N'Manager', N'Менеджер магазина')
INSERT [dbo].[webpages_Roles] ([RoleId], [RoleName], [RoleDescription]) VALUES (5, N'Operator', N'Оператор системы')
SET IDENTITY_INSERT [dbo].[webpages_Roles] OFF
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (2, 1)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (3, 3)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (5, 4)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (6, 3)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (7, 3)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (8, 4)
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (9, 5)
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__webpages__8A2B6160AE40273D]    Script Date: 31.10.2014 18:47:36 ******/
ALTER TABLE [dbo].[webpages_Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Consumers] ADD  CONSTRAINT [DF_Consumers_AddPhone]  DEFAULT ('') FOR [AddPhone]
GO
ALTER TABLE [dbo].[Consumers] ADD  CONSTRAINT [DF_Consumers_Email]  DEFAULT ('') FOR [Email]
GO
ALTER TABLE [dbo].[Consumers] ADD  CONSTRAINT [DF_Consumers_Sex]  DEFAULT ((0)) FOR [Sex]
GO
ALTER TABLE [dbo].[MapPoints] ADD  CONSTRAINT [DF_MapPoints_Num]  DEFAULT ((0)) FOR [Num]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_IsTemp]  DEFAULT ((1)) FOR [IsTemp]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_IsImportant]  DEFAULT ((0)) FOR [IsImportant]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_IsPayed]  DEFAULT ((0)) FOR [IsPayed]
GO
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_UnitCode]  DEFAULT ((796)) FOR [UnitCode]
GO
ALTER TABLE [dbo].[ShopProductChars] ADD  CONSTRAINT [DF_ShopProductChars_IsMain]  DEFAULT ((0)) FOR [IsMain]
GO
ALTER TABLE [dbo].[ShopProductChars] ADD  CONSTRAINT [DF_ShopProductChars_Type]  DEFAULT ((1)) FOR [Type]
GO
ALTER TABLE [dbo].[ShopProductChars] ADD  CONSTRAINT [DF_ShopProductChars_LoadInForm]  DEFAULT ((1)) FOR [LoadInForm]
GO
ALTER TABLE [dbo].[Shops] ADD  CONSTRAINT [DF_Shops_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsPhoneConfirmed]  DEFAULT ((0)) FOR [IsPhoneConfirmed]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsLocked]  DEFAULT ((0)) FOR [IsLocked]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [IsConfirmed]
GO
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [PasswordFailuresSinceLastSuccess]
GO
ALTER TABLE [dbo].[webpages_Roles] ADD  CONSTRAINT [DF_webpages_Roles_RoleDescription]  DEFAULT ('') FOR [RoleDescription]
GO
ALTER TABLE [dbo].[Consumers]  WITH CHECK ADD  CONSTRAINT [FK_Consumers_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Consumers] CHECK CONSTRAINT [FK_Consumers_Users]
GO
ALTER TABLE [dbo].[DeliveryAddress]  WITH CHECK ADD  CONSTRAINT [FK_OrderDelivery_MapPoints] FOREIGN KEY([PointID])
REFERENCES [dbo].[MapPoints] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DeliveryAddress] CHECK CONSTRAINT [FK_OrderDelivery_MapPoints]
GO
ALTER TABLE [dbo].[LogEntry]  WITH CHECK ADD  CONSTRAINT [FK_LogEntry_LogAction] FOREIGN KEY([ActionID])
REFERENCES [dbo].[LogAction] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LogEntry] CHECK CONSTRAINT [FK_LogEntry_LogAction]
GO
ALTER TABLE [dbo].[LogEntry]  WITH CHECK ADD  CONSTRAINT [FK_LogEntry_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LogEntry] CHECK CONSTRAINT [FK_LogEntry_Users]
GO
ALTER TABLE [dbo].[LoginHistory]  WITH CHECK ADD  CONSTRAINT [FK_LoginHistory_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LoginHistory] CHECK CONSTRAINT [FK_LoginHistory_Users]
GO
ALTER TABLE [dbo].[Managers]  WITH CHECK ADD  CONSTRAINT [FK_ShopManagers_Users] FOREIGN KEY([ManagerUserID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Managers] CHECK CONSTRAINT [FK_ShopManagers_Users]
GO
ALTER TABLE [dbo].[Managers]  WITH CHECK ADD  CONSTRAINT [FK_ShopManagers_Users1] FOREIGN KEY([ShopOwnerID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Managers] CHECK CONSTRAINT [FK_ShopManagers_Users1]
GO
ALTER TABLE [dbo].[MapPoints]  WITH CHECK ADD  CONSTRAINT [FK_MapPoints_MapSectors] FOREIGN KEY([SectorID])
REFERENCES [dbo].[MapSectors] ([ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[MapPoints] CHECK CONSTRAINT [FK_MapPoints_MapSectors]
GO
ALTER TABLE [dbo].[MapSectors]  WITH CHECK ADD  CONSTRAINT [FK_MapSectors_Shops] FOREIGN KEY([ShopID])
REFERENCES [dbo].[Shops] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MapSectors] CHECK CONSTRAINT [FK_MapSectors_Shops]
GO
ALTER TABLE [dbo].[MapSectors]  WITH CHECK ADD  CONSTRAINT [FK_MapSectors_Stores] FOREIGN KEY([StoreID])
REFERENCES [dbo].[Stores] ([ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[MapSectors] CHECK CONSTRAINT [FK_MapSectors_Stores]
GO
ALTER TABLE [dbo].[Marks]  WITH CHECK ADD  CONSTRAINT [FK_Marks_Shops] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Marks] CHECK CONSTRAINT [FK_Marks_Shops]
GO
ALTER TABLE [dbo].[Marks]  WITH CHECK ADD  CONSTRAINT [FK_Marks_Users] FOREIGN KEY([MarkAuthorID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Marks] CHECK CONSTRAINT [FK_Marks_Users]
GO
ALTER TABLE [dbo].[OperatorShops]  WITH CHECK ADD  CONSTRAINT [FK_OperatorShops_Shops] FOREIGN KEY([ShopID])
REFERENCES [dbo].[Shops] ([ID])
GO
ALTER TABLE [dbo].[OperatorShops] CHECK CONSTRAINT [FK_OperatorShops_Shops]
GO
ALTER TABLE [dbo].[OperatorShops]  WITH CHECK ADD  CONSTRAINT [FK_OperatorShops_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OperatorShops] CHECK CONSTRAINT [FK_OperatorShops_Users]
GO
ALTER TABLE [dbo].[OrderChars]  WITH CHECK ADD  CONSTRAINT [FK_OrderChars_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderChars] CHECK CONSTRAINT [FK_OrderChars_Orders]
GO
ALTER TABLE [dbo].[OrderedProducts]  WITH CHECK ADD  CONSTRAINT [FK_OrderedProducts_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderedProducts] CHECK CONSTRAINT [FK_OrderedProducts_Orders]
GO
ALTER TABLE [dbo].[OrderedProducts]  WITH CHECK ADD  CONSTRAINT [FK_OrderedProducts_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderedProducts] CHECK CONSTRAINT [FK_OrderedProducts_Products]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_OrderDelivery] FOREIGN KEY([AdressID])
REFERENCES [dbo].[DeliveryAddress] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_OrderDelivery]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Shops] FOREIGN KEY([ShopID])
REFERENCES [dbo].[Shops] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Shops]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users] FOREIGN KEY([AddedByID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users1] FOREIGN KEY([ConsumerID])
REFERENCES [dbo].[Consumers] ([ID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users1]
GO
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_Permissions_PermissionGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[PermissionGroups] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_Permissions_PermissionGroups]
GO
ALTER TABLE [dbo].[PermissionsForRoles]  WITH CHECK ADD  CONSTRAINT [FK_PermissionsForRoles_Permissions] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permissions] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PermissionsForRoles] CHECK CONSTRAINT [FK_PermissionsForRoles_Permissions]
GO
ALTER TABLE [dbo].[PermissionsForRoles]  WITH CHECK ADD  CONSTRAINT [FK_PermissionsForRoles_webpages_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[webpages_Roles] ([RoleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PermissionsForRoles] CHECK CONSTRAINT [FK_PermissionsForRoles_webpages_Roles]
GO
ALTER TABLE [dbo].[ProductChars]  WITH CHECK ADD  CONSTRAINT [FK_ProductChars_OrderedProducts] FOREIGN KEY([OrderedProductID])
REFERENCES [dbo].[OrderedProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductChars] CHECK CONSTRAINT [FK_ProductChars_OrderedProducts]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Users] FOREIGN KEY([OwnerID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Users]
GO
ALTER TABLE [dbo].[ShopManagers]  WITH CHECK ADD  CONSTRAINT [FK_ShopManagers_Managers] FOREIGN KEY([ManagerID])
REFERENCES [dbo].[Managers] ([ID])
GO
ALTER TABLE [dbo].[ShopManagers] CHECK CONSTRAINT [FK_ShopManagers_Managers]
GO
ALTER TABLE [dbo].[ShopManagers]  WITH CHECK ADD  CONSTRAINT [FK_ShopManagers_Shops] FOREIGN KEY([ShopID])
REFERENCES [dbo].[Shops] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShopManagers] CHECK CONSTRAINT [FK_ShopManagers_Shops]
GO
ALTER TABLE [dbo].[ShopProductChars]  WITH CHECK ADD  CONSTRAINT [FK_ShopProductChars_Shops] FOREIGN KEY([ShopID])
REFERENCES [dbo].[Shops] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShopProductChars] CHECK CONSTRAINT [FK_ShopProductChars_Shops]
GO
ALTER TABLE [dbo].[ShopProductChars]  WITH CHECK ADD  CONSTRAINT [FK_ShopProductChars_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[ShopProductChars] CHECK CONSTRAINT [FK_ShopProductChars_Users]
GO
ALTER TABLE [dbo].[Shops]  WITH CHECK ADD  CONSTRAINT [FK_Shop_Users] FOREIGN KEY([Owner])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Shops] CHECK CONSTRAINT [FK_Shop_Users]
GO
ALTER TABLE [dbo].[Shops]  WITH CHECK ADD  CONSTRAINT [FK_Shops_TaxPlanes] FOREIGN KEY([TaxPlanID])
REFERENCES [dbo].[TaxPlanes] ([ID])
GO
ALTER TABLE [dbo].[Shops] CHECK CONSTRAINT [FK_Shops_TaxPlanes]
GO
ALTER TABLE [dbo].[ShopSettings]  WITH CHECK ADD  CONSTRAINT [FK_ShopSettings_Shops] FOREIGN KEY([ShopID])
REFERENCES [dbo].[Shops] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShopSettings] CHECK CONSTRAINT [FK_ShopSettings_Shops]
GO
ALTER TABLE [dbo].[Stores]  WITH CHECK ADD  CONSTRAINT [FK_Stores_OrderDelivery] FOREIGN KEY([AdressID])
REFERENCES [dbo].[DeliveryAddress] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Stores] CHECK CONSTRAINT [FK_Stores_OrderDelivery]
GO
ALTER TABLE [dbo].[Stores]  WITH CHECK ADD  CONSTRAINT [FK_Stores_Shops] FOREIGN KEY([ShopID])
REFERENCES [dbo].[Shops] ([ID])
GO
ALTER TABLE [dbo].[Stores] CHECK CONSTRAINT [FK_Stores_Shops]
GO
ALTER TABLE [dbo].[UserAllowedRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserAllowedRoles_webpages_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[webpages_Roles] ([RoleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserAllowedRoles] CHECK CONSTRAINT [FK_UserAllowedRoles_webpages_Roles]
GO
ALTER TABLE [dbo].[UserPermissions]  WITH CHECK ADD  CONSTRAINT [FK_UserPermissions_Permissions] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permissions] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserPermissions] CHECK CONSTRAINT [FK_UserPermissions_Permissions]
GO
ALTER TABLE [dbo].[UserPermissions]  WITH CHECK ADD  CONSTRAINT [FK_UserPermissions_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserPermissions] CHECK CONSTRAINT [FK_UserPermissions_Users]
GO
ALTER TABLE [dbo].[webpages_Membership]  WITH CHECK ADD  CONSTRAINT [FK_webpages_Membership_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[webpages_Membership] CHECK CONSTRAINT [FK_webpages_Membership_Users]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_RoleId]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK_webpages_UsersInRoles_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [FK_webpages_UsersInRoles_Users]
GO
USE [master]
GO
ALTER DATABASE [1gb_trading] SET  READ_WRITE 
GO
