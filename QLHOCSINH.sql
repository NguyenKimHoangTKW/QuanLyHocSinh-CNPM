USE [QLHOCSINH]
GO
/****** Object:  Table [dbo].[HOCSINH]    Script Date: 06/05/2024 8:46:35 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HOCSINH](
	[MaHS] [varchar](10) NOT NULL,
	[HoTen] [nvarchar](50) NOT NULL,
	[GioiTinh] [bit] NOT NULL,
	[NgaySinh] [datetime] NOT NULL,
	[DiaChi] [nvarchar](150) NOT NULL,
	[DiemTB] [float] NOT NULL,
	[MaLop] [varchar](10) NOT NULL,
 CONSTRAINT [PK_HOCSINH] PRIMARY KEY CLUSTERED 
(
	[MaHS] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LOP]    Script Date: 06/05/2024 8:46:35 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LOP](
	[MaLop] [varchar](10) NOT NULL,
	[TenLop] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_LOP] PRIMARY KEY CLUSTERED 
(
	[MaLop] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[HOCSINH] ([MaHS], [HoTen], [GioiTinh], [NgaySinh], [DiaChi], [DiemTB], [MaLop]) VALUES (N'HS2', N'Đào Duy Thanh', 1, CAST(N'1990-03-13T00:00:00.000' AS DateTime), N'Lộc Ninh', 10, N'1')
INSERT [dbo].[HOCSINH] ([MaHS], [HoTen], [GioiTinh], [NgaySinh], [DiaChi], [DiemTB], [MaLop]) VALUES (N'HS3', N'Nguyễn Thành Long', 1, CAST(N'2003-03-18T00:00:00.000' AS DateTime), N'Lộc Ninh', 10, N'2')
INSERT [dbo].[HOCSINH] ([MaHS], [HoTen], [GioiTinh], [NgaySinh], [DiaChi], [DiemTB], [MaLop]) VALUES (N'HS4', N'Đào Thị Huỳnh Trang', 0, CAST(N'2003-03-18T00:00:00.000' AS DateTime), N'Lộc Ninh', 10, N'1')
INSERT [dbo].[HOCSINH] ([MaHS], [HoTen], [GioiTinh], [NgaySinh], [DiaChi], [DiemTB], [MaLop]) VALUES (N'HS5', N's', 0, CAST(N'2024-05-02T00:00:00.000' AS DateTime), N's', 2, N'2')
GO
INSERT [dbo].[LOP] ([MaLop], [TenLop]) VALUES (N'1', N'10A')
INSERT [dbo].[LOP] ([MaLop], [TenLop]) VALUES (N'2', N'10B')
INSERT [dbo].[LOP] ([MaLop], [TenLop]) VALUES (N'3', N'10C')
GO
ALTER TABLE [dbo].[HOCSINH]  WITH CHECK ADD  CONSTRAINT [FK_HOCSINH_LOP] FOREIGN KEY([MaLop])
REFERENCES [dbo].[LOP] ([MaLop])
GO
ALTER TABLE [dbo].[HOCSINH] CHECK CONSTRAINT [FK_HOCSINH_LOP]
GO
