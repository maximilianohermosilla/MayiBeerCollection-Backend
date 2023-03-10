USE [MayiBeerCollection]
GO
/****** Object:  Table [dbo].[Archivo]    Script Date: 10/1/2023 00:19:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Archivo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Archivo] [varbinary](max) NULL,
 CONSTRAINT [PK_Archivo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArchivoFilestream]    Script Date: 10/1/2023 00:19:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArchivoFilestream](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IDGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[RootDirectory] [varchar](max) NULL,
	[FileName] [varchar](max) NULL,
	[FileAttribute] [varchar](150) NULL,
	[FileCreateDate] [datetime] NULL,
	[FileSize] [numeric](10, 5) NULL,
	[FileStreamCol] [varbinary](max) FILESTREAM  NULL,
 CONSTRAINT [PK_ArchivoFilestream] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY] FILESTREAM_ON [Filestream MSSQL],
UNIQUE NONCLUSTERED 
(
	[IDGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] FILESTREAM_ON [Filestream MSSQL]
GO
/****** Object:  Table [dbo].[Cerveza]    Script Date: 10/1/2023 00:19:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cerveza](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[IBU] [int] NULL,
	[Alcohol] [float] NULL,
	[IdMarca] [int] NOT NULL,
	[IdEstilo] [int] NOT NULL,
	[IdCiudad] [int] NULL,
	[Observaciones] [varchar](200) NULL,
	[Contenido] [int] NOT NULL,
	[imagen] [varchar](max) NULL,
	[IdArchivo] [int] NULL,
 CONSTRAINT [PK_Cerveza] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ciudad]    Script Date: 10/1/2023 00:19:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ciudad](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[IdPais] [int] NOT NULL,
 CONSTRAINT [PK_Ciudad] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Estilo]    Script Date: 10/1/2023 00:19:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Estilo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[IdArchivo] [int] NULL,
 CONSTRAINT [PK_Estilo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Marca]    Script Date: 10/1/2023 00:19:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Marca](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[IdArchivo] [int] NULL,
 CONSTRAINT [PK_Marca] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pais]    Script Date: 10/1/2023 00:19:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pais](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[IdArchivo] [int] NULL,
 CONSTRAINT [PK_Pais] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cerveza]  WITH CHECK ADD  CONSTRAINT [FK_Cerveza_Archivo] FOREIGN KEY([IdArchivo])
REFERENCES [dbo].[Archivo] ([Id])
GO
ALTER TABLE [dbo].[Cerveza] CHECK CONSTRAINT [FK_Cerveza_Archivo]
GO
ALTER TABLE [dbo].[Cerveza]  WITH CHECK ADD  CONSTRAINT [FK_Cerveza_Ciudad] FOREIGN KEY([IdCiudad])
REFERENCES [dbo].[Ciudad] ([Id])
GO
ALTER TABLE [dbo].[Cerveza] CHECK CONSTRAINT [FK_Cerveza_Ciudad]
GO
ALTER TABLE [dbo].[Cerveza]  WITH CHECK ADD  CONSTRAINT [FK_Cerveza_Estilo] FOREIGN KEY([IdEstilo])
REFERENCES [dbo].[Estilo] ([Id])
GO
ALTER TABLE [dbo].[Cerveza] CHECK CONSTRAINT [FK_Cerveza_Estilo]
GO
ALTER TABLE [dbo].[Cerveza]  WITH CHECK ADD  CONSTRAINT [FK_Cerveza_Marca] FOREIGN KEY([IdMarca])
REFERENCES [dbo].[Marca] ([Id])
GO
ALTER TABLE [dbo].[Cerveza] CHECK CONSTRAINT [FK_Cerveza_Marca]
GO
ALTER TABLE [dbo].[Ciudad]  WITH CHECK ADD  CONSTRAINT [FK_Ciudad_Pais] FOREIGN KEY([IdPais])
REFERENCES [dbo].[Pais] ([Id])
GO
ALTER TABLE [dbo].[Ciudad] CHECK CONSTRAINT [FK_Ciudad_Pais]
GO
ALTER TABLE [dbo].[Estilo]  WITH CHECK ADD  CONSTRAINT [FK_Estilo_Archivo] FOREIGN KEY([IdArchivo])
REFERENCES [dbo].[Archivo] ([Id])
GO
ALTER TABLE [dbo].[Estilo] CHECK CONSTRAINT [FK_Estilo_Archivo]
GO
ALTER TABLE [dbo].[Marca]  WITH CHECK ADD  CONSTRAINT [FK_Marca_Archivo] FOREIGN KEY([IdArchivo])
REFERENCES [dbo].[Archivo] ([Id])
GO
ALTER TABLE [dbo].[Marca] CHECK CONSTRAINT [FK_Marca_Archivo]
GO
ALTER TABLE [dbo].[Pais]  WITH CHECK ADD  CONSTRAINT [FK_Pais_Archivo] FOREIGN KEY([IdArchivo])
REFERENCES [dbo].[Archivo] ([Id])
GO
ALTER TABLE [dbo].[Pais] CHECK CONSTRAINT [FK_Pais_Archivo]
GO
