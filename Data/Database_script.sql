IF DB_ID(N'QLHangHoa') IS NULL CREATE DATABASE QLHangHoa;
GO
USE QLHangHoa;
GO
IF OBJECT_ID(N'dbo.LoaiHang', N'U') IS NULL
BEGIN
 CREATE TABLE dbo.LoaiHang(
  MaLoai int IDENTITY(1,1) NOT NULL CONSTRAINT PK_LoaiHang PRIMARY KEY,
  TenLoai nvarchar(50) NOT NULL
 );
END;
GO
IF OBJECT_ID(N'dbo.HangHoa', N'U') IS NULL
BEGIN
 CREATE TABLE dbo.HangHoa(
  MaHang int IDENTITY(1,1) NOT NULL CONSTRAINT PK_HangHoa PRIMARY KEY,
  MaLoai int NOT NULL,
  TenHang nvarchar(50) NOT NULL,
  Gia numeric(18,0) NULL,
  Anh nvarchar(100) NULL,
  CONSTRAINT FK_HangHoa_LoaiHang FOREIGN KEY(MaLoai) REFERENCES dbo.LoaiHang(MaLoai)
 );
END;
GO
IF NOT EXISTS (SELECT 1 FROM dbo.LoaiHang)
BEGIN
 INSERT dbo.LoaiHang(TenLoai) VALUES
 (N'Điện thoại'),(N'Máy tính'),(N'Ti vi'),(N'Tủ lạnh'),(N'Phụ kiện');
END;
GO
IF NOT EXISTS (SELECT 1 FROM dbo.HangHoa)
BEGIN
 INSERT dbo.HangHoa(MaLoai,TenHang,Gia,Anh) VALUES
 (1,N'Samsung J8',1000,N'Img_1.jpg'),
 (1,N'Iphone 8',2000,N'Img_2.jpg'),
 (1,N'Nokia 8',1500,N'Img_3.jpg'),
 (2,N'Dell XPS',2500,N'Img_4.jpg'),
 (2,N'Sony SVT',2000,N'Img_5.jpg'),
 (3,N'Sony S1393',1800,N'Img_6.jpg'),
 (3,N'Hàng mới',1000,N'Img_1.jpg'),
 (3,N'Phụ kiện điện tử',1500,N'Img_2.jpg');
END;
GO
